using UnityEngine;

public class DoubleBeamerEnemy : Enemy {
    [SerializeField] GameObject laserPrefab;

    private DoubleBeamerController2D controller;
    private EnemyInfo enemyInfo;

    private new void Start() {
        base.Start();
        enemyInfo = new EnemyInfo(laserPrefab, controller, Random.Range(3, 7), -1);
        controller = GetComponent<DoubleBeamerController2D>();
    }

    private void Update() {
        FireLaser();
    }

    protected override void FireLaser() {
        if (Time.time > enemyInfo.canFire && isAlive) {
            enemyInfo.canFire = Time.time + enemyInfo.fireRate;
            GameObject enemyLaser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            for (int i = 0; i < lasers.Length; i++)
                lasers[i].AssignDoubleBeamerLaser();
        }
    }

}
