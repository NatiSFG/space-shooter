using UnityEngine;

public class ShieldPowerUp : PowerUp {
    protected override void ApplyPowerUp(GameObject player) {
        //typically the parameter is input and we don't use a keyword then.
        //the out keyword means the method must set a value to the parameter variable
        //ie it must output a value.
        //the ref keyword means the parameter's value can be changed and it can be input or output
        if (player.TryGetComponent(out HealthEntity entity))
            entity.ShieldPowerUpActive();
    }
}
