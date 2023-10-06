using System;
using System.Collections;
using UnityEngine;

using Random = UnityEngine.Random; //NOTE: This is an alias, because System.Random also exists!


public class Enemy : MonoBehaviour {
    [System.Serializable]
    protected struct EnemyInfo {
        public GameObject laserPrefab;
        public EnemyController2D controller;

        public float fireRate;
        public float canFire;

        public EnemyInfo(GameObject laserPrefab, EnemyController2D enemyController, float fireRate, float canFire) {
            this.laserPrefab = laserPrefab;
            this.controller = enemyController;

            this.fireRate = fireRate;
            this.canFire = canFire;
        }
    }

    public static event Action onAnyDefeated;

    private HealthEntity playerHealth;
    private Animator anim;
    protected ShipMovementController2D playerController;
    private EnemyController2D enemyController;

    private new AudioSource audio;

    private Collider2D col2D;

    protected bool isAlive = true;

    protected void Start() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (playerHealth != null)
            playerHealth = player.GetComponent<HealthEntity>();

        if (playerController != null)
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<ShipMovementController2D>();
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        col2D = GetComponent<Collider2D>();
        enemyController = GetComponent<EnemyController2D>();
    }

    protected virtual void FireLaser() { }
    

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player")
            TouchDamageWithPlayer();
        else
            CheckToDefeatFromPlayer(other);
    }

    private void TouchDamageWithPlayer() {
        playerHealth.TryDamage(); //error
        anim.SetTrigger("OnEnemyDeath");
        enemyController.Speed = 0;
        audio.Play();

        col2D.enabled = false;
        isAlive = false;
        Destroy(this.gameObject, 3);
    }

    private bool CheckToDefeatFromPlayer(Collider2D other) {
        if ((other.TryGetComponent(out Laser laser) && !laser.IsEnemyLaser)
            || other.tag == "Wave") {
            if (laser != null)
                Destroy(laser.gameObject);

            anim.SetTrigger("OnEnemyDeath");
            enemyController.Speed = 0;
            audio.Play();

            col2D.enabled = false;
            isAlive = false;

            if (onAnyDefeated != null)
                onAnyDefeated();
            Destroy(this.gameObject, 3);
            return true;
        }
        return false;
    }
}