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

    [SerializeField] private GameObject collectableContainer;

    private GameObject provision;

    public IEnumerator SpawnCoroutine() {
        yield return new WaitForSeconds(3);
        while (!IsPlayerDefeated) {
            int randProvision = Random.Range(0, provisions.Length);
            WaitForSeconds waitRandom = new WaitForSeconds(Random.Range(provisions[randProvision].minSpawnTime,
                provisions[randProvision].maxSpawnTime));
            yield return waitRandom;
            Vector3 pos = new Vector3(Random.Range(MinXSpawnPoint, MaxXSpawnPoint), TopYSpawnPoint, 0);
            provision = Instantiate(provisions[randProvision].prefab, pos, Quaternion.identity);
            provision.transform.SetParent(collectableContainer.transform);
        }
    }
}
