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

    public IEnumerator SpawnPowerUpCoroutine() {
        WaitForSeconds wait = new WaitForSeconds(3);
        yield return wait;
        while (!isPlayerDefeated) {
            int randPowerUp = Random.Range(0, powerUps.Length);
            WaitForSeconds waitRandom = new WaitForSeconds(Random.Range(powerUps[randPowerUp].minSpawnTime,
                powerUps[randPowerUp].maxSpawnTime + 1));
            yield return waitRandom;
            Vector3 pos = new Vector3(Random.Range(-8f, 8f), 8.5f, 0);
            powerUp = Instantiate(powerUps[randPowerUp].prefab, pos, Quaternion.identity);
            powerUp.transform.SetParent(collectableContainer.transform);
        }
    }
}
