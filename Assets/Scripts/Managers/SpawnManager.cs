using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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
    [SerializeField] private List<GameObject> enemies = new List<GameObject>();
    [Space(20)]
    [SerializeField] private WaveText waveUI;

    [Space(20)]
    [SerializeField] private int wave = 1;

    private HealthEntity playerHealth;
    
    private int maxEnemies = 3;
    private int bossWave = 10;
    private int finalMaxEnemies = 11;
    private bool isPlayerDefeated = false;

    public int Wave => wave;
    public bool IsRegularWave => wave < bossWave;
    public bool IsBossWave => wave == bossWave;

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
    //      or a List<YourComponentHere> You can check if that list's .Count > 0 or not

    //.Add(T item)
    //.Remove(T item) <-- returns a bool, cause it may OR may not find the item
    //      you wanted to remove
    //.RemoveAt(int index)
    //and remember.Count instead of.Length

    //first wave works, but then no more enemies spawn and the New Wave UI doesn't get updated or enable to display. Add Debug lines
    private IEnumerator SpawnEnemyCoroutine() {
        WaitForSeconds wait3Sec = new WaitForSeconds(3);
        WaitForSeconds wait5Sec = new WaitForSeconds(5);

        while (IsRegularWave) {
            StartCoroutine(waveUI.ShowWaveText());
            yield return wait3Sec;

            for (int i = 0; i < maxEnemies; i++) {
                Vector3 pos = new Vector3(Random.Range(-8f, 8f), 8.5f, 0);
                GameObject enemy = Instantiate(enemyPrefab, pos, Quaternion.identity);
                enemies.Add(enemy);

                enemy.transform.SetParent(enemyContainer.transform);
                yield return wait5Sec;
            }

            yield return WaitForAllEnemiesDefeated();
            yield return wait3Sec;

            wave++;
            maxEnemies++;

            if (isPlayerDefeated)
                yield break;
        }

        yield return BossWaveCoroutine();
    }

    private IEnumerator WaitForAllEnemiesDefeated() {
        while (enemies.Count > 0) {
            //NOTE: Here, we're removing enemies from the list as they are defeated.
            for (int i = enemies.Count - 1; i >= 0; i--) {
                if (enemies[i] == null)
                    enemies.RemoveAt(i);
            }
            yield return null;
        }
    }

    private IEnumerator BossWaveCoroutine() {
        StartCoroutine(waveUI.ShowWaveText());

        //TODO:
        yield break;
    }

    private IEnumerator SpawnPowerUpCoroutine() {
        WaitForSeconds wait = new WaitForSeconds(3);
        yield return wait;
        while (!isPlayerDefeated) {
            int randPowerUp = Random.Range(0, powerUps.Length);
            WaitForSeconds waitRandom = new WaitForSeconds(Random.Range(powerUps[randPowerUp].minSpawnTime,
                powerUps[randPowerUp].maxSpawnTime + 1));
            yield return waitRandom;
            Vector3 pos = new Vector3(Random.Range(-8f, 8f), 8.5f, 0);
            Instantiate(powerUps[randPowerUp].prefab, pos, Quaternion.identity);
        }
    }

    private IEnumerator SpawnProvisionCoroutine() {
        WaitForSeconds wait = new WaitForSeconds(3);
        yield return wait;
        while (!isPlayerDefeated) {
            int randProvision = Random.Range(0, provisions.Length);
            WaitForSeconds waitRandom = new WaitForSeconds(Random.Range(provisions[randProvision].minSpawnTime,
                provisions[randProvision].maxSpawnTime + 1));
            yield return waitRandom;
            Vector3 pos = new Vector3(Random.Range(-8f, 8f), 8.5f, 0);
            Instantiate(provisions[randProvision].prefab, pos, Quaternion.identity);
        }
    }

    private void CheckForDeath() {
        if (playerHealth.Health <= 0)
            OnPlayerDeath();
    }

    public void OnPlayerDeath() {
        isPlayerDefeated = true;
    }
}
