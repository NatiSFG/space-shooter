using UnityEngine;

public class WaveSystem : MonoBehaviour {
    [System.Serializable]
    protected struct SpawnInfo {
        public GameObject prefab;

        public float minSpawnTime;
        public float maxSpawnTime;
    }
    [SerializeField] protected static int wave = 1;

    protected static bool isPlayerDefeated = false;
    private HealthEntity playerHealth;
    private EnemyWaveSpawner enemyWaveSpawner;
    private PowerUpSpawner powerUpSpawner;
    private ProvisionSpawner provisionSpawner;

    public int Wave => wave;

    private void Start() {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null) {
            playerHealth = player.GetComponent<HealthEntity>();
            if (playerHealth != null)
                playerHealth.onDamaged += CheckForDeath;
        }
        enemyWaveSpawner = GetComponent<EnemyWaveSpawner>();
        powerUpSpawner = GetComponent<PowerUpSpawner>();
        provisionSpawner = GetComponent<ProvisionSpawner>();
    }

    private void OnDestroy() {
        if (playerHealth != null)
            playerHealth.onDamaged -= CheckForDeath;
    }

    public void StartSpawning() {
        StartCoroutine(enemyWaveSpawner.SpawnEnemyCoroutine());
        StartCoroutine(powerUpSpawner.SpawnPowerUpCoroutine());
        StartCoroutine(provisionSpawner.SpawnProvisionCoroutine());
    }

    private void CheckForDeath() {
        if (playerHealth.Health <= 0)
            OnPlayerDeath();
    }

    public void OnPlayerDeath() {
        isPlayerDefeated = true;
    }
}
