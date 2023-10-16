using System;
using System.Collections;
using System.IO.MemoryMappedFiles;
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
        if (player != null) {
            playerHealth = player.GetComponent<HealthEntity>();
            playerController = player.GetComponent<ShipMovementController2D>();
        }

        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        col2D = GetComponent<Collider2D>();
        enemyController = GetComponent<EnemyController2D>();
    }

    protected virtual void FireLaser() { }
    
    private bool PlayerLaserOrWaveDoesHarm(Collider2D other, out Laser laser) {
        return (other.TryGetComponent(out laser) && !laser.IsEnemyLaser)
           || other.tag == "Wave";
    }

    private void OnTriggerEnter2D(Collider2D other) {
        //if the player collides
        if (other.tag == "Player") {
            //and the enemy has a shield, try to damage the shield
            if (TryGetComponent(out EnemyShield shield) && shield.TryDamageShield()) {
            //if the enemy doesn't have a shield, potentially damage player and kill enemy
            } else TouchDamageWithPlayer();
        }

        //when calling this as a parameter, it promises to initialize this parameter
        //and send it back to the called by reference
        if (PlayerLaserOrWaveDoesHarm(other, out Laser laser)) {
            if (laser != null)
                Destroy(laser.gameObject);
            //if the enemy has a shield and try to damage the shield, then there's nothing to do
            if (TryGetComponent(out EnemyShield shield) && shield.TryDamageShield()) {
            //if the enemy doesn'y have a shield, kill the enemy
            } else Defeat();
            
        }
    }

    //player potentially gets damaged and enemy dies when colliding with player
    private void TouchDamageWithPlayer() {
        playerHealth.TryDamage();
        anim.SetTrigger("OnEnemyDeath");
        enemyController.Speed = 0;
        audio.Play();

        col2D.enabled = false;
        isAlive = false;
        Destroy(this.gameObject, 3);
    }

    public void Defeat() {
        anim.SetTrigger("OnEnemyDeath");
        enemyController.Speed = 0;
        audio.Play();

        col2D.enabled = false;
        isAlive = false;

        if (onAnyDefeated != null)
            onAnyDefeated();
        Destroy(this.gameObject, 3);
    }
}