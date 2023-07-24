using UnityEngine;

public class CameraShake : MonoBehaviour {

    [SerializeField] private float magnitude = 0.05f;
    [SerializeField] private float time = 0.5f;

    private Vector3 initialPos;
    private Camera mainCamera;

    private void Start() {
        mainCamera = GetComponent<Camera>();
    }

    public void Shake() {
        initialPos = mainCamera.transform.position;
        InvokeRepeating("StartCameraShake", 0f, 0.005f);
        Invoke("StopCameraShake", time);
    }

    void StartCameraShake() {
        float offsetX = Random.value * magnitude * 2 - magnitude;
        float offsetY = Random.value * magnitude * 2 - magnitude;
        Vector3 offsetPos = mainCamera.transform.position;
        offsetPos.x += offsetX;
        offsetPos.y += offsetY;
        mainCamera.transform.position = offsetPos;
    }

    void StopCameraShake() {
        CancelInvoke("StartCameraShake");
        mainCamera.transform.position = initialPos;
    }
}
