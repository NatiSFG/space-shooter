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

    private CameraShake cameraShake;
    private BoxCollider2D hbCol;
    private SpriteRenderer hbRenderer;
    private AudioSource hbAudio;

    private BossHealthBar healthBar;
    private Animator anim;
    private new AudioSource audio;
    private BoxCollider2D[] cols;
    private UIManager ui;
    private bool bossDefeatedCoroutineStarted = false;

    public BossHealthBar HealthBar => healthBar;

    private void Awake() {
        healthBar = Component.Instantiate(healthBarPrefab);
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        hbCol = hyperbeam.GetComponent<BoxCollider2D>();
        hbRenderer = hyperbeam.GetComponent<SpriteRenderer>();
        hbAudio = hyperbeam.GetComponent<AudioSource>();
    }

    private void Start() {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(currentHealth);
        cols = GetComponents<BoxCollider2D>();
        ui = Object.FindObjectOfType<UIManager>();
        cameraShake = Object.FindObjectOfType<CameraShake>();

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
        StartCoroutine(TeleportPlayerHomingLaser());
    }

    private IEnumerator Hyperbeam() {
        if (hbCol != null)
            hbCol.enabled = true;
        if (hbRenderer != null)
            hbRenderer.enabled = true;
        cameraShake.HyperbeamShake();
        hbAudio.Play();
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
        while (true) {
            WaitForSeconds wait = new WaitForSeconds(2);
            Vector3 pos = new Vector3(0, -1.25f, 0);
            GameObject laser = Instantiate(homingLaserPrefab, pos, Quaternion.identity);
            Debug.Log(laser.name);
            laser.transform.parent = this.gameObject.transform;
            laser.transform.localPosition = pos;
            yield return wait;
        }
    }

    private void Death() {
        if (anim != null)
            anim.SetTrigger("OnEnemyDeath");
        if (audio != null)
            audio.Play();
        foreach (BoxCollider2D c in cols)
            c.enabled = false;
        ui.StartCoroutine(ui.GameWonSequence());
        Destroy(this.gameObject, 3);
    }
}
