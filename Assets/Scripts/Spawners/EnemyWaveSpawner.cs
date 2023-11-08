﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveSpawner : WaveSystem {
    [SerializeField] private GameObject enemyContainer;
    [SerializeField] protected NewWaveDisplay newWaveDisplay;

    [Header("Enemies")]
    [SerializeField] private GameObject doubleBeamerPrefab;
    [SerializeField] private GameObject chargerPrefab;
    [SerializeField] private GameObject spinnerPrefab;
    [SerializeField] private GameObject backShooterPrefab;
    [SerializeField] private GameObject dodgerPrefab;


    //new variables
    private int[] enemiesAlive = new int[5] {0, 0, 0, 0, 0 };
    private int[,] maxEnemies = new int[9, 5] {
        //Double Beamer, Dodger, Charger, Spinner, Back Shooter
        { 3, 1, 0, 0, 0 },
        { 4, 1, 2, 0, 0 },
        { 5, 1, 3, 1, 0 },
        { 6, 1, 4, 2, 1 },
        { 7, 1, 5, 3, 2 },
        { 8, 1, 6, 4, 3 },
        { 9, 1, 7, 5, 4 },
        { 10, 1, 8, 6, 5 },
        { 11, 1, 9, 7, 6 }
    };

    #region variables
    private int bossWave = 10;
    private float minXSpawnPoint = -8f;
    private float maxXSpawnPoint = 8f;
    private float topYSpawnPoint = 9f;
    public float MinXSpawnPoint => minXSpawnPoint;
    public float MaxXSpawnPoint => maxXSpawnPoint;
    public float TopYSpawnPoint => topYSpawnPoint;
    public bool IsRegularWave => wave < bossWave;
    public bool IsBossWave => wave == bossWave;
    public NewWaveDisplay NewWaveDisplay => newWaveDisplay;
    #endregion

    private void Update() {
        Debug.Log("double beamers: " + enemiesAlive[0] + "\nchargers: " + enemiesAlive[2]);
    }

    //this coroutine runs once for the current wave and stops
    public IEnumerator SpawningCoroutine() {
        WaitForSeconds wait = new WaitForSeconds(3);
        GameObject enemy;
        while (enemiesAlive[0] < maxEnemies[wave - 1, 0]) {
            for (int i = 0; i <= 4; i++) { //maxEnemies.GetLength(1) uses the second length of the 2d array which how many types of enemies
                Vector2 pos = new Vector2(Random.Range(minXSpawnPoint, maxXSpawnPoint), topYSpawnPoint);
                if (enemiesAlive[i] < maxEnemies[wave - 1, i]) {
                    switch (i) {
                        case 0:
                            enemy = Instantiate(doubleBeamerPrefab, pos, Quaternion.identity);
                            enemy.transform.parent = enemyContainer.transform;
                            break;
                        case 1:
                            enemy = Instantiate(dodgerPrefab, pos, Quaternion.identity);
                            enemy.transform.parent = enemyContainer.transform;
                            break;
                        case 2:
                            enemy = Instantiate(chargerPrefab, pos, Quaternion.identity);
                            enemy.transform.parent = enemyContainer.transform;
                            break;
                        case 3:
                            enemy = Instantiate(spinnerPrefab, pos, Quaternion.identity);
                            enemy.transform.parent = enemyContainer.transform;
                            break;
                        case 4:
                            enemy = Instantiate(backShooterPrefab, pos, Quaternion.identity);
                            enemy.transform.parent = enemyContainer.transform;
                            break;
                    }
                    enemiesAlive[i]++;
                    yield return wait;
                }
            }
        }
        wave++;
        enemiesAlive = new int[5] { 0, 0, 0, 0, 0 };
        StartCoroutine(WaitToStartNewWaveCoroutine());
    }

    private IEnumerator WaitToStartNewWaveCoroutine() {
        while (enemyContainer.transform.childCount > 0) {
            yield return null;
        }
        StartCoroutine(NewWaveDisplay.ShowWaveText());
        yield return new WaitForSeconds(3);
        if (IsRegularWave)
            StartCoroutine(SpawningCoroutine());
        else if (IsBossWave)
            StartCoroutine(BossWaveCoroutine());
    }

    public IEnumerator BossWaveCoroutine() {
        
        yield break;
    }
}
