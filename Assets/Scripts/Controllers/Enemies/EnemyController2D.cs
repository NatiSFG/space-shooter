using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController2D : MonoBehaviour {
    [SerializeField] protected float speed = 4;

    public float Speed {
        get { return speed; }
        set { speed = value; }
    }

    private float topOfLevel = 9;
    private float bottomOfLevel = -7;
    public float TopOfLevel => topOfLevel;
    public float BottomOfLevel => bottomOfLevel;

    private void OnValidate() {
        topOfLevel = Mathf.Max(0, topOfLevel);
        //bottomOfLevel will always be 0 or negative, the minimum between itself and 0.
        bottomOfLevel = Mathf.Min(0, bottomOfLevel);
    }

    protected virtual void CalculateMovement() {
        transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);
        if (transform.position.y <= BottomOfLevel) {
            float randX = Random.Range((float) -10, 10);
            transform.position = new Vector3(randX, 9, 0);
        }
    }
}
