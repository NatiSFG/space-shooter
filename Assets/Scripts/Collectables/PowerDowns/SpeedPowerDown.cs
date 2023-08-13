using UnityEngine;

public class SpeedPowerDown : PowerDown {
    protected override void ApplyPowerDown(GameObject player) {
        if (player.TryGetComponent(out ShipMovementController2D moveController)) {
            moveController.SpeedPowerDownActive();
        }
    }
}
