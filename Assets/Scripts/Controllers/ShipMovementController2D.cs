﻿using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Represents a player-controllable ship in the game, using <see cref="Input.GetAxis(string)"/> with <c>"Horizontal"</c> and <c>"Vertical"</c>.
/// </summary>
public class ShipMovementController2D : MonoBehaviour {
    [Tooltip("The player's currrent speed, measured in world-space units per second.")]
    [SerializeField, Min(0)] private float speed = 5;

    [Header("Speed Boost")]
    [Tooltip("The boosted speed at which you move after activating the speed boost (by using the Shift key).")]
    [SerializeField, Min(0)] private float speedBoost = 7;
    [SerializeField, Min(0.01f)] private float speedBoostDuration = 3;
    [SerializeField, Min(0.01f)] private float speedBoostCooldown = 23;
    [SerializeField] private SpeedBoostBar speedBoostBar;
    [SerializeField] private AudioClip noSpeedBoostClip;

    [Header("Speed PowerUp")]
    [SerializeField] private float speedPowerUpMultiplier = 2;
    [SerializeField] private Image speedPowerUpImage;

    [Header("Speed PowerDown")]
    [SerializeField] private float speedPowerDownSpeed = 3;
    [SerializeField] private Image speedPowerDownImage;

    [Space(20)]
    [SerializeField] private SpriteRenderer thrusterSprite;
    [SerializeField] private Laser laser;

    private LevelBounds level;

    private new AudioSource audio;
    private float baseSpeed;
    private float timeUsedSpeedBoost = float.NegativeInfinity;
    private bool isSpeedPowerUpActive;
    private bool isSpeedPowerDownActive;

    public float Speed {
        get { return speed; }
        set { speed = Mathf.Max(0, value); }
    }

    public float SpeedBoostDuration => speedBoostDuration;
    public float SpeedBoostCooldown => speedBoostCooldown;

    public bool IsSpeedPowerDownActive => isSpeedPowerDownActive;
    public bool IsSpeedPowerUpActive => isSpeedPowerUpActive;
    public bool IsSpeedBoostActive => Time.time < (timeUsedSpeedBoost + speedBoostDuration);
    public bool IsSpeedBoostCoolingDown => Time.time < (timeUsedSpeedBoost + speedBoostCooldown);

    public float TimeUsedSpeedBoost => timeUsedSpeedBoost;
    public float RemainingSpeedBoostCooldown => Mathf.Max(0, (timeUsedSpeedBoost + speedBoostCooldown) - Time.time);
    public bool IsSpeedBoostAvailable => !IsSpeedPowerUpActive && !IsSpeedPowerDownActive && !IsSpeedBoostActive && !IsSpeedBoostCoolingDown;

    private void Awake() {
        baseSpeed = speed;
    }

    private void Start() {
        transform.position = Vector3.zero;
        audio = GetComponent<AudioSource>();
        level = Object.FindObjectOfType<LevelBounds>();
    }

    private void Update() {
        CalculateMovement();
    }

    private void CalculateMovement() {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0).normalized;
        StartSpeedBoost();

        Vector3 pos = transform.position + direction * speed * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, -3.6f, 0);
        pos.z = 0;

        WrapX(ref pos);

        transform.position = pos;
    }

    private void WrapX(ref Vector3 value) {
        if (value.x >= level.RightBound)
            value.x = level.LeftBound;
        else if (value.x <= level.LeftBound)
            value.x = level.RightBound;
    }

    private void StartSpeedBoost() {
        if (Speed <= 0)
            return;
        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && IsSpeedBoostAvailable)
            StartCoroutine(SpeedBoostCoroutine());
        else if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && !IsSpeedBoostAvailable) {
            speedBoostBar.NoSpeedBoostBarUIScaling();
            audio.clip = noSpeedBoostClip;
            audio.Play();
        }
    }

    private IEnumerator SpeedBoostCoroutine() {
        timeUsedSpeedBoost = Time.time;
        while (IsSpeedBoostActive) {
            speed = speedBoost;
            yield return null;
        }
        speed = baseSpeed;

        //NOTE: If we want to do something during/after cooldown, use this:
        //while (IsSpeedBoostCoolingDown)
        //    yield return null;
    }

    public void SpeedPowerUpActive() {
        isSpeedPowerUpActive = true;
        speedPowerUpImage.enabled = true;
        speedPowerDownImage.enabled = false;
        speed = baseSpeed;
        speed *= speedPowerUpMultiplier;
        thrusterSprite.color = Color.cyan;
        StartCoroutine(SpeedPowerUpShutDown());
    }

    private IEnumerator SpeedPowerUpShutDown() {
        WaitForSeconds wait = new WaitForSeconds(5);
        while (IsSpeedPowerUpActive) {
            yield return wait;
            speedPowerUpImage.enabled = false;
            speed /= speedPowerUpMultiplier;
            thrusterSprite.color = Color.white;
            isSpeedPowerUpActive = false;
        }
    }
    
    public void SpeedPowerDownActive() {
        isSpeedPowerDownActive = true;
        speedPowerDownImage.enabled = true;
        speedPowerUpImage.enabled = false;
        speed = speedPowerDownSpeed;
        thrusterSprite.color = Color.grey;
        StartCoroutine(SpeedPowerDownShutDown());
    }

    private IEnumerator SpeedPowerDownShutDown() {
        WaitForSeconds wait = new WaitForSeconds(5);
        while (IsSpeedPowerDownActive) {
            yield return wait;
            speedPowerDownImage.enabled = false;
            speed = 5;
            thrusterSprite.color = Color.white;
            isSpeedPowerDownActive = false;
        }
    }


}
