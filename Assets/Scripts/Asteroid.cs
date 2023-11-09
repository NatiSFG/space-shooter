using System.Collections;
using UnityEngine;

public class Asteroid : MonoBehaviour {

    [SerializeField] private float speed = 25f;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private EnemyWaveSpawner waveSpawner;
    [SerializeField] private NewWaveDisplay waveDisplay;
    
    private WaveSystem waveSystem;

    public NewWaveDisplay NewWaveDisplay => waveDisplay;

    private void Start() {
        waveSystem = Object.FindObjectOfType<WaveSystem>();
    }

    private void Update() {
        transform.Rotate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player Laser") {
            Destroy(other.gameObject);
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            waveDisplay.ShowWaveText();
            waveSystem.StartSpawning();
            Destroy(this.gameObject, .25f);
        }
    }
}
