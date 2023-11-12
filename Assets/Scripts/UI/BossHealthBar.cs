using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour {
    [SerializeField] private Slider slider;


    private void Start() {
        SetMaxHealth(20);
    }

    public void DisplayHealthBar() {
        if (!this.gameObject.activeSelf)
            this.gameObject.SetActive(true);
    }

    public void HideHealthBar() {
        if (this.gameObject.activeSelf)
            this.gameObject.SetActive(false);
    }

    private void SetMaxHealth(int health) {
        slider.value = health;
    }

    public void SubtractHealth(int damage) {
        slider.value -= damage;
    }
}
