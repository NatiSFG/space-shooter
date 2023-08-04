using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveSpawner : WaveSystem {
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject enemyContainer;
    [SerializeField] private List<GameObject> enemies = new List<GameObject>();
    [SerializeField] private WaveText waveUI;

    private WaveSystem waveSystem;
    private int maxEnemies = 3;
    private int bossWave = 10;
    private int finalMaxEnemies = 11;

    public bool IsRegularWave => wave < bossWave;
    public bool IsBossWave => wave == bossWave;

    private void Start() {
        waveSystem = GetComponent<WaveSystem>();
    }

    public IEnumerator SpawnEnemyCoroutine() {
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

            if (WaveSystem.isPlayerDefeated)
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
}
