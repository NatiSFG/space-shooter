using UnityEngine;

public class SpinnerController2D : EnemyController2D {
    [SerializeField] private float rotateSpeed;
    
    void Update() {
        CalculateMovement();
    }

    protected override void CalculateMovement() {
        base.CalculateMovement();
        transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
        //transform.localRotation = Quaternion.Euler(0f, 0f, 1f);
    }
}
