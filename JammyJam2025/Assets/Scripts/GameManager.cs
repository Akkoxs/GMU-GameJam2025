using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // These two are totally not confusing as fuck
    public Player playerData;
    public Shroomaloom shroomaloomData;
    public GameObject player;
    public GameObject shroomaloom;
    public Transform spawnPoint1;
    public Transform spawnPoint2;
    public GameObject enemyPrefab;
    public float spawnInterval = 2f;

    private int currentWave = 0;
    private int maxEnemiesPerWave = 10;
    private int enemiesSpawned = 0;
    private int enemiesAlive = 0;

    void Start()
    {
        StartNextWave();
    }

    void StartNextWave()
    {
        currentWave++;
        enemiesSpawned = 0;
        enemiesAlive = 0;
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (enemiesSpawned < maxEnemiesPerWave)
        {
            SpawnEnemy();
            enemiesSpawned++;
            enemiesAlive++;
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        Transform spawnPoint = (Random.value > 0.5f) ? spawnPoint1 : spawnPoint2;
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    public void OnEnemyKilled()
    {
        enemiesAlive--;
        if (enemiesAlive <= 0 && enemiesSpawned >= maxEnemiesPerWave)
        {
            StartNextWave();
        }
    }
}