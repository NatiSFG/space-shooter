using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DoubleBeamerEnemy : Enemy {
    [SerializeField] GameObject laserPrefab;
    [SerializeField] private float rangeX = 1f;
    [SerializeField] private Collectable[] collectables;

    private GameObject collectableContainer;
    private DoubleBeamerController2D controller;
    private WaveSystem waveSystem;
    private EnemyInfo enemyInfo;

    private bool isCollectableBelowInLine = false;

    public bool IsCollectableBelowInLine {
        get { return isCollectableBelowInLine; }
        set { isCollectableBelowInLine = value; }
    }

    private new void Start() {
        base.Start();
        enemyInfo = new EnemyInfo(laserPrefab, controller, Random.Range(3, 7), -1);
        controller = GetComponent<DoubleBeamerController2D>();
        waveSystem = FindObjectOfType<WaveSystem>();
        collectableContainer = GameObject.FindGameObjectWithTag("Collectable Container");
    }

    private void Update() {
        //FireLaser();
        //find all collectables in container
        collectables = collectableContainer.GetComponentsInChildren<Collectable>();
        StartCoroutine(CheckIfCollectableInFront());
    }

    protected override void FireLaser() {
        if (Time.time > enemyInfo.canFire && isAlive && !waveSystem.IsPlayerDefeated) {
            enemyInfo.canFire = Time.time + enemyInfo.fireRate;
            GameObject enemyLaser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            for (int i = 0; i < lasers.Length; i++)
                lasers[i].AssignDoubleBeamerLaser();
        }
    }

    private IEnumerator CheckIfCollectableInFront() {
        WaitForSeconds wait = new WaitForSeconds(0.5f);
        while (this.gameObject != null && collectables.Length > 0) {
            foreach (Collectable collectable in collectables) {
                if (Mathf.Abs(collectable.transform.position.x - transform.position.x) <= rangeX
                    && collectable.transform.position.y < transform.position.y) {
                    if (!IsCollectableBelowInLine) {
                        IsCollectableBelowInLine = true;
                        GameObject enemyLaser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
                        Laser laser = enemyLaser.GetComponent<Laser>();
                        laser.AssignDoubleBeamerLaser();
                        //collectable destroyed in Laser OnTriggerEnter2D
                    } else {
                        if (IsCollectableBelowInLine)
                            IsCollectableBelowInLine = false;
                    } yield return wait;
                }
            }
        }
    }
}
