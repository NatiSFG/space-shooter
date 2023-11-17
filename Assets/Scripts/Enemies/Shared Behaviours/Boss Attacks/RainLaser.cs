using UnityEngine;

public class RainLaser : MonoBehaviour {
    [SerializeField] private float speed = 3f;

    private LevelBounds level;

    private void Start() {
        level = Object.FindObjectOfType<LevelBounds>();
    }

    private void Update() {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
        if (LaserOutOfBounds())
            Destroy(gameObject);
    }

    private bool LaserOutOfBounds() {
        float x = transform.position.x;
        float y = transform.position.y;
        if (x < level.LeftBound || x > level.RightBound ||
            y < level.BottomBound || y > level.TopBound)
            return true;
        else return false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<HealthEntity>()) {
            HealthEntity player = other.GetComponent<HealthEntity>();
            if (player != null) {
                player.TryDamage();
                Destroy(gameObject);
            }
        }
    }
}
