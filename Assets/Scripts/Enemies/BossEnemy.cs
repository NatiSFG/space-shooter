using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Serialization;

public class BossEnemy : MonoBehaviour {
    [FormerlySerializedAs("healthBar")]
    [SerializeField] private BossHealthBar healthBarPrefab;
    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth = 20;

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
    }

    private void Start() {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(currentHealth);
        cols = GetComponents<BoxCollider2D>();
        ui = Object.FindObjectOfType<UIManager>();
    }

    private void Update() {
        if (currentHealth <= 0 && !bossDefeatedCoroutineStarted) {
            Death();
            bossDefeatedCoroutineStarted = true;
        }
    }

    private void OnDestroy() {
        //use this when boss dies
        //don't destroy the health bar if the health bar is already null
        if (healthBar != null)
            GameObject.Destroy(healthBar.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        //if player touches boss, player only gets hurt
        if (other.tag == "Player") {
            HealthEntity player = other.GetComponent<HealthEntity>();
            if (player != null) {
                player.TryDamage();
            }
        }
        //if player shoots boss, take one hit
        else if (other.tag == "Player Laser" && other.GetComponent<Laser>() != null) {
            Destroy(other.gameObject);
            TakeDamage(1);
        }
    }

    private void TakeDamage(int damage) {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }

    private void Death() {
        if (anim != null)
            anim.SetTrigger("OnEnemyDeath");
        if (audio != null)
            audio.Play();
        foreach (BoxCollider2D c in cols)
            c.enabled = false;
        ui.StartCoroutine(ui.GameWonSequence());
        //ui.StartCoroutine(ui.BossDefeatedDisplay());
        Destroy(this.gameObject, 3);
    }
}
