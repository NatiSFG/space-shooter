using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveSpawner : WaveSystem {
    [SerializeField] private GameObject enemyContainer;
    [SerializeField] protected NewWaveDisplay newWaveDisplay;

    [Header("Enemies")]
    [SerializeField] private GameObject doubleBeamerPrefab;
    [SerializeField] private List<GameObject> dBEnemies = new List<GameObject>();

    [SerializeField] private GameObject chargerPrefab;
    [SerializeField] private List<GameObject> chargerEnemies = new List<GameObject>();
    
    [SerializeField] private GameObject spinnerPrefab;
    [SerializeField] private List<GameObject> sEnemies = new List<GameObject>();

    [SerializeField] private GameObject backShooterPrefab;
    [SerializeField] private List<GameObject> backShooterEnemies = new List<GameObject>();

    [SerializeField] private GameObject dodgerPrefab;
    [SerializeField] private List<GameObject> dodgerEnemies = new List<GameObject>();
    
    public bool AnyDoubleBeamerAlive => dBEnemies.Count > 0;
    public bool AnyChargerAlive => chargerEnemies.Count > 0;
    public bool AnySpinnerAlive => sEnemies.Count > 0;
    public bool AnyBackShooterAlive => backShooterEnemies.Count > 0;
    public bool AnyDodgerAlive => dodgerEnemies.Count > 0;

    private int maxDoubleBeamers = 3; //final max enemies is 11 on wave 9
    private int maxChargers = 2; //final max enemies is 8 on wave 9
    private int maxSpinners = 1; //final max enemies is 7 on wave 9
    private int maxBackShooters = 1; //final max is 6 enemies on wave 9
    private int maxDodgers = 1; //final max is 1? remains 1 throughout

    private int bossWave = 10;
    private GameObject dBEnemy;
    private GameObject chargerEnemy;
    private GameObject sEnemy;
    private GameObject backShooterEnemy;
    private GameObject dodgerEnemy;

    private float minXSpawnPoint = -8f;
    private float maxXSpawnPoint = 8f;
    private float topYSpawnPoint = 9f;
    public float MinXSpawnPoint => minXSpawnPoint;
    public float MaxXSpawnPoint => maxXSpawnPoint;
    public float TopYSpawnPoint => topYSpawnPoint;
    public bool IsRegularWave => wave < bossWave;
    public bool IsBossWave => wave == bossWave;
    public NewWaveDisplay NewWaveDisplay => newWaveDisplay;

    private void Update() {
        Debug.Log("double beamers: " + dBEnemies.Count + "\nchargers: " + chargerEnemies.Count);
    }

    public void SpawnEnemies() {
        StartCoroutine(SpawnDoubleBeamerCoroutine());
        StartCoroutine(SpawnDodgerCoroutine());
    }


    public IEnumerator SpawnDoubleBeamerCoroutine() {
        WaitForSeconds wait = new WaitForSeconds(3);
        WaitForSeconds inBetweenSpawn = new WaitForSeconds(5);

        while (IsRegularWave) {
            yield return wait;

            for (int i = 0; i < maxDoubleBeamers; i++) {
                Vector2 pos = new Vector2(Random.Range(minXSpawnPoint, maxXSpawnPoint), topYSpawnPoint);
                dBEnemy = Instantiate(doubleBeamerPrefab, pos, Quaternion.identity);
                dBEnemies.Add(dBEnemy);

                dBEnemy.transform.SetParent(enemyContainer.transform);
                yield return inBetweenSpawn;
            }

            yield return WaitForAllDoubleBeamerDefeated(); //this ends when they're all dead

            yield return wait;
            yield break;
        }
    }

    public IEnumerator SpawnChargerCoroutine() {
        WaitForSeconds initPause = new WaitForSeconds(4 + wave);
        WaitForSeconds wait = new WaitForSeconds(5 + wave);
        WaitForSeconds lastPause = new WaitForSeconds(3);

        while (IsRegularWave && wave >= 2) {
            yield return initPause;

            for (int i = 0; i < maxChargers; i++) {
                Vector2 pos = new Vector2(Random.Range(minXSpawnPoint, maxXSpawnPoint), topYSpawnPoint);
                chargerEnemy = Instantiate(chargerPrefab, pos, Quaternion.identity);
                chargerEnemies.Add(chargerEnemy);
                Debug.Log("max chargers: " + maxChargers);
                chargerEnemy.transform.SetParent(enemyContainer.transform);
                yield return wait;
            }

            yield return WaitForAllChargerDefeated();
            yield return WaitToStartNewWaveCoroutine();
            yield return lastPause;

            yield break;
        }
    }

    public IEnumerator SpawnSpinnerCoroutine() {
        WaitForSeconds initPause = new WaitForSeconds(4 + wave);
        WaitForSeconds wait = new WaitForSeconds(5 + wave);
        WaitForSeconds lastPause = new WaitForSeconds(3);

        while (IsRegularWave && wave >= 3) {
            yield return initPause;

            for (int i = 0; i < maxSpinners; i++) {
                Vector2 pos = new Vector2(Random.Range(minXSpawnPoint, maxXSpawnPoint), topYSpawnPoint);
                sEnemy = Instantiate(spinnerPrefab, pos, Quaternion.identity);
                sEnemies.Add(sEnemy);

                sEnemy.transform.SetParent(enemyContainer.transform);
                yield return wait;
            }

            yield return WaitForAllSpinnerDefeated();
            yield return WaitToStartNewWaveCoroutine();
            yield return lastPause;

            yield break;
        }
    }

    public IEnumerator SpawnBackShooterCoroutine() {
        WaitForSeconds initPause = new WaitForSeconds(3 + wave);
        WaitForSeconds wait = new WaitForSeconds(5 + wave);
        WaitForSeconds lastPause = new WaitForSeconds(3);

        //back shooter enemies start showing up at wave 4
        while (IsRegularWave && wave >= 4) {
            yield return initPause;

            for (int i = 0; i < maxBackShooters; i++) {
                Vector2 pos = new Vector2(Random.Range(minXSpawnPoint, maxXSpawnPoint), topYSpawnPoint);
                backShooterEnemy = Instantiate(backShooterPrefab, pos, Quaternion.identity);
                backShooterEnemies.Add(backShooterEnemy);

                backShooterEnemy.transform.SetParent(enemyContainer.transform);
                yield return wait;
            }

            yield return WaitForAllBackShooterDefeated();
            yield return WaitToStartNewWaveCoroutine();
            yield return lastPause;

            yield break;
        }
    }

    public IEnumerator SpawnDodgerCoroutine() {
        WaitForSeconds newWaveDelay = new WaitForSeconds(4 + wave);

        while (IsRegularWave) {
            yield return newWaveDelay;
            for (int i = 0; i < maxDodgers; i++) {
                Vector2 pos = new Vector2(Random.Range(MinXSpawnPoint, maxXSpawnPoint), topYSpawnPoint);
                dodgerEnemy = Instantiate(dodgerPrefab, pos, Quaternion.identity);
                dodgerEnemies.Add(dodgerEnemy);

                dodgerEnemy.transform.SetParent(enemyContainer.transform);
            }
            yield return WaitForAllDodgerDefeated();
            yield return WaitToStartNewWaveCoroutine();
            yield return newWaveDelay;

            yield break;
        }
    }

    private IEnumerator WaitToStartNewWaveCoroutine() {
        bool enemiesStillAlive = AnyDoubleBeamerAlive || AnySpinnerAlive || AnyChargerAlive
            || AnyBackShooterAlive || AnyDodgerAlive;
        while (enemiesStillAlive) {
            yield return null;
            enemiesStillAlive = AnyDoubleBeamerAlive || AnySpinnerAlive || AnyChargerAlive
            || AnyBackShooterAlive || AnyDodgerAlive;
        }
        if (!enemiesStillAlive)
            StartNewWave();
    }

    private IEnumerator StartNewWave() {
        yield return WaitToStartNewWaveCoroutine(); //this still runs forever because at this
        if (IsRegularWave && wave == 1) {
            wave++;
            StartCoroutine(NewWaveDisplay.ShowWaveText());
            maxDoubleBeamers++;
            StartCoroutine(SpawnChargerCoroutine());
            //StartCoroutine(SpawnDodgerCoroutine());
        } else if (IsRegularWave && wave == 2) {
            wave++;
            StartCoroutine(NewWaveDisplay.ShowWaveText());
            maxDoubleBeamers++;
            maxChargers++;
            //StartCoroutine(SpawnSpinnerCoroutine());
            StartCoroutine(SpawnChargerCoroutine());
            //StartCoroutine(SpawnDodgerCoroutine());
        } else if (IsRegularWave && wave == 3) {
            wave++;
            StartCoroutine(NewWaveDisplay.ShowWaveText());
            maxDoubleBeamers++;
            maxChargers++;
            maxSpinners++;
            //StartCoroutine(SpawnSpinnerCoroutine());
            StartCoroutine(SpawnChargerCoroutine());
            //StartCoroutine(SpawnBackShooterCoroutine());
            //StartCoroutine(SpawnDodgerCoroutine());
        } else if (IsRegularWave && wave >= 4) {
            wave++;
            StartCoroutine(NewWaveDisplay.ShowWaveText());
            maxDoubleBeamers++;
            maxChargers++;
            maxSpinners++;
            maxBackShooters++;
            //StartCoroutine(SpawnSpinnerCoroutine());
            StartCoroutine(SpawnChargerCoroutine());
            //StartCoroutine(SpawnBackShooterCoroutine());
            //StartCoroutine(SpawnDodgerCoroutine());
        } else if (wave == 9) {
            wave++;
            StartCoroutine(NewWaveDisplay.ShowWaveText());
            StartCoroutine(BossWaveCoroutine());
        } else if (IsPlayerDefeated)
            yield break;
    }

    private IEnumerator WaitForAllDoubleBeamerDefeated() {
        while (AnyDoubleBeamerAlive) {
            for (int i = dBEnemies.Count - 1; i >= 0; i--) {
                if (dBEnemies[i] == null)
                    dBEnemies.RemoveAt(i);
            }
            yield return null;
        }
    }

    private IEnumerator WaitForAllChargerDefeated() {
        while (AnyChargerAlive) {
            for (int i = chargerEnemies.Count - 1; i >= 0; i--) {
                if (chargerEnemies[i] == null)
                    chargerEnemies.RemoveAt(i);
            }
            yield return null;
        }
    }

    private IEnumerator WaitForAllSpinnerDefeated() {
        while (AnySpinnerAlive) {
            for (int i = sEnemies.Count - 1; i >= 0; i--) {
                if (sEnemies[i] == null)
                    sEnemies.RemoveAt(i);
            }
            yield return null;
        }
    }

    private IEnumerator WaitForAllBackShooterDefeated() {
        while (AnyBackShooterAlive) {
            for (int i = backShooterEnemies.Count - 1; i >= 0; i--) {
                if (backShooterEnemies[i] == null)
                    backShooterEnemies.RemoveAt(i);
            }
            yield return null;
        }
    }

    private IEnumerator WaitForAllDodgerDefeated() {
        while (AnyDodgerAlive) {
            for (int i = dodgerEnemies.Count - 1; i >= 0; i--) {
                if (dodgerEnemies[i] == null)
                    dodgerEnemies.RemoveAt(i);
            }
            yield return null;
        }
    }

    public IEnumerator BossWaveCoroutine() {
        if (IsBossWave)
            StartCoroutine(NewWaveDisplay.ShowWaveText());

        //TODO:
        yield break;
    }
}
