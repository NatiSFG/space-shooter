using System;
using System.Collections;
using UnityEngine;

public class DodgerEnemy : Enemy {
    private Animator anim;

    protected override void Start() {
        base.Start();
        anim = GetComponentInChildren<Animator>();
    }
}
