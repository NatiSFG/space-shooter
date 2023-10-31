using System.Collections;
using UnityEngine;

public class DodgerController2D : EnemyController2D {
    [SerializeField] private float rotateSpeed;

    private void Update() {
        CalculateMovement();
    }

    //avoid the player’s lasers. When the player shoots, the enemy detects a laser
    //in range and tries to avoid it.

    //private IEnumerator CheckIfLaserInRange() {
    //    WaitForSeconds wait = new WaitForSeconds(0.5f);
    //    while ( ()
    //    {
            
    //    }
    //}
}
