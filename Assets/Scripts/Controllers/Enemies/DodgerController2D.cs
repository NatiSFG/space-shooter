using System.Collections;
using UnityEngine;

public class DodgerController2D : EnemyController2D {
    [SerializeField] private float rotateSpeed;

    private void Update() {
        CalculateMovement();
    }
}
