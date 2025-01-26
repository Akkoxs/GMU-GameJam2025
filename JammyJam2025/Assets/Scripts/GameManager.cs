using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // These two are totally not confusing as fuck
    public Player player;
    public Shroomaloom shroomaloom;
    public Transform spawnPoint1;
    public Transform spawnPoint2;
    public GameObject enemyPrefab;
    public float spawnInterval = 8f;
    public GrowthSerum serum;

    public int currentWave = 0;
    private int maxEnemiesPerWave = 4;
    private int enemiesSpawned = 0;
    private int enemiesAlive = 0;

    // void Start()
    // {
    //     StartNextWave();
    // }

    public void FirstWave(){ //this method is initiated in GrowthSerum script
        StartNextWave();
        //musical queue !
    }

    void StartNextWave()
    {
        currentWave++;
        enemiesSpawned = 0;
        enemiesAlive = 0;
        maxEnemiesPerWave += currentWave;
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (enemiesSpawned != maxEnemiesPerWave)
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
        if (enemiesAlive <= 0 && enemiesSpawned >= maxEnemiesPerWave) //when there is no one alive, and we spawned all enemies, start next wave.
        {
            StartNextWave();
        }
    }
}