using UnityEngine;
using UnityEngine.AI;

public class WaypointAI : MonoBehaviour
{
    public Transform[] waypoints;

    public float attackRange = 1.4f;
    public float damageCooldown = 2f;
    public int damageAmount = 5;

    private float nextDamageTime;

    private NavMeshAgent agent;
    private Animator animator;
    private int currentWaypoint = 0;

    private Transform player;
    private PlayerHealth playerHealth;

    private bool fightingPlayer = false;
    private bool reachedFinalWaypoint = false;

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
            agent.isStopped = false;
            agent.SetDestination(waypoints[currentWaypoint].position);
        }
    }

    void Update()
    {
        if (reachedFinalWaypoint) return;

        CheckForPlayer();

        if (fightingPlayer) return;

        FollowWaypoints();

        if (animator != null)
        {
            animator.SetBool("IsMoving", agent.velocity.magnitude > 0.1f);
        }
    }

    void CheckForPlayer()
    {
        if (player == null || playerHealth == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            fightingPlayer = true;

            agent.isStopped = true;

            if (animator != null)
            {
                animator.SetBool("IsMoving", false);
            }

            FacePlayer();

            if (Time.time >= nextDamageTime)
            {
                if (animator != null)
                {
                    animator.SetTrigger("Attack");
                }

                playerHealth.TakeDamage(damageAmount);
                nextDamageTime = Time.time + damageCooldown;
            }
        }
        else
        {
            if (fightingPlayer)
            {
                fightingPlayer = false;
                agent.isStopped = false;

                if (currentWaypoint < waypoints.Length)
                {
                    agent.SetDestination(waypoints[currentWaypoint].position);
                }
            }
        }
    }

    void FacePlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0;

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 8f);
        }
    }

    void FollowWaypoints()
    {
        if (waypoints.Length == 0) return;
        if (agent.pathPending) return;

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            currentWaypoint++;

            if (currentWaypoint < waypoints.Length)
            {
                agent.SetDestination(waypoints[currentWaypoint].position);
            }
            else
            {
                reachedFinalWaypoint = true;
                agent.isStopped = true;

                if (animator != null)
                {
                    animator.SetBool("IsMoving", false);
                }
            }
        }
    }
}