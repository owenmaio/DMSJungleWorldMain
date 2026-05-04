using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] waypoints;
    public float spawnDelay = 2f;
    public float spawnRate = 5f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), spawnDelay, spawnRate);
    }

    void SpawnEnemy()
    {
        GameObject newEnemy = Instantiate(enemyPrefab, transform.position, transform.rotation);

        WaypointAI ai = newEnemy.GetComponent<WaypointAI>();

        if (ai != null)
        {
            ai.waypoints = waypoints;
        }
    }
}
