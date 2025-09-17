using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour {
    // These two are totally not confusing as fuck
    public Player player;
    public Shroomaloom shroomaloom;
    public Transform spawnPoint1;
    public Transform spawnPoint2;
    public GameObject enemyPrefab;
    public GrowthSerum serum;

    public int currentWave = 0;
    private int maxEnemiesPerWave = 4;
    private int enemiesSpawned = 1;
    private int enemiesAlive = 1;

    [Header("Ending Testing ")]
    [SerializeField] bool enable_DevTestEnding = true; //change to false l8r


    public void Update() {
        if (serum.droppedOffSerum) {
            WaveCycle(currentWave);
        }
    }

    public void FirstWave() { //this method is initiated in GrowthSerum script
        StartNextWave();
        //musical queue !
    }

    void StartNextWave() {
        currentWave++;
        enemiesSpawned = 1;
        enemiesAlive = 1;
        maxEnemiesPerWave += currentWave;
        //StartCoroutine(SpawnEnemies());
    }

    public IEnumerator WaveCycle(int currWave) {
        switch (currWave) {
            case 1: //one enemy from either side 
                SpawnEnemy('L', 0f);
                SpawnEnemy('R', 0f);
                currentWave++;
                break;
            case 2: // 3 enemies from left, and one from the right
                SpawnEnemy('L', 0f);
                SpawnEnemy('L', 0.75f);
                SpawnEnemy('L', 1.5f);
                SpawnEnemy('R', 7f);
                currentWave++;
                break;
            case 3: //
                currentWave++;
                break;
            case 4: //
                currentWave++;
                break;
            case 5: //
                currentWave++;
                break;
            case 6: //
                currentWave++;
                break;
            case 7: //
                currentWave++;
                break;
            case 8: //
                currentWave++;
                break;

            case 9: //ending
                EndingState();
                break;

        }
        yield return null;
    }


    public IEnumerator SpawnEnemy(char side, float delay) {
        yield return new WaitForSeconds(delay);
        switch (side) {
            case 'L':
                Instantiate(enemyPrefab, spawnPoint1.position, spawnPoint1.rotation);
                break;

            case 'R':
                Instantiate(enemyPrefab, spawnPoint2.position, spawnPoint2.rotation);
                break;
        }
        enemiesSpawned++;
        enemiesAlive++;
    }

    public void OnEnemyKilled() {
        enemiesAlive--;
        if (enemiesAlive <= 1 && (enemiesSpawned >= maxEnemiesPerWave)) //when there is no one alive, and we spawned all enemies, start next wave.
        {
            StartNextWave();
        }
    }
    
    public void EndingState() {
        //if ending conditions met or DevTestOK!

        //set the height of the thingy 
        //set the current wave, etc.
        //no enemies 
    }

}