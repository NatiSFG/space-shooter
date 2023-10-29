using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//TODO: Maybe append "Collectable" at the end of all the Collectable power up class names?
//(and then call this WavePowerUp)
public class WaveAttack : MonoBehaviour {
    [SerializeField] private HealthEntity health;
    [SerializeField] private GameObject wave;
    [SerializeField] private Image waveImage;

    private bool isWavePowerUpActive;

    public bool IsWavePowerUpActive {
        get { return isWavePowerUpActive; }
        set { isWavePowerUpActive = value; }
    }

    public void WavePowerUpActive() {
        waveImage.enabled = true;
        StartCoroutine(WavePowerDownCoroutine());
        health.StartInvincibility(5);
        wave.SetActive(true);
    }

    private IEnumerator WavePowerDownCoroutine() {
        IsWavePowerUpActive = true;
        yield return new WaitForSeconds(5);
        waveImage.enabled = false;
        wave.SetActive(false);
        IsWavePowerUpActive = false;
    }
}
