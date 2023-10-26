using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleBeamerEnemy : Enemy {
    [SerializeField] GameObject laserPrefab;
    [SerializeField] private float rangeX = 1f;
    [SerializeField] private Transform collectableContainer;
    [SerializeField] private List<Collectable> collectables = new List<Collectable>();

    private DoubleBeamerController2D controller;
    private WaveSystem waveSystem;
    private EnemyInfo enemyInfo;

    private Transform collectable;
    private bool isCollectableBelowInLine = false;
    //private float distanceX;

    public bool IsCollectableBelowInLine {
        get { return isCollectableBelowInLine; }
        set { isCollectableBelowInLine = value; }
    }

    //we want to cycle through all the collectables in the scene and see if they're in line with
    //the enemy. then shoot. find them in the collectable container
    private new void Start() {
        base.Start();
        //because a collectable hasn't spawned yet, at start this is trying to access it
        if (collectableContainer != null && collectableContainer.childCount > 0) {
            collectable = Object.FindObjectOfType<Collectable>().transform;

        }
        //if (collectable.)
        enemyInfo = new EnemyInfo(laserPrefab, controller, Random.Range(3, 7), -1);
        controller = GetComponent<DoubleBeamerController2D>();
        waveSystem = FindObjectOfType<WaveSystem>();
            
        StartCoroutine(CheckIfCollectableInFront());
    }

    private void Update() {
        //FireLaser();
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
        while (this.gameObject != null) {
            float distanceX = Mathf.Abs(collectable.position.x - transform.position.x);

            if (distanceX <= rangeX && transform.position.y > collectable.position.y) {
                if (!IsCollectableBelowInLine) {
                    IsCollectableBelowInLine = true;
                    GameObject enemyLaser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
                    //collectable destroyed in Laser OnTriggerEnter2D
                }
            } else {
                if (IsCollectableBelowInLine)
                    IsCollectableBelowInLine = false;
            } yield return wait;
        }
    }
}
