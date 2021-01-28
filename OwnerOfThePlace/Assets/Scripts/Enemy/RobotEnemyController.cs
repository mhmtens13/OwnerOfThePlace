﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotEnemyController : MonoBehaviour
{
    Rigidbody2D enemyBody2D;
    public float enemySpeed;
    EnemyHealth enemyHealth;
    Animator robotEnemyAnim;

    //Yeri Bulma
    bool isGrounded;
    Transform groundCheck;
    const float GroundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public bool moveRight;

    //Uçurumu bulma
    bool onEdge;
    Transform edgeCheck;

    //Ses
    AudioSource au_Source;
    AudioClip ac_Dead;

    void Start()
    {
        enemyBody2D = GetComponent<Rigidbody2D>();
        groundCheck = transform.Find("GroundCheck");
        edgeCheck = transform.Find("EdgeCheck");
        robotEnemyAnim = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
        ac_Dead = Resources.Load("SoundEffects/EnemyDead") as AudioClip;
    }

    void Update()
    {
        //Duvara değiyoruz muyuz diye bak
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, GroundCheckRadius, groundLayer);
        onEdge = Physics2D.OverlapCircle(edgeCheck.position, GroundCheckRadius, groundLayer);

        if (isGrounded || !onEdge)
            moveRight = !moveRight;

        enemyBody2D.velocity = (moveRight) ? new Vector2(enemySpeed, 0) : new Vector2(-enemySpeed, 0);
        transform.localScale = (moveRight) ? new Vector2(-1, 1) : new Vector2(1, 1);


    }
}
