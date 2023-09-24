using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnerEnemy : Enemy {
    [SerializeField] GameObject iceLaserPrefab;

    private SpinnerController2D controller;
    private EnemyInfo enemyInfo;

    private new void Start() {
        base.Start();
        enemyInfo = new EnemyInfo(iceLaserPrefab, controller, Random.Range(1, 4), -1);
        controller = GetComponent<SpinnerController2D>();
    }

    private void Update() {
        FireLaser();
    }

    protected override void FireLaser() {
        if (Time.time > enemyInfo.canFire && isAlive) {
            enemyInfo.canFire = Time.time + enemyInfo.fireRate;
            GameObject enemyLaser = Instantiate(iceLaserPrefab, transform.position, Quaternion.identity);

            Vector2 launchDirection = transform.forward;
            Rigidbody2D rb = enemyLaser.GetComponent<Rigidbody2D>();
            rb.velocity = launchDirection * Laser.speed;
            enemyLaser.transform.rotation = transform.rotation;

            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            for (int i = 0; i < lasers.Length; i++)
                lasers[i].AssignSpinnerLaser();
        }
    }
}
