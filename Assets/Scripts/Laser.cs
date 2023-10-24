using System.Collections;
using UnityEngine;

public class Laser : MonoBehaviour {

    [SerializeField] private float speed = 8;

    private LevelBounds levelBounds;
    private ShipMovementController2D playerController;
    private bool isDoubleBeamerLaser;
    private bool isSpinnerLaser;
    private bool isBackShooterLaser;
    private SpriteRenderer playerSprite;

    public float Speed => speed;
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
    }

    private void Update() {
        if (!IsDoubleBeamerLaser && !IsSpinnerLaser && !IsBackShooterLaser)
            PlayerLaser();
        else if (IsDoubleBeamerLaser || IsSpinnerLaser)
            DownwardEnemyLaser();
        else UpwardEnemyLaser();
    }

    private void PlayerLaser() {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        
        if (transform.position.y >= levelBounds.topBound) {
            if(transform.parent != null) {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
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
        if (x < levelBounds.leftBound || x > levelBounds.rightBound ||
            y < levelBounds.bottomBound || y > levelBounds.topBound)
            return true;
        else return false;
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
        if (IsDoubleBeamerLaser || IsBackShooterLaser) {
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
    }

     public IEnumerator FreezeCoroutine() {
        playerController.Speed = 0;
        playerSprite.color = new Color(0.4039f, 0.9019f, 1.0f, 1.0f);
        yield return new WaitForSeconds(2);
        playerSprite.color = Color.white;
        playerController.Speed = 5;
     }
}
