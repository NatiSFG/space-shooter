using UnityEngine;
using UnityEngine.UI;

public class MagnetPull : MonoBehaviour {
    [SerializeField] private float magnetStrength = 10.0f;
    [SerializeField] private GameObject magnet;
    [SerializeField] private Image magnetImage;

    private float magnetDuration = 10.0f;
    private float timeLeft;
    private bool isMagnetActive = false;

    public bool IsMagnetActive {
        get { return isMagnetActive; }
        set { isMagnetActive = value; }
    }

    private void Update() {
        if (IsMagnetActive) {
            timeLeft -= Time.deltaTime;

            if (timeLeft <= 0) {
                magnetImage.enabled = false;
                magnet.SetActive(false);
                IsMagnetActive = false;
            }
        }
    }

    //we apply a force to all PowerUp objects in the scene if the magnet effect is active
    private void FixedUpdate() {
        if (IsMagnetActive) {
            GameObject[] positiveCollectables = GameObject.FindGameObjectsWithTag("Positive Collectable");
            foreach (var powerUp in positiveCollectables) {
                Rigidbody2D rb = powerUp.GetComponent<Rigidbody2D>();
                if (rb != null) {
                    Vector2 direction = (transform.position - powerUp.transform.position).normalized;
                    rb.AddForce(direction * magnetStrength);
                }
            }
        }
    }

    public void MagnetPowerUpActive() {
        IsMagnetActive = true;
        timeLeft = magnetDuration;
        magnetImage.enabled = true;
        magnet.SetActive(true);
    }
}
