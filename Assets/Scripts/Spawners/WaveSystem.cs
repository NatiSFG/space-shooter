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
        StartCoroutine(enemyWaveSpawner.SpawnDoubleBeamerCoroutine());
        StartCoroutine(enemyWaveSpawner.SpawnDodgerCoroutine());
        StartCoroutine(powerUpSpawner.SpawnPowerUpCoroutine());
        StartCoroutine(powerDownSpawner.SpawnPowerDownCoroutine());
        StartCoroutine(provisionSpawner.SpawnProvisionCoroutine());

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
