﻿using UnityEngine;

public class HomingShotPowerUp : PowerUp {
    protected override void ApplyPowerUp(GameObject player) {
        if (player.TryGetComponent(out ShootController controller))
            controller.HomingShotPowerUpActive();
    }
}
