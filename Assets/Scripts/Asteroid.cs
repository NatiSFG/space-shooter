using UnityEngine;

public class Asteroid : MonoBehaviour {

    [SerializeField]
    private float speed = 25f;
    [SerializeField]
    private GameObject explosionPrefab;
    private WaveSystem waveSystem;

    private void Start() {
        waveSystem = GameObject.FindGameObjectWithTag("Spawner").GetComponent<WaveSystem>();
    }

    private void Update() {
        transform.Rotate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Laser") {
            Destroy(other.gameObject);
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            waveSystem.StartSpawning();
            Destroy(this.gameObject, .25f);
        }
    }
}
