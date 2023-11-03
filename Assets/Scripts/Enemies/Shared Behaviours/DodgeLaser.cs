using UnityEngine;

public class DodgeLaser : MonoBehaviour {
    [SerializeField] private float dodgeDistance = 2f;
    [SerializeField] private float speed = 5f;
    [SerializeField] private Transform dodger;

    private EnemyWaveSpawner spawner;
    private Transform lastNoticedLaser;
    private Vector3 lastDodgeStartPos;
    private Vector3 targetPos;

    private void Start() {
        spawner = Object.FindObjectOfType<EnemyWaveSpawner>();
    }

    private void Update() {
        if (lastNoticedLaser != null) {
            Vector3 currentPos = dodger.position;
            currentPos.x = Mathf.Lerp(currentPos.x, targetPos.x, speed * Time.deltaTime);
            dodger.position = currentPos;
            if (Mathf.Abs(currentPos.x - targetPos.x) < 0.001f)
                lastNoticedLaser = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player Laser") {
            lastNoticedLaser = other.transform;
            lastDodgeStartPos = dodger.position;
            targetPos = lastDodgeStartPos;
            int direction = Random.Range(0, 2);
            float leftSpaceAvailable = Mathf.Abs(spawner.MinXSpawnPoint - lastDodgeStartPos.x);
            float rightSpaceAvailable = Mathf.Abs(spawner.MaxXSpawnPoint - lastDodgeStartPos.x);
            if (direction == 0 && leftSpaceAvailable >= dodgeDistance)
                targetPos.x -= dodgeDistance; //new x value to move to the left
            else if (direction == 1 && rightSpaceAvailable >= dodgeDistance)
                targetPos.x += dodgeDistance; //new x value to move to the right
        }
    }
}
