using System.Collections;
using UnityEngine;

public class DodgeLaser : MonoBehaviour {
    [SerializeField] private float dodgeDistance = 2f;
    [SerializeField] private float speed = 5f;
    [SerializeField] private Transform dodgerEnemy;

    private EnemyWaveSpawner enemyWaveSpawner;
    private Transform lastNoticedLaser;
    private Vector3 lastDodgeStartPos;
    private Vector3 targetPos;

    private void Start() {
        enemyWaveSpawner = Object.FindObjectOfType<EnemyWaveSpawner>();
    }

    private void Update() {
        if (lastNoticedLaser != null) {
            Vector3 currentPos = dodgerEnemy.position;
            currentPos.x = Mathf.Lerp(currentPos.x, targetPos.x, speed * Time.deltaTime);
            dodgerEnemy.position = currentPos;
            if (Mathf.Abs(currentPos.x - targetPos.x) < 0.001f) {
                lastNoticedLaser = null;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player Laser") {
            lastNoticedLaser = other.transform;
            lastDodgeStartPos = dodgerEnemy.position;
            targetPos = lastDodgeStartPos;
            int direction = Random.Range(0, 2);
            if (direction == 0)
                targetPos.x -= dodgeDistance; //move to the left
            else targetPos.x += dodgeDistance; //move to the right
        }
    }
}
