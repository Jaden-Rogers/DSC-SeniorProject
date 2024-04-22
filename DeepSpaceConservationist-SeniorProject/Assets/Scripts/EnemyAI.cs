using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    //Variables
    public NavMeshAgent agent;
    public Transform player;
    public Transform firePoint;
    public LayerMask whatIsGround, whatIsPlayer;
    public GameObject projectile;
    public int health;
    public bool stunnable = true;
    public int timesStunned = 0;
    private LevelLoader levelLoader;

    //Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        levelLoader = GameObject.FindObjectOfType<LevelLoader>();
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }
    // Update is called once per frame
    void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        if (playerInSightRange && !playerInAttackRange || health < 5 && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange || health < 5 && playerInAttackRange) AttackPlayer();

        if (timesStunned > 3)
        {
            levelLoader.LoadNextLevel();
        }
    }

    private void Patrolling()
    {
        //Debug.Log("Patrolling");
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }

    }
    private void SearchWalkPoint()
    {
        //Debug.Log("Searching for walk point");
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }
    private void ChasePlayer()
    {
        //Debug.Log("Chasing player");
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        //Debug.Log("Attacking player");
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            //Attack code here
            Rigidbody rb = Instantiate(projectile, firePoint.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        //Take damage
        health -= damage;
        Debug.Log("Enemy health = " + health);
        if (health <= 0)
        {
            if (stunnable)
            {
                StunEnemy();
            }
            else
            { 
                Die();
            }
        }

    }

    private void StunEnemy()
    {
        //Stun enemy
        health = 1000;
        timesStunned += 1;
        sightRange += 100;
        agent.speed += 5;
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        StartCoroutine(Recover());
    }

    private IEnumerator Recover()
    {
        //Recover enemy
        yield return new WaitForSeconds(5f);
        agent.isStopped = false;
        health = 5;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
