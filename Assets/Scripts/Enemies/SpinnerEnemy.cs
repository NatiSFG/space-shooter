using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnerEnemy : Enemy {
    [SerializeField] GameObject iceLaserPrefab;

    private SpinnerController2D controller;
    private EnemyInfo enemyInfo;

    private new void Start() {
        base.Start();
        controller = GetComponent<SpinnerController2D>();
        enemyInfo = new EnemyInfo(iceLaserPrefab, controller, Random.Range(1, 4), -1);
    }

    private void Update() {
        FireLaser();
    }

    protected override void FireLaser() {
        if (Time.time > enemyInfo.canFire && isAlive) {
            enemyInfo.canFire = Time.time + enemyInfo.fireRate;
            GameObject enemyLaser = Instantiate(iceLaserPrefab, transform.position, Quaternion.identity);

            Vector2 launchDirection = transform.forward;
            enemyLaser.transform.rotation = transform.rotation;

            Laser laser = enemyLaser.GetComponentInChildren<Laser>();
            laser.AssignSpinnerLaser();
            Rigidbody2D rb = enemyLaser.GetComponent<Rigidbody2D>();
            rb.velocity = launchDirection * laser.Speed;
        }
    }
}
