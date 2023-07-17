using UnityEngine;
using UnityEngine.UI;

public class SpeedBoostBar : MonoBehaviour {
    [SerializeField] private ShipMovementController2D target;
    [SerializeField] private Slider slider;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fill;

    //TODO: This UI bar should increase towards 100% as the target uses its entire speed boost duration.
    //      This UI bar should decrease towards 0% as the target's speed boost cooldown approaches 0.
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
            //The numerator is the elapsed time that passed since the end of the Speed Boost duration.
            //Then that's divided by the remainder of the cooldown after the duration
            //(which is 20 - 3 = 17). Whatever the denominator evaluates to is how long this math effect will take place for.
            //The denominator will tell you how long it will take to get there.

            //startTime is the time Speed Boost just finished
            float startTime = target.TimeUsedSpeedBoost + target.SpeedBoostDuration;
            float timePercentage = (Time.time - startTime) / (target.SpeedBoostCooldown - target.SpeedBoostDuration);
            slider.value = 1 - timePercentage;
            UpdateGradient();
        } else {
            slider.value = 0;
            UpdateGradient();
            return;
        }
    }

    private void UpdateGradient() => fill.color = gradient.Evaluate(slider.value);
}