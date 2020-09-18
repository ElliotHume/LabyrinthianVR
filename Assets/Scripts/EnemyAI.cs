using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{

    public float health = 100f;
    public LayerMask whatIsGround, whatIsPlayer;
    Transform target;
    NavMeshAgent agent;
    Animator anim;

    // Patrolling
    Vector3 walkPoint, startPoint;
    bool walkPointSet;
    public float walkPointRange = 10f, idleTime = 6f;

    //Attacking
    public float attackCooldown = 1f;
    bool alreadyAttacked;

    //States
    public float sightRange = 10f, attackRange = 3f, startSpeed;
    bool targetInSightRange, targetInAttackRange, idling, walking;




    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        target = GameObject.Find("XR Rig").transform;
        startPoint = transform.position;
        startSpeed = agent.speed;
    }

    // Update is called once per frame
    void Update()
    {
        // Check for sight and attack range
        targetInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        targetInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!targetInSightRange && !targetInAttackRange) Patrolling();
        if (targetInSightRange && !targetInAttackRange) ChaseTarget();
        if (targetInSightRange && targetInAttackRange) AttackTarget();

        anim.SetBool("Walking", walking);
        anim.SetBool("Dead", health <= 0);
    }

    void Patrolling() {
        if (!walkPointSet) {
            walking = false;
            if (!idling) {
                idling = true;
                Idle();
            }
        } else {
            print("Patrolling");
            walking = true;
            agent.SetDestination(walkPoint);
        }

        if ((transform.position - walkPoint).magnitude < 1f) {
            walkPointSet = false;
        }
    }

    void Idle() {
        print("Idling");
        agent.ResetPath();
        Invoke(nameof(SearchWalkPoint), idleTime);
    }

    void ChaseTarget() {
        print("Chasing");
        walking = true;
        agent.SetDestination(target.position);
    }

    void AttackTarget() {
        print("Attacking");
        walking = false;

        //agent.SetDestination(transform.position);
        agent.ResetPath();
        transform.LookAt(target.position);

        if (!alreadyAttacked) {
            // Attack code
            Debug.Log("Attack Target");
            anim.SetTrigger("Attack");

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), attackCooldown);
        }
    }

    void ResetAttack() {
        alreadyAttacked = false;
    }

    public void TakeDamage(float damage) {
        print("Take Damage: "+damage+"     health: "+health);
        anim.SetTrigger("TakeDamage");
        health -= damage;

        agent.speed = 0f;
        Invoke(nameof(ResumeMovement), 1f);


        if (health <= 0f) {
            Die();
        }
    }

    void ResumeMovement() {
        agent.speed = startSpeed;
    }

    public void Die() {
        // Death code

        Destroy(gameObject, 60f);
    }

    void SearchWalkPoint() {
        Debug.Log("Trying to find walkpoint");
        // Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(startPoint.x + randomX, transform.position.y, startPoint.z + randomZ);

        // Check if the random point is on valid ground and is reachable by the agent
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(target.position, path);
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround) && path.status == NavMeshPathStatus.PathComplete) {
            Debug.Log("Walkpoint Found");
            walkPointSet = true;
        }
        idling = false;
    }
}
