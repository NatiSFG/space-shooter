using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//TODO: Maybe append "Collectable" at the end of all the Collectable power up class names?
//(and then call this WavePowerUp)
public class WaveAttack : MonoBehaviour {
    [SerializeField] private HealthEntity health;
    [SerializeField] private GameObject wave;
    [SerializeField] private Image wavePowerUpImage;

    private bool isWavePowerUpActive;

    public bool IsWavePowerUpActive => isWavePowerUpActive;

    public void WavePowerUpActive() {
        wavePowerUpImage.enabled = true;
        StartCoroutine(WavePowerDownCoroutine());
        health.StartInvincibility(5);
        wave.SetActive(true);
    }

    private IEnumerator WavePowerDownCoroutine() {
        isWavePowerUpActive = true;
        yield return new WaitForSeconds(5);
        wavePowerUpImage.enabled = false;
        wave.SetActive(false);
        isWavePowerUpActive = false;
    }
}
