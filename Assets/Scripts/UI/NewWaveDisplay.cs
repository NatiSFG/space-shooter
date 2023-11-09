using System.Collections;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class NewWaveDisplay : MonoBehaviour {
    [SerializeField] private WaveSystem waveSystem;

    private TMP_Text text;

    private void Start() {
        text = GetComponent<TMP_Text>();
    }

    public void ShowWaveText() {
        StartCoroutine(ShowWaveTextCoroutine());
    }

    private IEnumerator ShowWaveTextCoroutine() {
        text.text = "Wave " + waveSystem.Wave;
        text.enabled = true;
        Debug.Log("showing text");
        yield return new WaitForSeconds(2);
        Debug.Log("disabling text");
        text.enabled = false;
    }
}
