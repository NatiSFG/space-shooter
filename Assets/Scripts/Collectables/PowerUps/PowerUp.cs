using UnityEngine;

public class PowerUp : Collectable {

    protected override void OnPickUp(GameObject player) {
        ApplyPowerUp(player);
    }

    protected virtual void ApplyPowerUp(GameObject player) { }
}
