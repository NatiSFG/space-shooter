using UnityEngine;
using TMPro;
using System.Collections;

public class NewWaveUI : MonoBehaviour {
    [SerializeField] private SpawnManager spawnManager;
    private TMP_Text text;

    private int runOnce = 1;

    private void Start() {
        text = GetComponent<TMP_Text>();
    }

    public IEnumerator WaveUICoroutine() {
        if (runOnce < 2) {
            WaitForSeconds wait = new WaitForSeconds(2);
            text.text = "Wave " + spawnManager.wave;
            text.enabled = true;
            yield return wait;
            text.enabled = false;
            runOnce++;
        }
    }
}
