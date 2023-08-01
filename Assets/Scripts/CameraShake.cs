using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    [SerializeField] private float magnitude = 0.05f;
    [SerializeField] private float duration;

    private Vector3 initialPos;
    private Camera mainCamera;

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
}
