using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
    [System.Serializable]
    private struct SpawnInfo {
        public GameObject prefab;

        public float minSpawnTime;
        public float maxSpawnTime;
    }

    [SerializeField] private SpawnInfo[] powerUps = {
        new SpawnInfo {
            minSpawnTime = 7,
            maxSpawnTime = 15
        }
    };
    
    [Space(20)]
    [SerializeField] private SpawnInfo[] provisions = {
        new SpawnInfo {
        minSpawnTime = 5,
        maxSpawnTime = 14
        }
    };

    [Space(20)]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject enemyContainer;
    [SerializeField] List<GameObject> enemies = new List<GameObject>();
    [Space(20)]
    [SerializeField] private NewWaveUI newWaveUI;

    private HealthEntity playerHealth;
    private bool spawning = true;
    private int maxEnemies = 3;
    
    public int wave = 1;

    private void Start() {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null) {
            playerHealth = player.GetComponent<HealthEntity>();
            if (playerHealth != null)
                playerHealth.onDamaged += CheckForDeath;
        }
    }

    private void OnDestroy() {
        if (playerHealth != null)
            playerHealth.onDamaged -= CheckForDeath;
    }

    public void StartSpawning() {
        StartCoroutine(SpawnEnemyCoroutine());
        StartCoroutine(SpawnPowerUpCoroutine());
        StartCoroutine(SpawnProvisionCoroutine());
    }

    //first wave has 3 enemies. must kill them all and then can progress onto next wave
    //wave # displays in HUD
    //List of all the instances you've spawned, like List<GameObject>
    //or a List<YourComponentHere> You can check if that list's .Count > 0 or not

    //.Add(T item)
    //.Remove(T item) <-- returns a bool, cause it may OR may not find the item
    //you wanted to remove
    //.RemoveAt(int index)
    //and remember.Count instead of.Length
    private IEnumerator SpawnEnemyCoroutine() {
        StartCoroutine(newWaveUI.WaveUICoroutine());
        int lastEnemyWave = 3; //9
        int lastEnemyCount = 5; //11
        
        WaitForSeconds wait = new WaitForSeconds(3);
        
        yield return wait;
        while (spawning) {
            for (int i = 0; i < maxEnemies; i++) {
                enemies.Add(enemyPrefab);
                Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 8.5f, 0);
                GameObject newEnemy = Instantiate(enemyPrefab, posToSpawn, Quaternion.identity);
                newEnemy.transform.parent = enemyContainer.transform;
                WaitForSeconds wait5Secs = new WaitForSeconds(5.0f);
                yield return wait5Secs;
            }
            spawning = false;
            if (wave < lastEnemyWave && maxEnemies < lastEnemyCount && enemies.Count == 0) { //also see if there aren't any more enemies in scene
                enemies.Clear();
                wave++;
                maxEnemies++;
                StartCoroutine(newWaveUI.WaveUICoroutine());
                spawning = true;
                //waits before each wave
                yield return wait;
            }
        }
    }

    private IEnumerator SpawnPowerUpCoroutine() {
        WaitForSeconds wait = new WaitForSeconds(3);
        yield return wait;
        while (spawning) {
            int randPowerUp = Random.Range(0, powerUps.Length);
            WaitForSeconds waitRandom = new WaitForSeconds(Random.Range(powerUps[randPowerUp].minSpawnTime,
                powerUps[randPowerUp].maxSpawnTime + 1));
            yield return waitRandom;
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 8.5f, 0);
            Instantiate(powerUps[randPowerUp].prefab, posToSpawn, Quaternion.identity);
        }
    }

    private IEnumerator SpawnProvisionCoroutine() {
        WaitForSeconds wait = new WaitForSeconds(3);
        yield return wait;
        while (spawning) {
            int randProvision = Random.Range(0, provisions.Length);
            WaitForSeconds waitRandom = new WaitForSeconds(Random.Range(provisions[randProvision].minSpawnTime,
                provisions[randProvision].maxSpawnTime + 1));
            yield return waitRandom;
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 8.5f, 0);
            Instantiate(provisions[randProvision].prefab, posToSpawn, Quaternion.identity);
        }
    }

    private void CheckForDeath() {
        if (playerHealth.Health <= 0)
            OnPlayerDeath();
    }

    public void OnPlayerDeath() {
        spawning = false;
    }
}
