using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController2D : MonoBehaviour {
    [SerializeField] private float speed = 3f;

    private Vector3 endPos = new Vector3(0, 3, 0);

    private void Update() {
        MoveDownScreen();
    }

    private void MoveDownScreen() {
        transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);
        if (transform.position.y <= endPos.y)
            transform.position = endPos;
    }
}
