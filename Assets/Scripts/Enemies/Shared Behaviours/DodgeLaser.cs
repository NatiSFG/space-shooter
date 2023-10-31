using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeLaser : MonoBehaviour {
    //avoid the player’s lasers. When the player shoots, the enemy detects a laser
    //in range and tries to avoid it.

    private Laser laser;
    private int dodgeDistance = 3;
    private int speed = 3;

    private void Start() {
        laser = GetComponent<Laser>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player Laser") {
            Vector2 currentPos = transform.parent.position;
            Vector2 targetPos = currentPos;
            //0 is left, 1 is right. move 2.5 units to the left or right
            int direction = Random.Range(0, 1);
            if (direction == 0) {
                targetPos.x = Mathf.Lerp(currentPos.x, -2.5f, 1f);
                Debug.Log("target position is set to lerp");
                transform.parent.position = (targetPos.x, transform.parent.position.y, transform.parent.position.z);
            } else if (direction == 1) {
                targetPos.x = Mathf.Lerp(currentPos.x, 2.5f, 1f);
            }
        }
    }
}
