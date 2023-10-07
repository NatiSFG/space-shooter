using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthEntity : MonoBehaviour {
    [Header("Shield Protection")]
    [SerializeField] private SpriteRenderer shield;
    [SerializeField] private int totalShieldProtection = 1;
    [SerializeField] private int currentShieldProtection;

    [SerializeField] Enemy enemy;


    public int CurrentShieldProtection {
        get { return currentShieldProtection; }
        set { currentShieldProtection = value;
        }
    }

    private void Start() {
        currentShieldProtection = totalShieldProtection;
    }

    public DamageResult TryDamage() {
        if (TryDamageShield())
            return DamageResult.ShieldDamaged;
        else if (enemy.CheckToDefeatFromPlayer())
            return DamageResult.Success;
    }

    public bool TryDamageShield() {
        if (CurrentShieldProtection == totalShieldProtection) {
            TakeShieldDamage();
            return true;
        } else return false;
    }

    private void TakeShieldDamage() {
        CurrentShieldProtection--;
        shield.gameObject.SetActive(false);
    }
}
