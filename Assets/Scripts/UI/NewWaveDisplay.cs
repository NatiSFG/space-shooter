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

    public IEnumerator ShowWaveText() {
        text.text = "Wave " + waveSystem.Wave;
        text.enabled = true;
        yield return new WaitForSeconds(2);
        Debug.Log("disabling text");
        text.enabled = false;
    }
}
