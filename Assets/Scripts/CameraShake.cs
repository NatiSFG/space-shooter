using UnityEngine;

public class CameraShake : MonoBehaviour {

    private Vector3 initialPos;

    [SerializeField] private float magnitude = 0.05f;
    [SerializeField] private float time = 0.5f;
    
    private Camera mainCamera;

    void Start() {
        mainCamera = this.gameObject.GetComponent<Camera>();
    }

    public void Shake() {
        initialPos = mainCamera.transform.position;
        InvokeRepeating("StartCameraShake", 0f, 0.005f);
        Invoke("StopCameraShake", time);
    }

    void StartCameraShake() {
        float offsetX = Random.value * magnitude * 2 - magnitude;
        float offsetY = Random.value * magnitude * 2 - magnitude;
        Vector3 medPos = mainCamera.transform.position;
        medPos.x += offsetX;
        medPos.y += offsetY;
        mainCamera.transform.position = medPos;
    }

    void StopCameraShake() {
        CancelInvoke("StartCameraShake");
        mainCamera.transform.position = initialPos;
    }
}
