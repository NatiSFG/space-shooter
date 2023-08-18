using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleBeamerEnemy : Enemy {
    [SerializeField] GameObject laserPrefab;

    private DoubleBeamerController2D controller;
    private EnemyInfo enemyInfo;

    private void Start() {
        enemyInfo = new EnemyInfo(laserPrefab, controller, 3, -1);
        controller = GetComponent<DoubleBeamerController2D>();
    }

    private void Update() {
        FireLaser();
    }

    protected override void FireLaser() {
        if (Time.time > enemyInfo.canFire && isAlive) {
            enemyInfo.fireRate = Random.Range(3, 7);
            enemyInfo.canFire = Time.time + enemyInfo.fireRate;
            GameObject enemyLaser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            for (int i = 0; i < lasers.Length; i++)
                lasers[i].AssignEnemyLaser();
        }
    }

}
