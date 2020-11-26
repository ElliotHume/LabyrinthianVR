using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(NavMeshAgent))]
public class CasterAI : MonoBehaviour
{

    public float health = 100f;
    float maxHealth;

    public LayerMask whatIsGround, whatIsPlayer, whatIsSpell, sightBlockingMask;
    public AudioSource hitSound;
    GameObject player;
    CharacterController playerController;
    Vector3 playerPos;
    NavMeshAgent agent;
    Animator anim;

    // Combat variables
    bool currentlyAttacking = false, blocking = false, takingDamage = false;
    public Transform castingAnchorPoint, shieldAnchorPoint;
    CasterReaction reaction;

    //States
    public float sightRange = 10f, attackRange = 3f;
    bool targetInSightRange, targetInAttackRange, canSeeTarget, walking = false, inCombat = false;

    // Enemy Specific
    public List<CasterReaction> reactions;
    public List<string> weaknesses;
    public List<float> weaknessMultipliers;
    public UnityEvent onDeath;




    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        player = GameObject.Find("XR Rig");
        playerController = player.GetComponent<CharacterController>();
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
                // Debug.Log("Can see: "+raycastHit.transform.gameObject);
                canSeeTarget = raycastHit.transform.gameObject.tag == "Player";
            }
        }

        // Check if target is in attack range
        targetInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        // Set animation variable(s)
        anim.SetBool("Moving", walking);

        // Set combat state
        if (!inCombat && targetInSightRange && canSeeTarget) {
            inCombat = true;
            anim.SetBool("Combat", true);
        }

        // Detect any spells in the area
        SpellData spellInArea = DetectSpells();

        // Check if the spells in the area will hit me
        bool willGetHit = (spellInArea != null) && spellInArea.WillHitObject(gameObject);

        // Thinking logic
        if (health > 0f && inCombat) {
            if (willGetHit) {
                Block(spellInArea);
            } 
            
            if ((!targetInAttackRange || (targetInAttackRange && !canSeeTarget)) && !currentlyAttacking && !blocking && !takingDamage) {
                ChaseTarget();
            } else {
                AttackTarget();
            }
        }
    }

    SpellData DetectSpells() {
        RaycastHit raycastHit;
        if( Physics.SphereCast(transform.position, 2f, (playerPos - (transform.position+transform.up)), out raycastHit, 100f, whatIsSpell) ) {
            SpellData sd = raycastHit.transform.gameObject.GetComponent<SpellData>();
            // Debug.Log("Found Spell: "+sd.name);
            if (sd != null) return sd;
        }
        return null;
    }

    void Block(SpellData spell) {
        //Debug.Log("Try to block");

        // Do not Block if currently attacking, blocking, or taking damage
        if (currentlyAttacking || blocking || takingDamage) return;

        // Check if there is an applicable action available
        bool doesReact = false;
        reaction = reactions[0];
        foreach (CasterReaction r in reactions) {
            if (r.spell == spell.name && Random.value <= r.reactionChance) {
                doesReact = true;
                reaction = r;
            }
        }
        //Debug.Log("Found reaction: "+doesReact);

        // if a reaction is found and rolls a succesful reaction
        if (doesReact) {
            walking = false;
            agent.ResetPath();
            transform.LookAt(new Vector3 (playerPos.x, transform.position.y, playerPos.z));
            
            //Debug.Log("Blocking");
            blocking = true;
            
            Invoke(nameof(React), Random.Range(0.2f, 0.5f));
            Invoke(nameof(ResetBlock), reaction.blockTime);
        }
    }

    void React() {
        anim.SetBool("Blocking", true);
        anim.Play("Blocking");
        Transform anchorPoint = reaction.castType == "missile" ? castingAnchorPoint : shieldAnchorPoint;
        Instantiate(reaction.reactionSpell, anchorPoint.position, anchorPoint.rotation);
    }

    void ResetBlock() {
        blocking = false;
        anim.SetBool("Blocking", false);
    }

    void ChaseTarget() {
        // Do not chase if currently attacking
        if (currentlyAttacking || blocking || takingDamage) return;

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
        if (!currentlyAttacking && !blocking && !takingDamage) {
            anim.SetTrigger("Cast");
            currentlyAttacking = true;

            // Attack actions(s)
            

            // wait for the attack cooldown before attacking again
            // Invoke(nameof(ResetAttack), attackCooldown);
        }
    }

    void ResetAttack() {
        currentlyAttacking = false;
    }

    public void TakeDamage(float damage) {
        // Do not animate if damage is small (like a damage over time effect)
        if (damage >= 5) {
            // Interrupt any attack

            // Trigger animation
            anim.SetTrigger("GetHit");

            // Play hit sound, lerped pitch with the current health of the caster
            if (hitSound) {
                hitSound.pitch = Mathf.Lerp(2f,1f,health/maxHealth);
                hitSound.Play();
            }
        }

        inCombat = true;
        anim.SetBool("Combat", true);
        health -= damage;
        if (health <= 0f) {
            Die();
        }
    }

    public void TakeDamage(string spell, float damage) {
        if (weaknesses.Contains(spell)) {
            damage *= weaknessMultipliers[weaknesses.IndexOf(spell)];
        }
        TakeDamage(damage);
    }

    public void OutOfReach() {
        if (targetInAttackRange) {
            AttackTarget();
        } else {
            walking = false;
            agent.ResetPath();
        }
    }

    public void Die() {
        anim.Play("Die");
        walking = false;
        onDeath.Invoke();
        GetComponent<CapsuleCollider>().enabled = false;
        Destroy(gameObject, 30f);
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
