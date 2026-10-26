using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;
using UnityEngine.Audio;

public class EnemyAi : MonoBehaviour
{
    private static bool navMeshBuilt;

    public NavMeshAgent agent;
    public GameObject projectile;
    private AudioManager audioManager;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public float health;

    //Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    public float shootSpeed;
    bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        GameObject playerObject = GameObject.Find("PlayerObj");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        agent = GetComponent<NavMeshAgent>();

        GameObject audioObject = GameObject.FindGameObjectWithTag("Audio");
        if (audioObject != null)
        {
            audioManager = audioObject.GetComponent<AudioManager>();
        }
    }

    private void Update()
    {
        // Wait until the agent is on a valid NavMesh
        if (!agent.isOnNavMesh)
            return;
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange)
        {
            Patroling();
        }
        else if (playerInAttackRange && CanSeePlayer())
        {
            AttackPlayer();
        }
        else
        {
            ChasePlayer();
        }
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();
        
        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        // Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        
        // Find a valid point on the NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(walkPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            walkPoint = hit.position;
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private bool CanSeePlayer()
    {
        Vector3 origin = transform.position + Vector3.up * 1f;
        Vector3 target = player.position + Vector3.up * 1f;

        Vector3 direction = target - origin;

        RaycastHit hit;

        if (Physics.Raycast(
            origin,
            direction.normalized,
            out hit,
            direction.magnitude,
            ~0,
            QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.GetComponentInParent<PlayerHealth>() != null)
            {
                return true;
            }
        }

        return false;
    }

    private void AttackPlayer()
    {
        // Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        // Make the enemy look at the player
        Vector3 lookDirection = (player.position - transform.position).normalized;
        // Keep the enemy's y rotation unchanged
        lookDirection.y = 0f; 

        if (lookDirection != Vector3.zero) // Check to avoid zero-length vector
        {
            Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // Smooth rotation
        }

        if (!alreadyAttacked && CanSeePlayer())
        {
            // Attack code here
            ShootPlayer();
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ShootPlayer()
    {
        // Spawn bullet above enemy, slightly forward
        Vector3 spawnPosition = transform.position
            + Vector3.up * 1f
            + transform.forward * 0.7f;

        // Aim toward player
        Vector3 target = player.position + Vector3.up * 1f;

        Vector3 direction = (target - spawnPosition).normalized;

        // Create bullet
        GameObject bullet = Instantiate(
            projectile, spawnPosition, Quaternion.identity);

        // Connect bullet to player health
        Damage bulletDamage = bullet.GetComponent<Damage>();

        if (bulletDamage != null)
        {
            bulletDamage.playerHealth =
                player.GetComponentInParent<PlayerHealth>();
        }

        // Move bullet toward player
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = direction * shootSpeed;
        }

        // Play shooting sound
        if (audioManager != null)
        {
            audioManager.PlaySFX(audioManager.Shoot);
        }

        // Destroy bullet after 5 seconds
        Destroy(bullet, 5f);
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }


}
