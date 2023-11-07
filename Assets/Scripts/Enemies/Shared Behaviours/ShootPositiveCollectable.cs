using System.Collections;
using UnityEngine;

public class ShootPositiveCollectable : MonoBehaviour {
    [SerializeField] private GameObject laserPrefab;

    private Laser[] lasers;
    private Laser laser;
    private GameObject parentEnemyobj;
    private Animator anim;

    private void Start() {
        parentEnemyobj = transform.parent.gameObject;
        if (GetComponent<Animator>() != null)
            anim = GetComponent<Animator>();
        else if (GetComponentInChildren<Animator>() != null)
            anim = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Destroyable by Laser") {
            if (parentEnemyobj.GetComponent<DoubleBeamerEnemy>() != null && anim.GetBool("OnEnemyDeath") == false) {
                GameObject enemyLaser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
                lasers = enemyLaser.GetComponentsInChildren<Laser>();
                foreach (Laser laser in lasers)
                    laser.AssignDoubleBeamerLaser();
            }

            if (parentEnemyobj.GetComponent<BackShooterEnemy>() != null && anim.GetBool("OnEnemyDeath") == false) {
                GameObject enemyLaser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
                laser = enemyLaser.GetComponent<Laser>();
                laser.AssignBackShooterLaser();
            }
        }
    }
}
