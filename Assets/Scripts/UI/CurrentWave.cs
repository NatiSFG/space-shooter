using TMPro;
using UnityEngine;

public class CurrentWave : MonoBehaviour {
    [SerializeField] private WaveSystem waveSystem;
    [SerializeField] private GameObject asteroid;

    private TMP_Text text;

    private void Start() {
        text = GetComponent<TMP_Text>();
    }

    private void Update() {
        if (asteroid == null)
            text.text = "Wave " + waveSystem.Wave;
    }
}