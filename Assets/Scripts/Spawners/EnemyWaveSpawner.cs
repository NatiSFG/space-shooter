using System.Collections;
using UnityEngine;

public class EnemyWaveSpawner : WaveSystem {
    [SerializeField] private GameObject enemyContainer;
    [SerializeField] protected NewWaveDisplay newWaveDisplay;

    [Header("Enemies")]
    [SerializeField] private GameObject doubleBeamerPrefab;
    [SerializeField] private GameObject chargerPrefab;
    [SerializeField] private GameObject spinnerPrefab;
    [SerializeField] private GameObject backShooterPrefab;
    [SerializeField] private GameObject dodgerPrefab;

    //enemiesAlive[0] is how many Double Beamers are alive, etc.
    private int[] enemiesAlive = new int[5] {0, 0, 0, 0, 0 };
    private int[,] maxEnemies = new int[9, 5] {
        //Double Beamer, Dodger, Charger, Spinner, Back Shooter
        { 3, 1, 0, 0, 0 }, //wave 1: 3 double beamers, 1 dodger
        { 4, 1, 2, 0, 0 },
        { 5, 1, 3, 1, 0 },
        { 6, 1, 4, 2, 1 },
        { 7, 1, 5, 3, 2 },
        { 8, 1, 6, 4, 3 },
        { 9, 1, 7, 5, 4 },
        { 10, 1, 8, 6, 5 },
        { 11, 1, 9, 7, 6 } //wave 9: 11 double beamers, 1 dodger, 9 chargers, 7 spinners, 6 back shooters
    };

    public NewWaveDisplay NewWaveDisplay => newWaveDisplay;

    public IEnumerator SpawnCoroutine() {
        WaitForSeconds wait = new WaitForSeconds(3);
        GameObject enemy;
        if (wave == 1)
            yield return wait;
        while (enemiesAlive[0] < maxEnemies[wave - 1, 0]) { //while there are less double beamers alive than the max double beamers allowed for the current wave
            for (int i = 0; i <= 4; i++) { //maxEnemies.GetLength(1) uses the second length of the 2d array which how many types of enemies
                Vector2 pos = new Vector2(Random.Range(MinXSpawnPoint, MaxXSpawnPoint), TopYSpawnPoint);
                if (enemiesAlive[i] < maxEnemies[wave - 1, i]) {
                    switch (i) {
                        case 0:
                            enemy = Instantiate(doubleBeamerPrefab, pos, Quaternion.identity);
                            enemy.transform.parent = enemyContainer.transform;
                            break;
                        case 1:
                            enemy = Instantiate(dodgerPrefab, pos, Quaternion.identity);
                            enemy.transform.parent = enemyContainer.transform;
                            break;
                        case 2:
                            enemy = Instantiate(chargerPrefab, pos, Quaternion.identity);
                            enemy.transform.parent = enemyContainer.transform;
                            break;
                        case 3:
                            enemy = Instantiate(spinnerPrefab, pos, Quaternion.identity);
                            enemy.transform.parent = enemyContainer.transform;
                            break;
                        case 4:
                            enemy = Instantiate(backShooterPrefab, pos, Quaternion.identity);
                            enemy.transform.parent = enemyContainer.transform;
                            break;
                    }
                    enemiesAlive[i]++;
                    yield return wait;
                }
            }
        }
        wave++;
        enemiesAlive = new int[5] { 0, 0, 0, 0, 0 };
        StartCoroutine(WaitToStartNewWaveCoroutine());
    }

    public IEnumerator WaitToStartNewWaveCoroutine() {
        WaitForSeconds wait = new WaitForSeconds(3);
        while (enemyContainer.transform.childCount > 0) {
            yield return null;
        }
        NewWaveDisplay.ShowWaveText();
        yield return wait;
        if (IsRegularWave) {
            StartCoroutine(SpawnCoroutine());
        }
        else if (IsBossWave)
            StartCoroutine(BossWaveCoroutine());
    }

    public IEnumerator BossWaveCoroutine() {
        
        yield break;
    }
}
