using UnityEngine;

public class Ripple : MonoBehaviour {

    [SerializeField] private float speed = 2f;

    private LevelBounds level;

    private void Start() {
        level = Object.FindObjectOfType<LevelBounds>();
    }

    private void Update() {
        Movement();
    }

    private void Movement() {
        transform.Translate(Vector3.down * speed * Time.deltaTime, Space.Self);
        if (transform.position.y <= level.BottomBound)
            Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.GetComponent<HealthEntity>()) {
            HealthEntity player = other.GetComponentInParent<HealthEntity>();
            if (player != null)
                player.TryDamage();
        }
    }
}
