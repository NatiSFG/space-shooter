using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveSpawner : WaveSystem {
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject enemyContainer;
    [SerializeField] private List<GameObject> enemies = new List<GameObject>();

    private int maxEnemies = 3; //final max regular enemies is 11 on wave 9
    private int bossWave = 10;
    private GameObject enemy;

    public bool IsRegularWave => wave < bossWave;
    public bool IsBossWave => wave == bossWave;

    private void Update() {
        //arrive at wave 9 with one enemy to kill and then wave 10 begins
        if (Input.GetKeyDown(KeyCode.B)) {
            wave = 9;
            maxEnemies = 1;
            enemies.Clear();
            enemies.Add(enemy);
            StartCoroutine(WaitForAllEnemiesDefeated());
        }
    }

    public IEnumerator SpawnEnemyCoroutine() {
        WaitForSeconds wait3Sec = new WaitForSeconds(3);
        WaitForSeconds wait5Sec = new WaitForSeconds(5);

        while (IsRegularWave) {
            StartCoroutine(NewWaveDisplay.ShowWaveText());
            yield return wait3Sec;

            for (int i = 0; i < maxEnemies; i++) {
                Vector3 pos = new Vector3(Random.Range(-8f, 8f), 8.5f, 0);
                enemy = Instantiate(enemyPrefab, pos, Quaternion.identity);
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
        StartCoroutine(NewWaveDisplay.ShowWaveText());

        //TODO:
        yield break;
    }
}
