using System.Collections;
using UnityEngine;

public class EnemyController2D : MonoBehaviour {
    [SerializeField] private float currentSpeed;
    [SerializeField] protected float standardSpeed = 3f;
    [SerializeField] private float aggressionSpeedMultiplier = 1.25f;
    [SerializeField] private float rangeX = 1f;
    [SerializeField] private float rangeY = 4f;

    private Transform player;
    private bool isAggressive = false;

    public float Speed {
        get { return standardSpeed; }
        set { standardSpeed = value; }
    }
    public bool IsAggressive {
        get { return isAggressive; }
        set { isAggressive = value; }
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

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //continuously check the distance to the player
        StartCoroutine(CheckDistanceToPlayer());
    }

    private void Update() {
        //If isAggressive is true, then currentSpeed will be set to standardSpeed *
        //aggressionSpeedMultiplier. Otherwise, if isAggressive is false, currentSpeed will
        //be set to standardSpeed.
        currentSpeed = IsAggressive ? standardSpeed * aggressionSpeedMultiplier : standardSpeed;
        transform.Translate(Vector3.down * currentSpeed * Time.deltaTime);
    }

    private IEnumerator CheckDistanceToPlayer() {
        WaitForSeconds wait = new WaitForSeconds(0.5f);
        while (true) {
            //calculate the distance to the player
            float distanceX = Mathf.Abs(player.position.x - transform.position.x);
            float distanceY = Mathf.Abs(player.position.y - transform.position.y);

            //check if the player is within the aggression range
            if (distanceX <= rangeX && distanceY <= rangeY) {
                if (!IsAggressive) {
                    //enemy enters the range and becomes aggressive
                    IsAggressive = true;
                    Debug.Log("Enemy is aggressive!");
                }
            } else {
                if (IsAggressive) {
                    //enemy is out of range, return to standard speed
                    IsAggressive = false;
                    Debug.Log("Enemy is no longer aggressive.");
                }
            }
            yield return wait;
        }
    }

    protected virtual void CalculateMovement() {
        transform.Translate(Vector3.down * standardSpeed * Time.deltaTime, Space.World);
        if (transform.position.y <= bottomOfLevel) {
            float randx = Random.Range((float) -10, 10);
            transform.position = new Vector3(randx, 9, 0);
        }
    }
}