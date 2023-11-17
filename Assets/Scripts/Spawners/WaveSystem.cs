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
    private PowerDownSpawner powerDownSpawner;
    private ProvisionSpawner provisionSpawner;
    private GameManager gameManager;

    private int bossWave = 10;
    private float minXSpawnPoint = -8f;
    private float maxXSpawnPoint = 8f;
    private float topYSpawnPoint = 8f;
    public float MinXSpawnPoint => minXSpawnPoint;
    public float MaxXSpawnPoint => maxXSpawnPoint;
    public float TopYSpawnPoint => topYSpawnPoint;
    public bool IsRegularWave => wave < bossWave;
    public bool IsBossWave => wave == bossWave;

    public bool IsPlayerDefeated => isPlayerDefeated;
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
        powerDownSpawner = GetComponent<PowerDownSpawner>();
        provisionSpawner = GetComponent<ProvisionSpawner>();
        gameManager = Object.FindObjectOfType<GameManager>();
    }

    private void OnDestroy() {
        if (playerHealth != null)
            playerHealth.onDamaged -= CheckForDeath;
    }

    public void StartSpawning() {
        StartCoroutine(enemyWaveSpawner.SpawnCoroutine());
        StartCoroutine(powerUpSpawner.SpawnCoroutine());
        StartCoroutine(powerDownSpawner.SpawnCoroutine());
        StartCoroutine(provisionSpawner.SpawnCoroutine());

        if (gameManager.IsGameOver)
            StopAllCoroutines();
    }

    private void CheckForDeath() {
        if (playerHealth.Health <= 0)
            OnPlayerDeath();
    }

    public void OnPlayerDeath() {
        isPlayerDefeated = true;
    }

    public void OnPlayerRestart() {
        isPlayerDefeated = false;
    }
}
