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

    public IEnumerator SpawnPowerDownCoroutine() {
        WaitForSeconds wait = new WaitForSeconds(3);
        yield return wait;
        while (!isPlayerDefeated) {
            WaitForSeconds waitRandom = new WaitForSeconds(Random.Range(powerDowns[0].minSpawnTime,
                powerDowns[0].maxSpawnTime + 1));
            yield return waitRandom;
            Vector3 pos = new Vector3(Random.Range(-8f, 8f), 8.5f, 0);
            Instantiate(powerDowns[0].prefab, pos, Quaternion.identity);
        }
    }
}
