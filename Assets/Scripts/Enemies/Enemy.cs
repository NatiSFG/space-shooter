using System;
using UnityEngine;

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

    protected HealthEntity playerHealth;
    private Animator anim;
    protected ShipMovementController2D playerController;
    protected EnemyController2D enemyController;

    protected new AudioSource audio;

    protected Collider2D col2D;

    protected bool isAlive = true;

    protected virtual void Start() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) {
            playerHealth = player.GetComponent<HealthEntity>();
            playerController = player.GetComponent<ShipMovementController2D>();
        }

        if (GetComponent<Animator>() != null)
            anim = GetComponent<Animator>();
        else if (GetComponentInChildren<Animator>() != null)
            anim = GetComponentInChildren<Animator>();
        audio = GetComponent<AudioSource>();
        col2D = GetComponent<Collider2D>();
        enemyController = GetComponent<EnemyController2D>();
    }

    protected virtual void FireLaser() { }
    
    private void OnTriggerEnter2D(Collider2D other) {
        //if the player collides
        if (other.tag == "Player") {
            //and the enemy has a shield, try to damage the shield
            if (TryGetComponent(out EnemyShield shield) && shield.TryDamageShield()) {
                //if the player has a shield, damage it too
                if (!playerHealth.TryDamageShield())
                    playerHealth.TryDamage();
            //if the enemy doesn't have a shield, potentially damage player and kill enemy
            } else TouchDamageWithPlayer();
        }

        if (other.tag == "Player Laser" || other.tag == "Wave") {
            //if the enemy has a shield and try to damage the shield, then there's nothing to do
            if (TryGetComponent(out EnemyShield shield) && shield.TryDamageShield()
                && other.GetComponent<Laser>() != null) {
                Destroy(other.gameObject);
                return;
            }
            if (other.GetComponent<Laser>() != null) {
                Destroy(other.gameObject);
                //if the enemy doesn't have a shield, kill the enemy
                Defeat();
            }
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