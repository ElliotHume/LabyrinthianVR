using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{

    public float health = 100f;
    float maxHealth;

    public LayerMask whatIsGround, whatIsPlayer, sightBlockingMask;
    public AudioSource hitSound;
    public Weapon weapon, enrageWeapon;
    GameObject player;
    CharacterController playerController;
    Vector3 playerPos;
    NavMeshAgent agent;
    Animator anim;

    // Patrolling
    public bool doesPatrol = true;
    Vector3 walkPoint, startPoint;
    bool walkPointSet;
    public float walkPointRange = 10f, idleTime = 6f;

    //Attacking
    public bool doesEnrage = false;
    public int numberOfEnrageAttacks = 1;
    bool hasAlreadyEnraged = false, enraging = false;
    public float attackCooldown = 4f, damageAnimationTime = 1f, enrageAnimationTime = 1f;
    bool alreadyAttacked;

    //States
    public float sightRange = 10f, attackRange = 3f, startSpeed;
    bool targetInSightRange, targetInAttackRange, canSeeTarget, idling, walking = false, inCombat = false, slowed = false;

    // Enemy Specific
    public string weakness;
    public float weaknessMultiplier = 2f;
    public UnityEvent onDeath;




    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        player = GameObject.Find("XR Rig");
        playerController = player.GetComponent<CharacterController>();
        startPoint = transform.position;
        startSpeed = agent.speed;
        maxHealth = health;
    }

    void FixedUpdate()
    {
        // Get player controller position in worldspace
        playerPos = player.transform.TransformPoint(playerController.center);

        // Check if the target is in sight range
        targetInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

        // If in sight range, check if the target is obscured
        if (targetInSightRange || targetInAttackRange) {
            RaycastHit raycastHit;
            if( Physics.SphereCast(transform.position, 1f, (playerPos - (transform.position+transform.up)), out raycastHit, 100f, sightBlockingMask) ) {
                canSeeTarget = raycastHit.transform.gameObject.tag == "Player";
            }
        }

        // Check if target is in attack range
        targetInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        // Set animation variable(s)
        anim.SetBool("Walking", walking);

        // Thinking logic
        if (health > 0f) {
            if (!inCombat) {
                if (!targetInSightRange || (targetInSightRange && !canSeeTarget)) {
                    if (doesPatrol) Patrolling();
                } else {
                    inCombat = true;
                    ChaseTarget();
                }
            } else {
                if ((!targetInAttackRange && !alreadyAttacked) || (targetInAttackRange && !canSeeTarget && !alreadyAttacked)) {
                    ChaseTarget();
                } else {
                    AttackTarget();
                }
            }
        } else if (agent.hasPath) {
            agent.ResetPath();
        }
    }

    void Patrolling() {
        // If: no walkpoint is set and not already idling, idle
        // Else: walk to the set walkpoint
        if (!walkPointSet) {
            walking = false;
            if (!idling) {
                idling = true;
                Idle();
            }
        } else {
            walking = true;
            agent.SetDestination(walkPoint);
        }

        // If target is at the set walkpoint, find a new walkpoint
        if ((transform.position - walkPoint).magnitude < 1f) {
            walkPointSet = false;
        }
    }

    void Idle() {
        // Stop moving, and after a time, look for a new walkpoint
        agent.ResetPath();
        Invoke(nameof(SearchWalkPoint), idleTime);
    }

    void ChaseTarget() {
        // Do not chase if enraging
        if (enraging) return;

        // Walk towards target
        walking = true;

        // Find closest point on navmesh from the player controllers center, check if it is reachable
        NavMeshHit hit;
        NavMeshPath path = new NavMeshPath();
        if (NavMesh.SamplePosition(playerPos, out hit, 2f, NavMesh.AllAreas)){
            agent.CalculatePath(hit.position, path);
            if (path.status == NavMeshPathStatus.PathComplete) {
                agent.SetDestination(hit.position);
            } else {
                OutOfReach();
            }
        } else {
            OutOfReach();
        }
    }

    void AttackTarget() {
        // Stop moving and look at the target
        walking = false;
        agent.ResetPath();
        transform.LookAt(new Vector3 (playerPos.x, transform.position.y, playerPos.z));

        // If you are not currently attacking, attack target
        if (!alreadyAttacked && !enraging) {
            anim.SetTrigger("Attack");
            alreadyAttacked = true;

            // Attack actions(s)
            if (weapon != null) weapon.Attack();

            // wait for the attack cooldown before attacking again
            Invoke(nameof(ResetAttack), attackCooldown);
        }
    }

    void ResetAttack() {
        alreadyAttacked = false;
    }

    public void TakeDamage(float damage) {
        // Do not animate if damage is small (like a damage over time effect)
        if (damage >= 5) {
            // Interrupt any attack
            if (weapon != null && alreadyAttacked) weapon.Interrupt();


            anim.SetTrigger("TakeDamage");
            if (hitSound) {
                hitSound.pitch = Mathf.Lerp(2f,1f,health/maxHealth);
                hitSound.Play();
            }
            agent.speed = 0f;
            if (doesEnrage && health <= maxHealth/(4f - (float)numberOfEnrageAttacks) && !hasAlreadyEnraged) {
                Invoke(nameof(Enrage), damageAnimationTime);
            } else {
                Invoke(nameof(ResumeMovement), damageAnimationTime);
            }
        }

        inCombat = true;
        health -= damage;
        if (health <= 0f) {
            Die();
        }
    }

    public void TakeDamage(string spell, float damage) {
        if (spell == weakness) damage *= weaknessMultiplier;
        TakeDamage(damage);
    }

    public void Enrage() {
        // If you are not currently enraging, do so
        if (!enraging) {
            enraging = true;
            anim.Play("Enrage");

            // Stop moving and look at the target
            walking = false;
            agent.ResetPath();
            transform.LookAt(new Vector3 (playerPos.x, transform.position.y, playerPos.z));
            
            numberOfEnrageAttacks -= 1;

            // Attack actions(s)
            if (enrageWeapon != null) enrageWeapon.Attack();

            // wait for the enrage to finish before moving again
            Invoke(nameof(ResetEnrage), enrageAnimationTime);
            
            // If you have reached the max amount of enrages, stop enraging
            if (numberOfEnrageAttacks <= 0) hasAlreadyEnraged = true;
        }
    }

    public void OutOfReach() {
        if (targetInAttackRange) {
            AttackTarget();
        } else {
            walking = false;
            agent.ResetPath();
            if (doesEnrage && !enraging) {
                numberOfEnrageAttacks += 1;
                Enrage();
            }
        }
    }

    void ResumeMovement() {
        agent.speed = startSpeed;
        slowed = false;
    }

    void ResetEnrage() {
        enraging = false;
        ResumeMovement();
    }

    public void Slow(float duration){
        if (!slowed) {
            agent.speed /= 5f;
            Invoke(nameof(ResumeMovement), duration);
        }
    }

    public void Die() {
        anim.Play("Dead");
        walking = false;
        onDeath.Invoke();
        GetComponent<CapsuleCollider>().enabled = false;
        Destroy(gameObject, 30f);
    }

    void SearchWalkPoint() {
        // Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(startPoint.x + randomX, transform.position.y, startPoint.z + randomZ);

        // Check if the random point is on valid ground and is reachable by the agent
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(walkPoint, path);
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround) && path.status == NavMeshPathStatus.PathComplete) {
            walkPointSet = true;
        }

        idling = false;
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere for the sight range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        // Draw a red sphere for the attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
