using System.Collections;
using UnityEngine;

public class DoubleBeamerController2D : EnemyController2D {
    private const float Threshold = 0.001f;
    private Vector3 targetPos;

    private void OnEnable() {
        ResetTargetPosition();
        StartCoroutine(MoveCoroutine());
    }

    /// <summary>
    /// Resets the target position to be located directly below this enemy in the y-axis.
    /// </summary>
    private void ResetTargetPosition() {
        Vector3 currentPos = transform.position;
        Vector3 nextTarget = currentPos;
        nextTarget.y = BottomOfLevel;
        //cleared targetPos complier bug
        targetPos = nextTarget;
    }

    private void SetHorizontalTargetPosition() {
        Vector3 currentPos = transform.position;
        float leftSpaceAvailable = Mathf.Abs(-10 - currentPos.x);
        float rightSpaceAvailable = Mathf.Abs(10 - currentPos.x);

        Vector3 nextTarget = currentPos;
        if (leftSpaceAvailable > rightSpaceAvailable)
            nextTarget.x = Random.Range(-10, Mathf.Lerp(currentPos.x, -10, 0.5f));
        else nextTarget.x = Random.Range(Mathf.Lerp(currentPos.x, 10, 0.5f), 10);
        targetPos = nextTarget;
    }

    private IEnumerator MoveCoroutine() {
        float elapsedTime = 0;
        float cooldown = Random.Range((float) 1, 3);
        while (isActiveAndEnabled) {
            float dt = Time.deltaTime;
            Vector3 currentPos = transform.position;
            Vector3 difference = targetPos - currentPos;
            float speed = (Mathf.Abs(difference.x) > Threshold) ? this.standardSpeed / 2 : this.standardSpeed;
            float distanceAway = difference.magnitude;

            bool reachedAGoal = distanceAway <= Threshold;
            if (reachedAGoal) {
                if (currentPos.y <= BottomOfLevel + Threshold) {
                    //NOTE: We've reached the bottom of the level.
                    currentPos.x = Random.Range((float) -10, 10);
                    currentPos.y = TopOfLevel;
                    transform.position = currentPos;
                } else {
                    //NOTE: We've reached a different goal.
                }
                ResetTargetPosition();
            } else {
                Vector3 direction = difference.normalized;
                float lengthOfMovement = Mathf.Min(distanceAway, speed * dt);
                transform.position = currentPos + lengthOfMovement * direction;
            }

            elapsedTime += dt;
            yield return null;

            if (elapsedTime >= cooldown) {
                elapsedTime = 0;
                cooldown *= 2;
                SetHorizontalTargetPosition();
            }
        }
    }
}
