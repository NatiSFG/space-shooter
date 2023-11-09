using System.Collections;
using UnityEngine;

public class PowerUpSpawner : WaveSystem {
    
    [SerializeField]
    private SpawnInfo[] powerUps = {
        new SpawnInfo {
            minSpawnTime = 7,
            maxSpawnTime = 15
        }
    };

    [SerializeField] private GameObject collectableContainer;

    private GameObject powerUp;

    public IEnumerator SpawnCoroutine() {
        yield return new WaitForSeconds(3);
        while (!IsPlayerDefeated) {
            int randPowerUp = Random.Range(0, powerUps.Length);
            WaitForSeconds waitRandom = new WaitForSeconds(Random.Range(powerUps[randPowerUp].minSpawnTime,
                powerUps[randPowerUp].maxSpawnTime + 1));
            yield return waitRandom;
            Vector3 pos = new Vector3(Random.Range(MinXSpawnPoint, MaxXSpawnPoint), TopYSpawnPoint, 0);
            powerUp = Instantiate(powerUps[randPowerUp].prefab, pos, Quaternion.identity);
            powerUp.transform.SetParent(collectableContainer.transform);
        }
    }
}
