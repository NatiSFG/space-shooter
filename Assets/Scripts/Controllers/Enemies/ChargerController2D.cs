using System.Collections;
using UnityEngine;

public class ChargerController2D : EnemyController2D {
    [SerializeField] private float currentSpeed;
    [SerializeField] private float aggressionSpeedMultiplier = 1.7f;
    [SerializeField] private float rangeX = 1f;
    [SerializeField] private float rangeY = 8f;

    private Transform player;
    private bool isAggressive = false;

    public bool IsAggressive {
        get { return isAggressive; }
        set { isAggressive = value; }
    }

    private new void Start() {
        base.Start();
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
        if (transform.position.y <= BottomOfLevel) {
            float randx = Random.Range(spawner.MinXSpawnPoint, spawner.MaxXSpawnPoint);
            transform.position = new Vector3(randx, spawner.TopYSpawnPoint, 0);
        }
    }

    private IEnumerator CheckDistanceToPlayer() {
        WaitForSeconds wait = new WaitForSeconds(0.5f);
        while (player != null) {
            //calculate the distance to the player
            float distanceX = Mathf.Abs(player.position.x - transform.position.x);
            float distanceY = Mathf.Abs(player.position.y - transform.position.y);

            //check if the player is within the aggression range
            if (distanceX <= rangeX && distanceY <= rangeY) {
                IsAggressive = true;
            }
            else if (IsAggressive) {
                //enemy is out of range, return to standard speed
                IsAggressive = false;
            }
            yield return wait;
        }
    }
}
