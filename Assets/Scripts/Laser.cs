using System.Collections;
using UnityEngine;

public class Laser : MonoBehaviour {

    [SerializeField] private float speed = 8f;

    [Header("Homing Laser")]
    [SerializeField] private float rotationSpeed = 300f;

    private LevelBounds levelBounds;
    private ShipMovementController2D playerController;
    private bool isPlayerLaser;
    private bool isHomingLaser;
    private bool isDoubleBeamerLaser;
    private bool isSpinnerLaser;
    private bool isBackShooterLaser;
    private SpriteRenderer playerSprite;
    private Transform target;

    public float Speed => speed;

    public bool IsPlayerLaser {
        get { return isPlayerLaser; }
        set { isPlayerLaser = value; }
    }

    public bool IsHomingLaser {
        get { return isHomingLaser; }
        set { isHomingLaser = value; }
    }

    public bool IsEnemyLaser => isDoubleBeamerLaser || isSpinnerLaser || isBackShooterLaser;
    public bool IsDoubleBeamerLaser {
        get { return isDoubleBeamerLaser; }
        set { isDoubleBeamerLaser = value; }
    }

    public bool IsSpinnerLaser {
        get { return isSpinnerLaser; }
        set { isSpinnerLaser = value; }
    }

    public bool IsBackShooterLaser {
        get { return isBackShooterLaser; }
        set { isBackShooterLaser = value; }
    }

    private void Start() {
        if (playerController != null) {
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<ShipMovementController2D>();
            playerSprite = playerController.GetComponent<SpriteRenderer>();
        }
        levelBounds = Object.FindObjectOfType<LevelBounds>();
        FindNearestTarget(); //may need to move this. this was in start originally
    }

    private void Update() {
        if (!IsDoubleBeamerLaser && !IsSpinnerLaser && !IsBackShooterLaser && !IsHomingLaser)
            PlayerLaser();
        else if (IsHomingLaser)
            HomingLaser();
        else if (IsDoubleBeamerLaser || IsSpinnerLaser)
            DownwardEnemyLaser();
        else UpwardEnemyLaser();
    }

    private void PlayerLaser() {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        
        if (LaserOutOfBounds()) {
            if (transform.parent != null)
                Destroy(transform.parent.gameObject);
            Destroy(this.gameObject);
        }
    }
    
    public void HomingLaser() {
        if (target == null)
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        else {
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            Vector3 eulerAngles = new Vector3();
            float rawAngle = Mathf.Atan2(dirToTarget.y, dirToTarget.x) * Mathf.Rad2Deg;
            eulerAngles.z = rawAngle - 90; //TODO: figure out why it's -90 in both cases. x < 0 and x > 0

            transform.rotation = Quaternion.RotateTowards(transform.rotation,
                Quaternion.Euler(eulerAngles), rotationSpeed * Time.deltaTime);

            //move the laser forward in the direction of the target
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
        if (LaserOutOfBounds()) {
            if (transform.parent != null)
                Destroy(transform.parent.gameObject);
            Destroy(this.gameObject);
        }
    }

    private void FindNearestTarget() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float minDistance = float.MaxValue;
        foreach (var enemy in enemies) {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance) {
                minDistance = distance;
                target = enemy.transform;
            }
        }
    }

    private void DownwardEnemyLaser() {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
        if (LaserOutOfBounds()) {
            if (transform.parent != null)
                Destroy(transform.parent.gameObject);
            else Destroy(this.gameObject);
        }
    }

    private void UpwardEnemyLaser() {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        if (LaserOutOfBounds()) {
            if (transform.parent != null)
                Destroy(transform.parent.gameObject);
            else Destroy(this.gameObject);
        }
    }

    public bool LaserOutOfBounds() {
        float x = transform.position.x;
        float y = transform.position.y;
        if (x < levelBounds.LeftBound || x > levelBounds.RightBound ||
            y < levelBounds.BottomBound || y > levelBounds.TopBound)
            return true;
        else return false;
    }

    public void AssignPlayerLaser() {
        IsPlayerLaser = true;
    }

    public void AssignHomingLaser() {
        IsHomingLaser = true;
    }

    public void AssignDoubleBeamerLaser() {
        IsDoubleBeamerLaser = true;
    }

    public void AssignSpinnerLaser() {
        IsSpinnerLaser = true;
    }

    public void AssignBackShooterLaser() {
        IsBackShooterLaser = true;
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if ((IsDoubleBeamerLaser || IsBackShooterLaser) && other.GetComponent<HealthEntity>()) {
            HealthEntity player = other.GetComponentInParent<HealthEntity>();
            if (player != null) {
                player.TryDamage();
                if (transform.parent != null) {
                    Destroy(transform.parent.gameObject);
                } else Destroy(this.gameObject);
            }
        }
        else if (IsSpinnerLaser) {
            HealthEntity player = other.GetComponentInParent<HealthEntity>();
            if (player != null) {
                DamageResult d = player.TryDamage();
                if (d == DamageResult.ShieldDamaged)
                    Destroy(transform.gameObject);
                if (d == DamageResult.Success) {
                    player.StartCoroutine(FreezeCoroutine());
                    Destroy(transform.gameObject);
                }
            }
        }
        if ((IsDoubleBeamerLaser || IsBackShooterLaser) && other.tag == "Destroyable by Laser") {
            GameObject parentObj = other.transform.parent.gameObject;
            Destroy(parentObj);
        }
    }

    public IEnumerator FreezeCoroutine() {
        if (playerController != null) {
            playerController.Speed = 0;
            playerSprite.color = new Color(0.4039f, 0.9019f, 1.0f, 1.0f);
            yield return new WaitForSeconds(2);
            playerSprite.color = Color.white;
            playerController.Speed = 5;
        }
    }
}
