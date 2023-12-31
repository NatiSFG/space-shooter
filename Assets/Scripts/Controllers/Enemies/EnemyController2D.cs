﻿using System.Collections;
using UnityEngine;

public class EnemyController2D : MonoBehaviour {
    
    [SerializeField] protected float standardSpeed = 3f;

    public float Speed {
        get { return standardSpeed; }
        set { standardSpeed = value; }
    }

    protected WaveSystem waveSystem;
    private float topOfLevel = 9;
    private float bottomOfLevel = -7;
    public float TopOfLevel => topOfLevel;
    public float BottomOfLevel => bottomOfLevel;

    private void OnValidate() {
        topOfLevel = Mathf.Max(0, topOfLevel);
        //bottomOfLevel will always be 0 or negative, the minimum between itself and 0.
        bottomOfLevel = Mathf.Min(0, bottomOfLevel);
    }

    public void Start() {
        waveSystem = Object.FindObjectOfType<EnemyWaveSpawner>();
    }

    protected virtual void CalculateMovement() {
        transform.Translate(Vector3.down * standardSpeed * Time.deltaTime, Space.World);
        if (transform.position.y <= BottomOfLevel) {
            float randx = Random.Range(waveSystem.MinXSpawnPoint, waveSystem.MaxXSpawnPoint);
            transform.position = new Vector3(randx, waveSystem.TopYSpawnPoint, 0);
        }
    }
}