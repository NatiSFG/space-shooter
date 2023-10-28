using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackShooterEnemy : Enemy {
    [SerializeField] GameObject laserPrefab;
    [SerializeField] private float rangeX = 1f;
    
    private BackShooterController2D controller;
    private WaveSystem waveSystem;
    private EnemyInfo enemyInfo;

    private Transform player;
    private bool isPlayerAboveInLine = false;

    public bool IsPlayerAboveInLine {
        get { return isPlayerAboveInLine; }
        set { isPlayerAboveInLine = value; }
    }

    private new void Start() {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyInfo = new EnemyInfo(laserPrefab, controller, Random.Range(2, 5), -1);
        controller = GetComponent<BackShooterController2D>();
        waveSystem = FindObjectOfType<WaveSystem>();

        //continuously check if the player is within range
        StartCoroutine(CheckIfPlayerInRange());
    }

    protected override void FireLaser() {
        if (Time.time > enemyInfo.canFire && isAlive && !waveSystem.IsPlayerDefeated && IsPlayerAboveInLine) {
            enemyInfo.canFire = Time.time + enemyInfo.fireRate;
            GameObject enemyLaser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            for (int i = 0; i < lasers.Length; i++)
                lasers[i].AssignBackShooterLaser();
        }
    }

    private IEnumerator CheckIfPlayerInRange() {
        WaitForSeconds wait = new WaitForSeconds(0.5f);
        while (player != null) {
            //calculate the distance to the player
            float distanceX = Mathf.Abs(player.position.x - transform.position.x);

            //check if the player is within line of shooting
            if (distanceX <= rangeX && transform.position.y < player.position.y) {
                if (!IsPlayerAboveInLine) {
                    IsPlayerAboveInLine = true;
                    FireLaser();
                }
            } else {
                if (IsPlayerAboveInLine)
                    IsPlayerAboveInLine = false;
            } yield return wait;
        }
    }
}
