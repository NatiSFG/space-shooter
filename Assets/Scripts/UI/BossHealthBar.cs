using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour {
    [SerializeField] private Slider slider;

    public void DisplayHealthBar() {
        if (!this.gameObject.activeSelf)
            this.gameObject.SetActive(true);
    }

    public void HideHealthBar() {
        if (this.gameObject.activeSelf)
            this.gameObject.SetActive(false);
    }

    public void SetMaxHealth(int health) {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(int health) {
        slider.value = health;
    }
}
