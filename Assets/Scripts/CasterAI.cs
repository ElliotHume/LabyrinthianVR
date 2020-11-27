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
    public float sightRange = 10f, attackRange = 3f, damageAnimationTime = 2f;
    bool targetInSightRange, targetInAttackRange, canSeeTarget, walking = false, inCombat = false;
    int currentAttackIndex = 0;

    // Enemy Specific
    public enum AttackPatternActions {Missile = 0, Bolt = 1, Shield = 2, Dash = 3, Summon = 4, Throw = 5};
    public List<AttackPatternActions> attackPattern;
    public List<CasterReaction> reactions;
    public List<string> weaknesses;
    public List<float> weaknessMultipliers;
    public List<CasterWeapon> missileWeapons, boltWeapons, shieldWeapons;
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
        targetInSightRange = Vector3.Distance(transform.position, playerPos) <= sightRange;

        // Check if target is in attack range
        targetInAttackRange = Vector3.Distance(transform.position, playerPos) <= attackRange;

        // If in sight range, check if the target is obscured
        if (targetInSightRange || targetInAttackRange) {
            RaycastHit raycastHit;
            if( Physics.SphereCast(transform.position+transform.up, 0.5f, (playerPos - (transform.position+transform.up)), out raycastHit, 100f, sightBlockingMask) ) {
                // Debug.Log("Can see: "+raycastHit.transform.gameObject);
                canSeeTarget = raycastHit.transform.gameObject.tag == "Player";
            }
        }

        // Set animation variable(s)
        anim.SetBool("Moving", walking);

        // Set combat state
        if (!inCombat && targetInSightRange && canSeeTarget)
            inCombat = true;

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
        Collider[] spellColliders = Physics.OverlapSphere(transform.position+transform.forward*3f, 8f, whatIsSpell);
        if (spellColliders.Length > 0) {
            SpellData sd = spellColliders[0].gameObject.GetComponent<SpellData>();
            Debug.Log("Found Spell: "+sd.name);
            if (sd != null) return sd;
        }
        return null;

        // RaycastHit raycastHit;
        // if( Physics.SphereCast(transform.position, 2f, (playerPos - (transform.position+transform.up)), out raycastHit, 100f, whatIsSpell) ) {
        //     SpellData sd = raycastHit.transform.gameObject.GetComponent<SpellData>();
        //     Debug.Log("Found Spell: "+sd.name);
        //     if (sd != null) return sd;
        // }
        // return null;
    }

    void Block(SpellData spell) {
        Debug.Log("Try to block");

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
        Debug.Log("Found reaction: "+doesReact);

        // if a reaction is found and rolls a succesful reaction
        if (doesReact) {
            walking = false;
            agent.ResetPath();
            transform.LookAt(new Vector3 (playerPos.x, transform.position.y, playerPos.z));
            
            //Debug.Log("Blocking");
            blocking = true;
            
            StartCoroutine(React());
            Invoke(nameof(ResetBlock), reaction.blockTime);
        }
    }

    IEnumerator React() {
        // Reaction delay, mimicks human reaction time
        yield return new WaitForSeconds(0.22f);


        // Play animation
        if (reaction.castType == "missile" || reaction.castType == "barrier") {
            anim.SetBool("Blocking", true);
            anim.Play("Blocking");
        } else if (reaction.castType == "explosion"){
            anim.SetInteger("CastType", 5);
            anim.SetTrigger("Cast");
        } else if (reaction.castType == "dash") {
            Dash(0);
        }

        // Wait for the animation to finish, if applicable
        if (reaction.animationTime > 0f) {
            yield return new WaitForSeconds(reaction.animationTime);
        }
        
        // Spawn the counter spell at the appropriate anchor point
        if (reaction.reactionSpell != null) {
            Transform anchorPoint = reaction.castType == "missile" ? castingAnchorPoint : shieldAnchorPoint;
            Instantiate(reaction.reactionSpell, anchorPoint.position, anchorPoint.rotation);
        }
    }

    // Dash type of 0 is evade
    //              1 is charge
    //              2 is retreat
    //              3 is random
    void Dash(int dashType) {
        Debug.Log("Start Dash");
        anim.SetTrigger("Jump");
        Vector3 moveDirection = Vector3.zero;
        switch(dashType) {
            case 0:
                // When evading, goes to the left 60% of the time
                moveDirection = Random.value <= 0.6f ? -transform.right : transform.right;
                break;
            case 1:
                moveDirection = transform.forward;
                break;
            case 2:
                moveDirection = -transform.forward;
                break;
            case 3:
                float c = Random.value;
                moveDirection = c > 0.75f ? transform.forward : c > 0.5 ? -transform.forward : c > 0.25 ? transform.right : -transform.right;
                break;
        }

        StartCoroutine(Jump(moveDirection));
    }

    IEnumerator Jump(Vector3 direction) {
        yield return new WaitForSeconds(2f);

        float currentTime = 0f, duration = 1f;
        while (currentTime < duration) {
            transform.position += direction * 8f * Time.deltaTime;
            currentTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

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
            currentlyAttacking = true;

            // Attack actions(s)
            CasterWeapon weapon = null;
            switch (attackPattern[currentAttackIndex]) {
                case AttackPatternActions.Missile:
                    weapon = missileWeapons[Random.Range(0, missileWeapons.Count-1)];
                    break;
                case AttackPatternActions.Bolt:
                    weapon = boltWeapons[Random.Range(0, boltWeapons.Count-1)];
                    break;
                case AttackPatternActions.Shield:
                    weapon = shieldWeapons[Random.Range(0, shieldWeapons.Count-1)];
                    break;
                case AttackPatternActions.Dash:
                    Dash(3);
                    Invoke(nameof(ResetAttack), 5f);
                    break;
                case AttackPatternActions.Summon:
                    break;
                case AttackPatternActions.Throw:
                    break;
            }

            if (weapon != null) {
                anim.SetInteger("CastType", weapon.castAnimationType);
                anim.SetTrigger("Cast");
                StartCoroutine(CastWeapon(weapon));
            }
            
            currentAttackIndex += 1;
            if (currentAttackIndex > attackPattern.Count-1) currentAttackIndex = 0;
        }
    }

    IEnumerator CastWeapon(CasterWeapon weapon) {
        yield return new WaitForSeconds(weapon.animationTime);
        weapon.Fire(gameObject);
        yield return new WaitForSeconds(weapon.cooldown);
        currentlyAttacking = false;
    }

    void ResetAttack() {
        currentlyAttacking = false;
    }

    public void TakeDamage(float damage) {
        // Do not animate if damage is small (like a damage over time effect)
        if (damage >= 5) {
            // Trigger animation
            if (!takingDamage) {
                anim.SetTrigger("GetHit");
                takingDamage = true;
                Invoke("ResetDamage", damageAnimationTime);
            }

            // Play hit sound, lerped pitch with the current health of the caster
            if (hitSound) {
                hitSound.pitch = Mathf.Lerp(2f,1f,health/maxHealth);
                hitSound.Play();
            }
        }

        inCombat = true;
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

    void ResetDamage() {
        takingDamage = false;
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

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position+transform.up, 0.5f);
        Gizmos.DrawLine(transform.position+transform.up, playerPos);
    }
}
