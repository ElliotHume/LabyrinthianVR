using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{

    public float health = 100f;
    float maxHealth;

    public LayerMask whatIsGround, whatIsPlayer;
    public AudioSource hitSound;
    public Weapon weapon;
    Transform target;
    NavMeshAgent agent;
    Animator anim;

    // Patrolling
    public bool doesPatrol = true;
    Vector3 walkPoint, startPoint;
    bool walkPointSet;
    public float walkPointRange = 10f, idleTime = 6f;

    //Attacking
    public float attackCooldown = 4f;
    bool alreadyAttacked;

    //States
    public float sightRange = 10f, attackRange = 3f, startSpeed;
    bool targetInSightRange, targetInAttackRange, canSeeTarget, idling, walking = false, inCombat = false, slowed = false;




    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        target = GameObject.Find("XR Rig").transform;
        startPoint = transform.position;
        startSpeed = agent.speed;
        maxHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the target is in sight range
        targetInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

        // If in sight range, check if the target is not obscured
        if (targetInSightRange) {
            RaycastHit raycastHit;
            if( Physics.Raycast( transform.position, (target.position - transform.position), out raycastHit, 100f ) ) {
                Debug.Log("Raycast hit: "+raycastHit.transform.gameObject);
                canSeeTarget = raycastHit.transform == target || raycastHit.transform.gameObject.tag == "Player";
            }
        }

        // Check if target is in attack range
        targetInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        // Set animation variable(s)
        anim.SetBool("Walking", walking);

        // Thinking logic
        if (health > 0f) {
            if (!inCombat) {
                if ((!targetInSightRange || (targetInSightRange && !canSeeTarget)) && !targetInAttackRange) {
                    if (doesPatrol) Patrolling();
                } else {
                    inCombat = true;
                    ChaseTarget();
                }
            } else {
                if (!targetInAttackRange && !alreadyAttacked) {
                    ChaseTarget();
                } else {
                    AttackTarget();
                }

                if ( inCombat && !targetInSightRange && !canSeeTarget) inCombat = false;
            }
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
        // Walk towards target
        walking = true;
        agent.SetDestination(target.position);
    }

    void AttackTarget() {
        // Stop moving and look at the target
        walking = false;
        agent.ResetPath();
        transform.LookAt(target.position);

        // If you are not currently attacking, attack target
        if (!alreadyAttacked) {
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
        anim.SetTrigger("TakeDamage");
        health -= damage;
        inCombat = true;

        if (hitSound) {
            hitSound.pitch = Mathf.Lerp(1f,2f,health/maxHealth);
            hitSound.Play();
        }

        agent.speed = 0f;
        Invoke(nameof(ResumeMovement), 1f);


        if (health <= 0f) {
            Die();
        }
    }

    void ResumeMovement() {
        agent.speed = startSpeed;
        slowed = false;
    }

    public void Slow(float duration){
        if (!slowed) {
            agent.speed /= 4;
            Invoke(nameof(ResumeMovement), duration);
        }
    }

    public void Die() {
        anim.SetBool("Dead", true);
        Destroy(gameObject, 60f);
    }

    void SearchWalkPoint() {
        // Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(startPoint.x + randomX, transform.position.y, startPoint.z + randomZ);

        // Check if the random point is on valid ground and is reachable by the agent
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(target.position, path);
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround) && path.status == NavMeshPathStatus.PathComplete) {
            walkPointSet = true;
        }

        idling = false;
    }
}
