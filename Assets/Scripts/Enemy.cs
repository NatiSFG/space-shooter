using System;
using UnityEngine;

using Random = UnityEngine.Random; //NOTE: This is an alias, because System.Random also exists!

public class Enemy : MonoBehaviour {
    public static event Action onAnyDefeated;

    [SerializeField] private GameObject laserPrefab;

    private HealthEntity playerHealth;
    private Animator anim;
    private EnemyController2D enemyController;

    private new AudioSource audio;
    private float fireRate = 3.0f;
    private float canFire = -1;

    private Collider2D col2D;

    private bool isAlive = true;

    private void Start() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<HealthEntity>();
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        col2D = GetComponent<Collider2D>();
        enemyController = GetComponent<EnemyController2D>();
    }

    void Update() {
        FireLaser();
    }

    private void FireLaser() {
        if (Time.time > canFire && isAlive) {
            fireRate = Random.Range(3, 7);
            canFire = Time.time + fireRate;
            GameObject enemyLaser = Instantiate (laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            for (int i = 0; i < lasers.Length; i++)
                lasers[i].AssignEnemyLaser();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player")
            TouchDamageWithPlayer();
        else
            CheckToDefeatFromPlayer(other);
    }

    private void TouchDamageWithPlayer() {
        playerHealth.TryDamage();
        anim.SetTrigger("OnEnemyDeath");
        enemyController.Speed = 0;
        audio.Play();

        col2D.enabled = false;
        isAlive = false;
        Destroy(this.gameObject, 3);
    }

    private bool CheckToDefeatFromPlayer(Collider2D other) {
        if ((other.TryGetComponent(out Laser laser) && !laser.IsEnemyLaser())
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