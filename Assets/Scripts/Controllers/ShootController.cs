using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShootController : MonoBehaviour {
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private AudioClip laserClip;
    [SerializeField] private AudioClip outOfAmmoClip;
    [SerializeField] private float fireRate = 0.15f;
    [SerializeField] private int ammoCount = 25;

    [Header("Triple Shot PowerUp")]
    [SerializeField] private GameObject tripleShotLasersPrefab;
    [SerializeField] private Image tripleShotPowerUpImage;

    [Header("Homing Shot PowerUp")]
    [SerializeField] private GameObject homingShotLaserPrefab;
    [SerializeField] private Image homingShotPowerUpImage;

    private new AudioSource audio;
    private float canFire = -1;
    private bool isTripleShotPowerUpActive;
    private bool isHomingShotPowerUpActive;
    private Laser laser;
    private Laser[] lasers;
    private Vector3 laserOffset;

    public event Action onAmmoCountChanged;

    public int AmmoCount => ammoCount;

    private void Start() {
        audio = GetComponent<AudioSource>();
        laser = GetComponent<Laser>();
        laserOffset = new Vector3(0, 1.075f, 0);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > canFire)
            FireLaser();
        if (Input.GetKeyDown(KeyCode.L))
            AddAmmoCount(100);
    }

    public void AddAmmoCount(int count) {
        ammoCount += count;
        if (onAmmoCountChanged != null)
            onAmmoCountChanged();
    }

    public void SubtractAmmo(int ammo) {
        ammoCount -= ammo;
        if (ammoCount < 0)
            ammoCount = 0;
        if (onAmmoCountChanged != null)
            onAmmoCountChanged();
    }

    private void FireLaser() {
        canFire = Time.time + fireRate;

        if (Input.GetKeyDown(KeyCode.Space) && ammoCount >= 1) {
            if (isTripleShotPowerUpActive) {
                GameObject tripleShot = Instantiate(tripleShotLasersPrefab, transform.position + laserOffset, Quaternion.identity);
                lasers = tripleShot.GetComponentsInChildren<Laser>();
                for (int i = 0; i < lasers.Length; i++)
                    lasers[i].AssignPlayerLaser();
                SubtractAmmo(3);
            } else if (isHomingShotPowerUpActive) {
                GameObject homingShot = Instantiate(homingShotLaserPrefab, transform.position + laserOffset, Quaternion.identity);
                laser = homingShot.GetComponent<Laser>();
                laser.AssignHomingLaser();
                Destroy(homingShot, 5);
                SubtractAmmo(1);
            }  else {
                GameObject singleLaser = Instantiate(laserPrefab, transform.position + laserOffset, Quaternion.identity);
                laser = singleLaser.GetComponent<Laser>();
                laser.AssignPlayerLaser();
                SubtractAmmo(1);
            }
            audio.clip = laserClip;
            audio.Play();
        }

        if (Input.GetKeyDown(KeyCode.Space) && ammoCount <= 0) {
            audio.clip = outOfAmmoClip;
            audio.Play();
        }
    }

    public void TripleShotPowerUpActive() {
        isTripleShotPowerUpActive = true;
        tripleShotPowerUpImage.enabled = true;
        StartCoroutine(TripleShotPowerDownCoroutine());
    }

    private IEnumerator TripleShotPowerDownCoroutine() {
        WaitForSeconds wait = new WaitForSeconds(5);
        while (isTripleShotPowerUpActive) {
            yield return wait;
            tripleShotPowerUpImage.enabled = false;
            isTripleShotPowerUpActive = false;
        }
    }

    public void HomingShotPowerUpActive() {
        isHomingShotPowerUpActive = true;
        homingShotPowerUpImage.enabled = true;
        StartCoroutine(HomingShotPowerDownCoroutine());
    }

    private IEnumerator HomingShotPowerDownCoroutine() {
        WaitForSeconds wait = new WaitForSeconds(5);
        while (isHomingShotPowerUpActive) {
            yield return wait;
            homingShotPowerUpImage.enabled = false;
            isHomingShotPowerUpActive = false;
        }
    }
}

