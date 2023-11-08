using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//TODO: Maybe append "Collectable" at the end of all the Collectable power up class names?
//(and then call this WavePowerUp)
public class WaveAttack : MonoBehaviour {
    [SerializeField] private HealthEntity health;
    [SerializeField] private Image waveImage;

    private SpriteRenderer sprite;
    private bool isWavePowerUpActive;

    public bool IsWavePowerUpActive {
        get { return isWavePowerUpActive; }
        set { isWavePowerUpActive = value; }
    }

    private void Start() {
        sprite = GetComponent<SpriteRenderer>();
    }

    public void WavePowerUpActive() {
        waveImage.enabled = true;
        StartCoroutine(WavePowerDownCoroutine());
        health.StartInvincibility(5);
    }

    private void OnTriggerEnter(Collider other) {
        while (IsWavePowerUpActive && other.tag == "Enemy") {
            Destroy(other.gameObject);
        }
    }

    private IEnumerator WavePowerDownCoroutine() {
        IsWavePowerUpActive = true;
        yield return new WaitForSeconds(5);
        waveImage.enabled = false;
        gameObject.SetActive(false);
        IsWavePowerUpActive = false;
    }
}
