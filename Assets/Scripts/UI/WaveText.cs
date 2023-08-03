using System.Collections;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class WaveText : MonoBehaviour {
    [SerializeField] private SpawnManager spawnManager;

    private TMP_Text text;

    private void Start() {
        text = GetComponent<TMP_Text>();
    }

    public IEnumerator ShowWaveText() {
        text.text = "Wave " + spawnManager.Wave;
        text.enabled = true;
        yield return new WaitForSeconds(2);
        text.enabled = false;
    }
}
