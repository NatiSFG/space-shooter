using UnityEngine;

public class ShootPositiveCollectable : MonoBehaviour {
    [SerializeField] private GameObject laserPrefab;

    private Laser[] lasers;
    private Laser laser;
    private GameObject parentEnemyobj;

    private void Start() {
        parentEnemyobj = transform.parent.gameObject;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Positive Collectable") {
            if (parentEnemyobj.GetComponent<DoubleBeamerEnemy>() != null) {
                GameObject enemyLaser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
                lasers = enemyLaser.GetComponentsInChildren<Laser>();
                foreach (Laser laser in lasers)
                    laser.AssignDoubleBeamerLaser();
            }

            if (parentEnemyobj.GetComponent<BackShooterEnemy>() != null) {
                GameObject enemyLaser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
                laser = enemyLaser.GetComponent<Laser>();
                laser.AssignBackShooterLaser();
            }
        }
    }
}
