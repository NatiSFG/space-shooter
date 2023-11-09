using UnityEngine;

public class WavePowerUp : PowerUp {
    protected override void ApplyPowerUp(GameObject player) {
        player.GetComponent<ShootController>().ActivateWaveObject();
    }
}
