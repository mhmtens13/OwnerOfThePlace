using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemyController : MonoBehaviour
{
    Rigidbody2D enemyBody2D;
  //  public float enemySpeed;

    public float turnDelay;
    bool forWalk = true;
    public Animator enemyAnimator;
    public int m_speed;
    bool facingright = false;

    //Yeri Bulma
    //bool isGrounded;
    //Transform groundCheck;
    //const float GroundCheckRadius = 0.2f;
    //public LayerMask groundLayer;
    //public bool moveRight;


    void Start()
    {
        enemyBody2D = GetComponent<Rigidbody2D>();
      //  groundCheck = transform.Find("GroundCheck");

        m_speed = 4;
        turnDelay = 3f;
        StartCoroutine(yondegis());
        enemyAnimator = GetComponent<Animator>();
        forWalk = true;
    }
    
    void Update()
    {
        //Duvara değiyoruz muyuz diye bak
        //isGrounded = Physics2D.OverlapCircle(groundCheck.position, GroundCheckRadius, groundLayer);

        //if (isGrounded)
        //    moveRight = !moveRight;

        //enemyBody2D.velocity = (moveRight) ? new Vector2(enemySpeed, 0) : new Vector2(-enemySpeed, 0);
        //transform.localScale = (moveRight) ? new Vector2(-1, 1) : new Vector2(1, 1);

        transform.Translate(Vector3.right * m_speed * Time.deltaTime);
        enemyAnimator.SetBool("walking", forWalk);

    }

    IEnumerator yondegis()
    {
        yield return new WaitForSeconds(turnDelay);
        donbaba();
    }

    void donbaba()
    {
        facingright = !facingright;
        Vector2 eksiyonscale = transform.localScale;
        eksiyonscale.x *= -1;
        transform.localScale = eksiyonscale;
        m_speed *= -1;
        StartCoroutine(yondegis());
    }

}