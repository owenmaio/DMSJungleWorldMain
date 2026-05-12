using UnityEngine;
using UnityEngine.AI;

public class WaypointAI : MonoBehaviour
{
    public Transform[] waypoints;

    public float attackRange = 1.2f;
    public float damageCooldown = 1.8f;
    public int damageAmount = 5;

    private float nextDamageTime;

    private NavMeshAgent agent;
    private Animator animator;
    private int currentWaypoint = 0;

    private Transform player;
    private PlayerHealth playerHealth;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
            playerHealth = playerObject.GetComponent<PlayerHealth>();
        }

        if (waypoints.Length > 0)
        {
            agent.SetDestination(waypoints[currentWaypoint].position);
        }
    }

    void Update()
    {
        if (player != null && playerHealth != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackRange && Time.time >= nextDamageTime)
            {
                playerHealth.TakeDamage(damageAmount);
                nextDamageTime = Time.time + damageCooldown;
            }
        }

        if (waypoints.Length == 0) return;

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            currentWaypoint++;

            if (currentWaypoint < waypoints.Length)
            {
                agent.SetDestination(waypoints[currentWaypoint].position);
            }
            else
            {
                agent.isStopped = true;
                agent.ResetPath();

                if (animator != null)
                {
                    animator.SetBool("IsMoving", false);
                }
            }
        }
    }
}