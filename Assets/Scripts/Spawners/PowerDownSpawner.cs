using System.Collections;
using UnityEngine;

public class PowerDownSpawner : WaveSystem {
    [SerializeField]
    private SpawnInfo[] powerDowns = {
        new SpawnInfo {
            minSpawnTime = 13,
            maxSpawnTime = 20
        }
    };

    [SerializeField] private GameObject collectableContainer;

    private GameObject powerDown;

    public IEnumerator SpawnCoroutine() {
        yield return new WaitForSeconds(3);
        WaitForSeconds waitRandom = new WaitForSeconds(Random.Range(powerDowns[0].minSpawnTime,
            powerDowns[0].maxSpawnTime + 1));
        while (!IsPlayerDefeated) {
            yield return waitRandom;
            Vector3 pos = new Vector3(Random.Range(MinXSpawnPoint, MaxXSpawnPoint), TopYSpawnPoint, 0);
            powerDown = Instantiate(powerDowns[0].prefab, pos, Quaternion.identity);
            powerDown.transform.SetParent(collectableContainer.transform);
        }
    }
}
