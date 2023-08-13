using UnityEngine;

public class PowerDown : Collectable {

    protected override void OnPickUp(GameObject player) {
        ApplyPowerDown(player);
    }

    //TODO: Make abstract later
    protected virtual void ApplyPowerDown(GameObject player) { }
}