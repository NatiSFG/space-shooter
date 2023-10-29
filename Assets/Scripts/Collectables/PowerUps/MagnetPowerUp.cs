using UnityEngine;

public class MagnetPowerUp : PowerUp {
    protected override void ApplyPowerUp(GameObject player) {
        if (player.TryGetComponent(out MagnetPull magnetPull))
            magnetPull.MagnetPowerUpActive();
    }
}