using UnityEngine;
using UnityEngine.AI;

public class WaypointAI : MonoBehaviour
{
    public Transform[] waypoints;

    private NavMeshAgent agent;
    private int currentWaypoint = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (waypoints.Length > 0)
        {
            agent.SetDestination(waypoints[currentWaypoint].position);
        }
    }

    void Update()
    {
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
            }
        }
    }
}