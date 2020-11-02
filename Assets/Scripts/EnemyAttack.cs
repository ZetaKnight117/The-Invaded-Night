using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsHover, whatIsPlayer;

    public float health;

    //Patroling
    public Vector3 hoverPoint;
    bool hoverPointSet;
    public float hoverPointRange;

    //Attacking
    public float timeBetwwenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    public void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!player && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    private void Patroling()
    {
        if (!hoverPointSet) SearchHoverPoint();

        if (!hoverPointSet)
            agent.SetDestination(hoverPoint);

        Vector3 distanceToHoverPoint = transform.position - hoverPoint;

        //Hoverpoint reached
        if (distanceToHoverPoint.magnitude < 1f)
            hoverPointSet = false;
    }

    private void SearchHoverPoint()
    {
        // Calculate random point range
        float RandomZ = Random.Range(-hoverPointRange, hoverPointRange);
        float RandomX = Random.Range(-hoverPointRange, hoverPointRange);

        hoverPoint = new Vector3(transform.position.x + RandomX, transform.position.y, transform.position.z + RandomZ);
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();

            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetwwenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
