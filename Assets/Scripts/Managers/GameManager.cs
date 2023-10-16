using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    private bool isGameOver;
    private int score = 0;

    private WaveSystem waveSystem;

    public bool IsGameOver => isGameOver;

    public event Action onScoreChanged;

    public int Score {
        get { return score; }
        set {
            score = Mathf.Max(0, value);
            if (onScoreChanged != null)
                onScoreChanged();
        }
    }

    private void Start() {
        Enemy.onAnyDefeated += OnAnyEnemyDefeated;
        waveSystem = FindObjectOfType<WaveSystem>();
    }

    private void OnDestroy() {
        Enemy.onAnyDefeated -= OnAnyEnemyDefeated;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R) && isGameOver) {
            SceneManager.LoadScene(1); //game scene
            waveSystem.OnPlayerRestart();
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
#if UNITY_EDITOR //preprocessor macro
            //exits play mode
            UnityEditor.EditorApplication.isPlaying = false;
#else
            //when the project is compiled for a build, we'll run the below line
            //if you ever need to test quiting the application below, comment out
            //the macro and hashes
            Application.Quit();
#endif
        }
    }

    private void OnAnyEnemyDefeated() {
        Score += 100;
    }

    public void GameOver() {
        isGameOver = true;
    }
}
