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
    
    private int maxDBEnemies = 3; //final max enemies is 11 on wave 9
    private int maxChargerEnemies = 2; //final max enemies is 8 on wave 9
    private int maxSpinnerEnemies = 1; //final max enemies is 7 on wave 9
    private int maxBackShooterEnemies = 1; //final max is 6 enemies on wave 9

    private int bossWave = 10;
    private GameObject dBEnemy;
    private GameObject chargerEnemy;
    private GameObject sEnemy;
    private GameObject backShooterEnemy;

    private float minXSpawnPoint = -10f;
    private float maxXSpawnPoint = 10f;
    private float topYSpawnPoint = 9f;
    public float MinXSpawnPoint => minXSpawnPoint;
    public float MaxXSpawnPoint => maxXSpawnPoint;
    public float TopYSpawnPoint => topYSpawnPoint;
    public bool IsRegularWave => wave < bossWave;
    public bool IsBossWave => wave == bossWave;
    public NewWaveDisplay NewWaveDisplay => newWaveDisplay;

    private void Update() {
        //shortcut to skip to wave 9 with one enemy to kill and then wave 10 begins
        if (Input.GetKeyDown(KeyCode.Alpha9)) {
            wave = 9;
            maxDBEnemies = 1;
            dBEnemies.Clear();
            dBEnemies.Add(dBEnemy);
        }

        //shortcut to wave 4. will effectively bring you to wave 5. must do this once asteroid
        //is shot
        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            wave = 4;
            dBEnemies.Clear();
            chargerEnemies.Clear();
            sEnemies.Clear();
            backShooterEnemies.Clear();
            StartCoroutine(WaitForAllDoubleBeamerDefeated());
        }
    }

    public IEnumerator SpawnDoubleBeamerCoroutine() {
        WaitForSeconds wait3Sec = new WaitForSeconds(3);
        WaitForSeconds wait5Sec = new WaitForSeconds(5);

        while (IsRegularWave) {
            StartCoroutine(NewWaveDisplay.ShowWaveText());
            yield return wait3Sec;

            for (int i = 0; i < maxDBEnemies; i++) {
                Vector3 pos = new Vector3(Random.Range(minXSpawnPoint, maxXSpawnPoint), topYSpawnPoint, 0);
                dBEnemy = Instantiate(doubleBeamerPrefab, pos, Quaternion.identity);
                dBEnemies.Add(dBEnemy);

                dBEnemy.transform.SetParent(enemyContainer.transform);
                yield return wait5Sec;
            }

            yield return WaitForAllDoubleBeamerDefeated();
            yield return wait3Sec;

            yield return WaitToStartNewWaveCoroutine();
        }
        yield return BossWaveCoroutine();
    }

    public IEnumerator SpawnChargerCoroutine() {
        WaitForSeconds wait3Sec = new WaitForSeconds(3);
        WaitForSeconds wait4Sec = new WaitForSeconds(4);
        WaitForSeconds wait5Sec = new WaitForSeconds(5);

        while (IsRegularWave && wave >= 2) {
            yield return wait4Sec;

            for (int i = 0; i < maxChargerEnemies; i++) {
                Vector3 pos = new Vector3(Random.Range(minXSpawnPoint, maxXSpawnPoint), topYSpawnPoint, 0);
                chargerEnemy = Instantiate(chargerPrefab, pos, Quaternion.identity);
                chargerEnemies.Add(chargerEnemy);

                chargerEnemy.transform.SetParent(enemyContainer.transform);
                yield return wait5Sec;
            }

            yield return WaitForAllChargerDefeated();
            yield return wait3Sec;

            yield break;
        }
    }

    public IEnumerator SpawnSpinnerCoroutine() {
        WaitForSeconds wait3Sec = new WaitForSeconds(3);
        WaitForSeconds wait4Sec = new WaitForSeconds(4);
        WaitForSeconds wait5Sec = new WaitForSeconds(5);

        while (IsRegularWave && wave >= 3) {
            yield return wait4Sec;

            for (int i = 0; i < maxSpinnerEnemies; i++) {
                Vector3 pos = new Vector3(Random.Range(minXSpawnPoint, maxXSpawnPoint), topYSpawnPoint, 0);
                sEnemy = Instantiate(spinnerPrefab, pos, Quaternion.identity);
                sEnemies.Add(sEnemy);

                sEnemy.transform.SetParent(enemyContainer.transform);
                yield return wait5Sec;
            }

            yield return WaitForAllSpinnerDefeated();
            yield return wait3Sec;

            yield break;
        }
    }

    public IEnumerator SpawnBackShooterCoroutine() {
        WaitForSeconds wait3Sec = new WaitForSeconds(3);
        WaitForSeconds wait5Sec = new WaitForSeconds(4);

        //back shooter enemies start showing up at wave 4
        while (IsRegularWave && wave >= 4) {
            yield return wait3Sec;

            for (int i = 0; i < maxBackShooterEnemies; i++) {
                Vector3 pos = new Vector3(Random.Range(minXSpawnPoint, maxXSpawnPoint), topYSpawnPoint, 0);
                backShooterEnemy = Instantiate(backShooterPrefab, pos, Quaternion.identity);
                backShooterEnemies.Add(backShooterEnemy);

                backShooterEnemy.transform.SetParent(enemyContainer.transform);
                yield return wait5Sec;
            }

            yield return WaitForAllBackShooterDefeated();
            yield return wait3Sec;

            yield break;
        }
    }

    private IEnumerator WaitToStartNewWaveCoroutine() {
        while (dBEnemies.Count > 0 || sEnemies.Count > 0) {
            yield return null;
        }
        StartNewWave();
    }

    private void StartNewWave() {
        if (IsRegularWave && wave == 1) {
            wave++;
            maxDBEnemies++;
            StartCoroutine(SpawnChargerCoroutine());
        } else if (IsRegularWave && wave == 2) {
            wave++;
            maxDBEnemies++;
            maxChargerEnemies++;
            StartCoroutine(SpawnSpinnerCoroutine());
            StartCoroutine(SpawnChargerCoroutine());
        } else if (IsRegularWave && wave == 3) {
            wave++;
            maxDBEnemies++;
            maxChargerEnemies++;
            maxSpinnerEnemies++;
            StartCoroutine(SpawnSpinnerCoroutine());
            StartCoroutine(SpawnChargerCoroutine());
            StartCoroutine(SpawnBackShooterCoroutine());
        } else if (IsRegularWave && wave >= 4) {
            wave++;
            maxDBEnemies++;
            maxChargerEnemies++;
            maxSpinnerEnemies++;
            maxBackShooterEnemies++;
            StartCoroutine(SpawnSpinnerCoroutine());
            StartCoroutine(SpawnChargerCoroutine());
            StartCoroutine(SpawnBackShooterCoroutine());
        } else if (isPlayerDefeated)
            return;
    }

    private IEnumerator WaitForAllDoubleBeamerDefeated() {
        while (dBEnemies.Count > 0) {
            //NOTE: Here, we're removing enemies from the list as they are defeated.
            for (int i = dBEnemies.Count - 1; i >= 0; i--) {
                if (dBEnemies[i] == null)
                    dBEnemies.RemoveAt(i);
            }
            yield return null;
        }
    }

    private IEnumerator WaitForAllChargerDefeated() {
        while (chargerEnemies.Count > 0) {
            for (int i = chargerEnemies.Count - 1; i >= 0; i--) {
                if (chargerEnemies[i] == null)
                    chargerEnemies.RemoveAt(i);
            }
            yield return null;
        }
    }

    private IEnumerator WaitForAllSpinnerDefeated() {
        while (sEnemies.Count > 0) {
            for (int i = sEnemies.Count - 1; i >= 0; i--) {
                if (sEnemies[i] == null)
                    sEnemies.RemoveAt(i);
            }
            yield return null;
        }
    }

    private IEnumerator WaitForAllBackShooterDefeated() {
        while (backShooterEnemies.Count > 0) {
            for (int i = backShooterEnemies.Count - 1; i >= 0; i--) {
                if (backShooterEnemies[i] == null)
                    backShooterEnemies.RemoveAt(i);
            }
            yield return null;
        }
    }

    private IEnumerator BossWaveCoroutine() {
        if (IsBossWave)
            StartCoroutine(NewWaveDisplay.ShowWaveText());

        //TODO:
        yield break;
    }
}
