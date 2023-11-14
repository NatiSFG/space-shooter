using UnityEngine;

public class Hyperbeam : MonoBehaviour {

    public void OnTriggerStay2D(Collider2D other) {
        if (other.GetComponent<HealthEntity>()) {
            Debug.Log("hyperbeam collided with the player");
            HealthEntity player = other.GetComponentInParent<HealthEntity>();
            if (player != null)
                player.TryDamage();
        }
    }
}
