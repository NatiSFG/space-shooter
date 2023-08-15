using UnityEngine;
using UnityEngine.UI;

public class SpeedBoostBar : MonoBehaviour {
    [SerializeField] private ShipMovementController2D target;
    [SerializeField] private Slider slider;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fill;

    [SerializeField] private Animator speedBoostBarAnim;

    private void Update() {
        UpdateBar();
    }

    //TODO: Use events instead
    private void UpdateBar() {
        if (target == null)
            return;

        if (target.TimeUsedSpeedBoost < 0) {
            slider.value = 0;
            UpdateGradient();
            return;
        }

        //Time.time                                     (changing variable over time)
        //TimeUsedSpeedBoost                            (start time)
        //TimeUsedSpeedBoost + SpeedBoostDuration       (end time)
        //TimeUsedSpeedBoost + SpeedBoostCooldown       (end of cooldown)

        if (target.IsSpeedBoostActive) {
            float timePercentage = (Time.time - target.TimeUsedSpeedBoost) / target.SpeedBoostDuration;
            slider.value = timePercentage;
            UpdateGradient();
        } else if (target.IsSpeedBoostCoolingDown) {
            //cooldownStartTime is the time Speed Boost just finished being active
            float cooldownStartTime = target.TimeUsedSpeedBoost + target.SpeedBoostDuration;
            float timePercentage = (Time.time - cooldownStartTime) / (target.SpeedBoostCooldown - target.SpeedBoostDuration);
            slider.value = 1 - timePercentage;
            UpdateGradient();
        } else {
            slider.value = 0;
            UpdateGradient();
            return;
        }
    }

    public void NoSpeedBoostBarScaling() {
        speedBoostBarAnim.Play("No Speed Boost");
    }

    private void UpdateGradient() => fill.color = gradient.Evaluate(slider.value);
}