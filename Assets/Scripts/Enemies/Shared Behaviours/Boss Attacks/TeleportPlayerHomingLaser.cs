using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TeleportPlayerHomingLaser : MonoBehaviour {
    [SerializeField] private float speed = 8f;
    [SerializeField] private float rotationSpeed = 300f;

    private LevelBounds level;
    private Transform target;

    private void Start() {
        level = Object.FindObjectOfType<LevelBounds>();
        FindPlayer();
    }

    private void Update() {
        Movement();
    }

    private void FindPlayer() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        float minDistance = float.MaxValue;
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance < minDistance) {
            minDistance = distance;
            target = player.transform;
        }
    }

    private void Movement() {
        if (target == null)
            transform.Translate(Vector3.down * speed * Time.deltaTime);
        else {
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            Vector3 eulerAngles = new Vector3();
            float rawAngle = Mathf.Atan2(dirToTarget.y, dirToTarget.x) * Mathf.Rad2Deg;
            eulerAngles.z = rawAngle - 90;

            transform.rotation = Quaternion.RotateTowards(transform.rotation,
                Quaternion.Euler(eulerAngles), rotationSpeed * Time.deltaTime);

            //move the laser forward in the direction of the target
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
        if (LaserOutOfBounds())
            Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<HealthEntity>()) {
            HealthEntity player = other.GetComponentInParent<HealthEntity>();
            if (player != null) {
                player.TryDamage();
                //teleport player game object to a random point within a set space
                Destroy(this.gameObject);
            }
        }
    }

    public bool LaserOutOfBounds() {
        float x = transform.position.x;
        float y = transform.position.y;
        if (x < level.LeftBound || x > level.RightBound ||
            y < level.BottomBound || y > level.TopBound)
            return true;
        else return false;
    }
}
