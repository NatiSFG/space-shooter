using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthEntity : MonoBehaviour {
    [SerializeField, Min(1)] private int health = 3;

    [Header("Shield Protection")]
    [SerializeField] private SpriteRenderer shield;
    [SerializeField] private int currentShieldProtection;
    [SerializeField] private int totalShieldProtection = 3;

    [Space(20)]
    [SerializeField] private CameraShake cameraShake;
    [SerializeField] private AudioClip damagedClip;
    [SerializeField] private Image shieldPowerUpImage;
    [SerializeField] private Image[] shieldHealthImages;

    private int maxHealth;
    private float timeInvincibleUntil = 0;

    public event Action onDamaged;
    public event Action onHealed;
    public event Action onHealthChanged; //NOTE: Useful for UI in the future

    public int Health => health;
    public int MaxHealth => maxHealth;
    public bool IsDefeated => health <= 0;
    public bool IsInvincible => Time.time <= timeInvincibleUntil;
    public bool IsShieldPowerUpActive => currentShieldProtection > 0;
    public int TotalShieldProtection => totalShieldProtection;
    public int CurrentShieldProtection {
        get { return currentShieldProtection; }
        set { currentShieldProtection = value; }
    }

    private void Awake() {
        maxHealth = health;
    }

    private void Update() {
        SubtractOneShieldHealth();
    }

    private void SubtractOneShieldHealth() {
        if (Input.GetKeyDown(KeyCode.Q))
            TakeShieldDamage();
    }

    public DamageResult TryDamage() {
        if (IsInvincible)
            return DamageResult.Unaffected;
        else if (TryDamageShield())
            return DamageResult.ShieldDamaged;
        else {
            Damage();
            return DamageResult.Success;
        }
    }

    public bool TryDamageShield() {
        if (IsShieldPowerUpActive) {
            TakeShieldDamage();
            return true;
        }
        else return false;
    }

    private void Damage() {
        if (health == maxHealth || health == 2)
            cameraShake.Shake();
        AudioSource.PlayClipAtPoint(damagedClip, transform.position);
        health--;
        OnDamaged();
        if (onDamaged != null)
            onDamaged();
        if (onHealthChanged != null)
            onHealthChanged();
    }

    //NOTE: This updates both us, AND other scripts, after we take damage.
    //This is IN RESPONSE to damage.
    private void OnDamaged() {
        if (health <= 0) {
            //NOTE: This would be problematic for our UI Manager script,
            //      who still needs to access the player's health briefly first.
            //      So let's destroy after a short delay.
            Destroy(gameObject, 0.1f);
            return;
        }
        StartInvincibility(2);
    }

    public void Heal() {
        if (health == 1 || health == 2) {
            health++;
            if (onHealed != null)
                onHealed();
            if (onHealthChanged != null)
                onHealthChanged();
        }
    }

    public void StartInvincibility(int seconds) {
        timeInvincibleUntil = Time.time + seconds;
    }

    private void TakeShieldDamage() {
        currentShieldProtection--;
        shieldHealthImages[currentShieldProtection].enabled = false;
        UpdateShieldColor();
        if (CurrentShieldProtection <= 0)
            ShieldPowerDown();
        StartInvincibility(2);
    }

    private void UpdateShieldColor() {
        Color c = shield.color;
        c.a = (float) CurrentShieldProtection / TotalShieldProtection;
        shield.color = c;
    }

    public void ShieldPowerUpActive() {
        CurrentShieldProtection = TotalShieldProtection;
        UpdateShieldColor();
        shieldPowerUpImage.enabled = true;
        for (int i = 0; i < shieldHealthImages.Length; i++) {
            shieldHealthImages[i].enabled = true;
        }
        shield.gameObject.SetActive(true);
    }

    private void ShieldPowerDown() {
        shieldPowerUpImage.enabled = false;
        shield.gameObject.SetActive(false);
    }
}
