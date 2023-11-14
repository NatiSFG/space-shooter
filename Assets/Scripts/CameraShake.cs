using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    [SerializeField] private float magnitude = 0.05f;
    [SerializeField] private float duration;

    [Header("Hyperbeam Screen Shake")]
    [SerializeField] private float hbMagnitude = 0.03f;
    [SerializeField] private float hbDuration;

    private Vector3 initialPos;
    private Camera mainCamera;

    private Vector3 hbInitalPos;

    private void Start() {
        mainCamera = GetComponent<Camera>();
    }

    public void Shake() {
        initialPos = mainCamera.transform.position;
        StartCoroutine(CameraShakeCoroutine());
    }

    private IEnumerator CameraShakeCoroutine() {
        duration = Time.time + 0.5f;
        while (Time.time < duration) {
            WaitForSeconds wait = new WaitForSeconds(0.005f);
            float offsetX = Random.value * magnitude * 2 - magnitude;
            float offsetY = Random.value * magnitude * 2 - magnitude;
            Vector3 offsetPos = mainCamera.transform.position;
            offsetPos.x += offsetX;
            offsetPos.y += offsetY;
            mainCamera.transform.position = offsetPos;
            yield return wait;
        }
        mainCamera.transform.position = initialPos;
    }

    public void HyperbeamShake() {
        hbInitalPos = mainCamera.transform.position;
        StartCoroutine(HyperbeamShakeCoroutine());
    }

    private IEnumerator HyperbeamShakeCoroutine() {
        hbDuration = Time.time + 6.0f;
        while (Time.time < hbDuration) {
            WaitForSeconds wait = new WaitForSeconds(0.005f);
            float offsetX = Random.value * hbMagnitude * 2 - hbMagnitude;
            float offsetY = Random.value * hbMagnitude * 2 - hbMagnitude;
            Vector3 offsetPos = mainCamera.transform.position;
            offsetPos.x += offsetX;
            offsetPos.y += offsetY;
            mainCamera.transform.position = offsetPos;
            yield return wait;
        }
        mainCamera.transform.position = hbInitalPos;
    }
}
