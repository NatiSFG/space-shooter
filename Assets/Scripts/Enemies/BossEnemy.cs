using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class BossEnemy : MonoBehaviour {
    [FormerlySerializedAs("healthBar")]
    [SerializeField] private BossHealthBar healthBarPrefab;
    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth = 20;

    [Header("Attacks")]
    [SerializeField] private GameObject hyperbeam;
    [SerializeField] private GameObject ripplePrefab;
    [SerializeField] private GameObject homingLaserPrefab;
    [SerializeField] private GameObject rainLaserPrefab;

    [Header("Laser Rain Audio Clips")]
    [SerializeField] private AudioClip laserRainClip;
    [SerializeField] private AudioClip explosionClip;

    private GameObject laserContainer;
    private CameraShake cameraShake;
    private BoxCollider2D hbCol;
    private SpriteRenderer hbRenderer;
    private AudioSource aud;

    private BossHealthBar healthBar;
    private Animator anim;
    private BoxCollider2D[] cols;
    private UIManager ui;
    private bool bossDefeatedCoroutineStarted = false;
    private WaveSystem wave;

    public BossHealthBar HealthBar => healthBar;

    private void Awake() {
        healthBar = Component.Instantiate(healthBarPrefab);
        anim = GetComponent<Animator>();
        hbCol = hyperbeam.GetComponent<BoxCollider2D>();
        hbRenderer = hyperbeam.GetComponent<SpriteRenderer>();
        aud = GetComponent<AudioSource>();
    }

    private void Start() {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(currentHealth);
        cols = GetComponents<BoxCollider2D>();
        ui = Object.FindObjectOfType<UIManager>();
        cameraShake = Object.FindObjectOfType<CameraShake>();
        wave = Object.FindObjectOfType<WaveSystem>();
        laserContainer = GameObject.FindGameObjectWithTag("Laser Container");
    }

    private void Update() {
        if (currentHealth <= 0 && !bossDefeatedCoroutineStarted) {
            Death();
            bossDefeatedCoroutineStarted = true;
        }
    }

    private void OnDestroy() {
        //use this when boss dies. don't destroy the health bar if the health bar is already null
        if (healthBar != null)
            GameObject.Destroy(healthBar.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            HealthEntity player = other.GetComponent<HealthEntity>();
            if (player != null) {
                player.TryDamage();
            }
        }
        
        else if (other.tag == "Player Laser" && other.GetComponent<Laser>() != null) {
            Destroy(other.gameObject);
            TakeDamage(1);
        }
    }

    private void TakeDamage(int damage) {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }

    public void StartBossAttacks() {
        StartCoroutine(LaserRain());
    }

    private IEnumerator Hyperbeam() {
        if (hbCol != null)
            hbCol.enabled = true;
        if (hbRenderer != null)
            hbRenderer.enabled = true;
        cameraShake.HyperbeamShake();
        aud = hyperbeam.GetComponent<AudioSource>();
        aud.Play();
        yield return new WaitForSeconds(5);
        hbCol.enabled = false;
        hbRenderer.enabled = false;
    }

    private IEnumerator Ripple() {
        WaitForSeconds wait = new WaitForSeconds(2);
        Vector3 pos = new Vector3(0, -1.25f, 0);
        for (int i = 0; i < 3; i++) {
            GameObject ripple = Instantiate(ripplePrefab, pos, Quaternion.identity);
            ripple.transform.parent = this.gameObject.transform;
            ripple.transform.localPosition = pos;
            yield return wait;
        }
    }

    private IEnumerator TeleportPlayerHomingLaser() {
        WaitForSeconds wait = new WaitForSeconds(2);
        Vector3 pos = new Vector3(0, -1.25f, 0);
        GameObject laser = Instantiate(homingLaserPrefab, pos, Quaternion.identity);
        laser.transform.parent = this.gameObject.transform;
        laser.transform.localPosition = pos;
        yield return wait;
    }

    private IEnumerator LaserRain() {
        WaitForSeconds wait = new WaitForSeconds(0.3f);
        float duration = Time.time + 10f;
        aud = GetComponent<AudioSource>();
        aud.clip = laserRainClip;
        aud.Play();
        while (Time.time < duration) {
            Vector2 pos = new Vector2(Random.Range(wave.MinXSpawnPoint, wave.MaxXSpawnPoint), wave.TopYSpawnPoint);
            GameObject laser = Instantiate(rainLaserPrefab, pos, Quaternion.identity);
            laser.transform.parent = laserContainer.transform;
            yield return wait;
        }
    }

    private void Death() {
        if (anim != null)
            anim.SetTrigger("OnEnemyDeath");
        if (aud != null) {
            aud = GetComponent<AudioSource>();
            aud.clip = explosionClip;
            aud.Play();
        }
        foreach (BoxCollider2D c in cols)
            c.enabled = false;
        ui.StartCoroutine(ui.GameWonSequence());
        Destroy(this.gameObject, 3);
    }
}
