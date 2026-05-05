using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    public Transform[] leftPath;
    public Transform[] rightPath;

    public float spawnDelay = 2f;
    public float spawnRate = 5f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), spawnDelay, spawnRate);
    }

    void SpawnEnemy()
    {
        GameObject newEnemy = Instantiate(enemyPrefab, transform.position, transform.rotation);

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