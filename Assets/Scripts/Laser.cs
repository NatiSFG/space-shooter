using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Laser : MonoBehaviour {

    [SerializeField] public static float speed = 8;
    private ShipMovementController2D playerController;
    private bool isDoubleBeamerLaser;
    private bool isSpinnerLaser;
    private SpriteRenderer playerSprite;

    public bool isPlayerFrozen;

    private void Start() {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<ShipMovementController2D>();
        playerSprite = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();
    }

    private void Update() {
        if (isDoubleBeamerLaser == false && isSpinnerLaser == false)
            PlayerLaser();
        else EnemyLaser();
    }

    private void PlayerLaser() {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        
        if (transform.position.y >= 8) {
            if(transform.parent != null) {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    private void EnemyLaser() {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (transform.position.y <= -8) {
            if (transform.parent != null) {
                Destroy(transform.parent.gameObject);
            } else Destroy(this.gameObject);
        }
    }

    public void AssignDoubleBeamerLaser() {
        isDoubleBeamerLaser = true;
    }

    public void AssignSpinnerLaser() {
        isSpinnerLaser = true;
    }

    public bool IsDoubleBeamerLaser() {
        return isDoubleBeamerLaser;
    }

    public bool IsSpinnerLaser() {
        return isSpinnerLaser;
    }

    public bool IsEnemyLaser() {
        if (isDoubleBeamerLaser || isSpinnerLaser)
            return true;
        else return false;
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if (isDoubleBeamerLaser) {
            HealthEntity player = other.GetComponentInParent<HealthEntity>();
            if (player != null) {
                player.TryDamage();
                Destroy(transform.parent.gameObject);
            }
        }
        else if (isSpinnerLaser) {
            HealthEntity player = other.GetComponentInParent<HealthEntity>();
            if (player != null) {
                player.TryDamage();
                if (!player.IsShieldPowerUpActive) {
                    isPlayerFrozen = true;
                    StartCoroutine(FreezeCoroutine());

                    if (!isPlayerFrozen)
                        Destroy(transform.gameObject);
                }
            }
        }
    }

    public void FreezePlayer() {

    }

    public IEnumerator FreezeCoroutine() {
        while (isPlayerFrozen) {
            playerController.Speed = 0;
            playerSprite.color = new Color(0.4039f, 0.9019f, 1.0f, 1.0f);
            yield return new WaitForSeconds(2);
            playerSprite.color = Color.white;
            playerController.Speed = 5;
            isPlayerFrozen = false;
        }
    }
}
