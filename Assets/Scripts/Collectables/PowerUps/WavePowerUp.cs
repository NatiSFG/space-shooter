using UnityEngine;

public class WavePowerUp : PowerUp {
    protected override void ApplyPowerUp(GameObject player) {
        if (player.GetComponentInChildren<WaveAttack>() != null) {
            WaveAttack attack;
            attack = player.GetComponentInChildren<WaveAttack>();
            attack.WavePowerUpActive();

        }
    }
}
