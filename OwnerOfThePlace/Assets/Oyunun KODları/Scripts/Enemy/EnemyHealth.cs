using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    int maxEnemyHealth = 100;
    public float currentEnemyHealth;
    internal bool gotDamage;
    public float damage;
    public float projectileDamage = 25;
    CircleCollider2D circle2D;
    Player player;
    Rigidbody2D body2D;

    //Audio
    AudioSource auSource;
    AudioClip ac_Dead;
    void Start()
    {
        currentEnemyHealth = maxEnemyHealth;
        player = FindObjectOfType<Player>();
        circle2D = GetComponent<CircleCollider2D>();
        body2D = GetComponent<Rigidbody2D>();
        auSource.GetComponent<AudioSource>();
        ac_Dead = Resources.Load("SoundEffects/EnemyDead") as AudioClip;
    }


    void Update()
    {
        if (currentEnemyHealth <= 0)
        {
            circle2D.enabled = false;
            body2D.constraints = RigidbodyConstraints2D.FreezePositionX;
            Destroy(gameObject, 1);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerItem")
        {
            currentEnemyHealth -= damage;
        }

        if(other.tag== "PlayerProjectile")
        {
            currentEnemyHealth -= projectileDamage;
            auSource.PlayOneShot(ac_Dead);
            Destroy(other.gameObject);
        }
    }
}