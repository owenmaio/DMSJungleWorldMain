using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    public Transform[] leftPath;
    public Transform[] rightPath;

    public float spawnDelay = 2f;
    public float spawnRate = 5f;

    public float enemySpeed = 15f;
    public float enemyAcceleration = 20f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), spawnDelay, spawnRate);
    }

    void SpawnEnemy()
    {
        GameObject newEnemy = Instantiate(enemyPrefab, transform.position, transform.rotation);

        UnityEngine.AI.NavMeshAgent agent = newEnemy.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null)
        {
            agent.speed = enemySpeed;
            agent.acceleration = enemyAcceleration;
        }

        if (newEnemy.GetComponent<EnemyHealth>() == null)
        {
            newEnemy.AddComponent<EnemyHealth>();
        }

        WaypointAI ai = newEnemy.GetComponent<WaypointAI>();

        if (ai != null)
        {
            if (Random.value < 0.5f)
            {
                ai.waypoints = leftPath;
            }
            else
            {
                ai.waypoints = rightPath;
            }
        }
    }
}