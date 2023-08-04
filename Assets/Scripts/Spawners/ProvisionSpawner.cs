using System.Collections;
using UnityEngine;

public class ProvisionSpawner : WaveSystem {
    [SerializeField]
    private SpawnInfo[] provisions = {
        new SpawnInfo {
            minSpawnTime = 5,
            maxSpawnTime = 14
        }
    };

    public IEnumerator SpawnProvisionCoroutine() {
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
}
