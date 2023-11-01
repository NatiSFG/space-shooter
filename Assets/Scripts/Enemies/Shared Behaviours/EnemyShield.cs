using UnityEngine;

public class EnemyShield : MonoBehaviour {
    [Header("Shield Protection")]
    [SerializeField] private int currentShieldProtection;
    [SerializeField] private int totalShieldProtection = 1;
    [SerializeField] private SpriteRenderer shield;

    public bool IsShieldActive => shield.enabled == true;

    public int CurrentShieldProtection {
        get { return currentShieldProtection; }
        set { currentShieldProtection = value; }
    }

    private void Start() {
        currentShieldProtection = totalShieldProtection;
    }

    public bool TryDamageShield() {
        if (IsShieldActive) {
            TakeShieldDamage();
            return true;
        } else return false;
    }

    private void TakeShieldDamage() {
        CurrentShieldProtection--;
        shield.enabled = false;
    }
}

