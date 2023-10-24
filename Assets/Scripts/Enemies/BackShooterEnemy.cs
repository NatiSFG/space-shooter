using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackShooterEnemy : Enemy {
    [SerializeField] GameObject laserPrefab;
    
    private BackShooterController2D controller;
    private WaveSystem waveSystem;
    private EnemyInfo enemyInfo;



    private new void Start() {
        base.Start();
        enemyInfo = new EnemyInfo(laserPrefab, controller, Random.Range(2, 5), -1);
        controller = GetComponent<BackShooterController2D>();
        waveSystem = FindObjectOfType<WaveSystem>();
    }

    private void Update() {
        FireLaser();
    }

    protected override void FireLaser() {
        if (Time.time > enemyInfo.canFire && isAlive && !waveSystem.IsPlayerDefeated) {
            enemyInfo.canFire = Time.time + enemyInfo.fireRate;
            GameObject enemyLaser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            for (int i = 0; i < lasers.Length; i++)
                lasers[i].AssignBackShooterLaser();
        }
    }
}
