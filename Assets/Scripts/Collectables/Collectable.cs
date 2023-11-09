using UnityEngine;

public class Collectable : MonoBehaviour {

    [SerializeField] private float speed = 3;
    [SerializeField] private AudioClip clip;
    [SerializeField] private WavePowerUp wavePowerUp;

    protected virtual void Update() {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
        if (transform.position.y <= -6f)
            Destroy(gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other) {
        //if the player collides with us and this is not a Wave PowerUp
        if (other.tag == "Player" && wavePowerUp == null) {
            GameObject player = other.gameObject;

            AudioSource.PlayClipAtPoint(clip, transform.position);
            OnPickUp(player);
            Destroy(gameObject);
        } else if (other.tag == "Player" && wavePowerUp != null) {
            ShootController player = other.GetComponent<ShootController>();

            AudioSource.PlayClipAtPoint(clip, transform.position);
            OnPickUp(player.gameObject);
            Destroy(gameObject);
        }
    }

    protected virtual void OnPickUp(GameObject player) { }
}
