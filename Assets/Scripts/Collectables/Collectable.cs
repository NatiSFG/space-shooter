using UnityEngine;

public class Collectable : MonoBehaviour {

    [SerializeField] private float speed = 3;
    [SerializeField] private AudioClip clip;

    protected virtual void Update() {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
        if (transform.position.y <= -6f)
            Destroy(gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other) {
        //if the player collides with us and this is not a Wave PowerUp
        if (other.tag == "Player" && gameObject.GetComponent<WavePowerUp>() == null) {
            GameObject player = other.gameObject;

            AudioSource.PlayClipAtPoint(clip, transform.position);
            OnPickUp(player);
            Destroy(gameObject);
        } else if (other.tag == "Player" && gameObject.GetComponents<WavePowerUp>() != null) {
            GameObject player = other.gameObject;
            WaveAttack wave = player.GetComponentInChildren<WaveAttack>();
            GameObject waveObj = wave.gameObject;

            AudioSource.PlayClipAtPoint(clip, transform.position);
            OnPickUp(waveObj);
            Destroy(gameObject);
        }
    }

    protected virtual void OnPickUp(GameObject player) { }
}
