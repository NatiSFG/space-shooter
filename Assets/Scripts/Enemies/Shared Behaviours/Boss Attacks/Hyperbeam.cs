using UnityEngine;

public class Hyperbeam : MonoBehaviour {

    private void OnTriggerStay2D(Collider2D other) {
        if (other.GetComponent<HealthEntity>()) {
            HealthEntity player = other.GetComponentInParent<HealthEntity>();
            if (player != null)
                player.TryDamage();
        }
    }
}
