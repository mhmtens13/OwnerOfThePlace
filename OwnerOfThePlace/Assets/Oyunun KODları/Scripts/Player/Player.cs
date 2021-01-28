using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{

    Rigidbody2D body2D;
    public float knockBackForce;
    GameManager gameManager;

    //Collider's
    BoxCollider2D box2D;
    CircleCollider2D circle2D;

    [Range(0, 100)]
    public float playerSpeed;

    //Zıplama işlemleri
    public float jumpPower;

    public float doubleJumpPower;
    internal bool canDoubleJump;

    //Yeri bulma
    [Tooltip("Karakterin yere değip değmediğini kontrol eder")]
    public bool isGrounded;
    Transform groundCheck;
    const float GroundCheckRadius = 0.2f;
    [Tooltip("Yerin ne olduğunu belirler")]
    public LayerMask groundLayer;

    //Karakteri döndürme
    bool facingRight = true;

    //Animator Controller, animasyonları kontrol eder
    Animator playerAnimController;

    //Karakterin canı
    internal int maxPlayerHealth = 100;
    public int currentPlayerHealth;
    internal bool isHurt;

    //Oyuncuyu öldür
    internal bool isDead;
    public float deadForce;

    //Oyuncunun puanları
    internal int currentPoints;
    internal int point = 10;

    //CheckPoint
    public GameObject startPos;
    GameObject checkPoint;

    //Sound
    AudioSource auSource;
    AudioClip ac_Jump;
    AudioClip ac_Hurt;
    AudioClip ac_Coin;
    AudioClip ac_Dead;
    AudioClip ac_Shoot;

    //Ateş Etme
    Transform firePoint;
    GameObject bullet;

//    public GameObject[] heart;


    void Start()
    {
        transform.position = startPos.transform.position;

        body2D = GetComponent<Rigidbody2D>();

        //Collider'ları al
        box2D = GetComponent<BoxCollider2D>();
        circle2D = GetComponent<CircleCollider2D>();

        //GroundCheck'i bul
        groundCheck = transform.Find("GroundCheck");

        //Animator'u bul
        playerAnimController = GetComponent<Animator>();

        //Canı, maxCana eşitle

        currentPlayerHealth = maxPlayerHealth;
    //    HearthSystem();

        //GameManager
        gameManager = FindObjectOfType<GameManager>();

        //Sesleri yükleme
        auSource = GetComponent<AudioSource>();
        ac_Jump = Resources.Load("SoundEffects/PowerUp") as AudioClip;
        ac_Hurt = Resources.Load("SoundEffects/Hurt") as AudioClip;
        ac_Coin = Resources.Load("SoundEffects/PickupCoin") as AudioClip;
        ac_Shoot = Resources.Load("SoundEffects/Shoot") as AudioClip;

        //Ateş Etme
        firePoint = transform.Find("FirePoint");
        bullet = Resources.Load("Bullet") as GameObject;

    }

    void Update()
    {
        UpdateAnimations();
        ReduceHealth();
        isDead = currentPlayerHealth <= 0;
        if (isDead)
            KillPlayer();

        if (transform.position.y <= -15)
            isDead = true;

        //Eğer canımız, maxCandan yüksekse canımızı maxCana eşitle
        if (currentPlayerHealth > maxPlayerHealth)
            currentPlayerHealth = maxPlayerHealth;
    }

    //Framerate'den bağımsız olarak çalışır, Fizik ile ilgili kurallar buraya yazılır
    void FixedUpdate()
    {
        //Yere değiyoruz muyuz diye bak
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, GroundCheckRadius, groundLayer);

       // Hareket etme
        //float h = Input.GetAxis("Horizontal");
        //body2D.velocity = new Vector2(h * playerSpeed, body2D.velocity.y);
        //Flip(h);
    }

    public void Move(bool right)
    {
        if (right)
        {
            body2D.velocity = new Vector2(playerSpeed, body2D.velocity.y);
            Flip(1);
        }
        else
        {
            body2D.velocity = new Vector2(-playerSpeed, body2D.velocity.y);
            Flip(-1);
        }
    }

    public void ZeroVelocity()
    {
        body2D.velocity = Vector2.zero;
    }

    public void Jump()
    {

        //MOBILE 
        if (isGrounded)
        {
            //Rigidbody'e dikey yönde(y) güç ekler  
            body2D.AddForce(new Vector2(0, jumpPower));
            // body2D.velocity = new Vector2(0, jumpPower);
            auSource.PlayOneShot(ac_Jump);
            auSource.pitch = Random.Range(0.8f, 1.1f);
            canDoubleJump = true;
        }
        else DoubleJump();

        ////Rigidbody'e dikey yönde(y) güç ekler  
        //body2D.AddForce(new Vector2(0, jumpPower));
        //// body2D.velocity = new Vector2(0, jumpPower);
        //auSource.PlayOneShot(ac_Jump);
        //auSource.pitch = Random.Range(0.8f, 1.1f);
    }

    public void DoubleJump()
    {

        //MOBILE
        if (!isGrounded && canDoubleJump)
        {
            //Rigidbody'e dikey yönde(y) ani bir güç ekler  
            body2D.AddForce(new Vector2(0, doubleJumpPower), ForceMode2D.Impulse);
            auSource.PlayOneShot(ac_Jump);
            auSource.pitch = Random.Range(0.8f, 1.1f);
            canDoubleJump = false;
        }

        ////Rigidbody'e dikey yönde(y) ani bir güç ekler  
        //body2D.AddForce(new Vector2(0, doubleJumpPower), ForceMode2D.Impulse);
        //auSource.PlayOneShot(ac_Jump);
        //auSource.pitch = Random.Range(0.8f, 1.1f);
    }

    //Karakteri döndürme fonksiyonu
    void Flip(float h)
    {
        if (h > 0 && !facingRight || h < 0 && facingRight)
        {
            facingRight = !facingRight;
            Vector2 theScale = transform.localScale;
            theScale.x *= -1;
                transform.localScale = theScale;
            }
    }

    //Animator'u yenileme fonksiyonu
    void UpdateAnimations()
    {
        playerAnimController.SetFloat("VelocityX", Mathf.Abs(body2D.velocity.x));
        playerAnimController.SetBool("isGrounded", isGrounded);
        playerAnimController.SetFloat("VelocityY", body2D.velocity.y);
        playerAnimController.SetBool("isDead", isDead);

        if (isHurt && !isDead)
            playerAnimController.SetTrigger("isHurt");
    }

    void ReduceHealth()
    {
        if (isHurt)
        {
            /*Eğer canımız         o zaman canımızdan zarar kadar çıkar
            100 ise                     -zarar-
            eğer bu kondisyon doğru ise can-zarar=yeni can  
            */

            isHurt = false;

            //Eger havadaysak sol veya sag ve dikey yonde guc uygula
            if (facingRight && !isGrounded)
                body2D.AddForce(new Vector2(-knockBackForce, 1000), ForceMode2D.Force);
            else if (!facingRight && !isGrounded)
                body2D.AddForce(new Vector2(knockBackForce, 1000), ForceMode2D.Force);

            //Eger yerdeysek sol veya sag yonde guc uygula
            if (facingRight && isGrounded)
                body2D.AddForce(new Vector2(-knockBackForce, 0), ForceMode2D.Force);
            else if (!facingRight && isGrounded)
                body2D.AddForce(new Vector2(knockBackForce, 0), ForceMode2D.Force);

            if (!isDead)
            {
                auSource.PlayOneShot(ac_Hurt);
                auSource.pitch = Random.Range(0.8f, 1.1f);
            }

        }
    }

    //Oyuncuyu öldürme fonksiyonu
    void KillPlayer()
    {
        if (isDead)
        {
            isHurt = false;
            body2D.AddForce(new Vector2(0, deadForce), ForceMode2D.Impulse);
            body2D.drag += Time.deltaTime * 8;
            deadForce -= Time.deltaTime * 15;
            body2D.constraints = RigidbodyConstraints2D.FreezePositionX;
            box2D.enabled = false;
            circle2D.enabled = false;
        }
    }

    public void ShootProjectile()
    {
        GameObject b = Instantiate(bullet) as GameObject;
        b.transform.position = firePoint.transform.position;
        b.transform.rotation = firePoint.transform.rotation;

        auSource.PlayOneShot(ac_Shoot);
        auSource.pitch = Random.Range(0.8f, 1.1f);

        if (transform.localScale.x < 0)
        {
            b.GetComponent<Projectile>().bulletSpeed *= -1;
            b.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            b.GetComponent<Projectile>().bulletSpeed *= 1;
            b.GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    public void RecoverPlayer()
    {
        if (checkPoint != null)
            transform.position = checkPoint.transform.position;
        else
            transform.position = startPos.transform.position;

        deadForce = 5;
        body2D.gravityScale = 5;
        body2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        currentPlayerHealth = maxPlayerHealth;
        box2D.enabled = true;
        circle2D.enabled = true;
        body2D.constraints = RigidbodyConstraints2D.None;
        body2D.freezeRotation = true;
        body2D.simulated = true;
        body2D.drag = 0;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Coin")
        {
            currentPoints += point;
            other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            Destroy(other.gameObject);
            auSource.PlayOneShot(ac_Coin);
            auSource.pitch = Random.Range(0.8f, 1.1f);
        }

        if (other.tag == "CheckPoint")
        {
            checkPoint = other.gameObject;
        }

        if (other.tag == "Enemy" && isDead)
        {
            auSource.PlayOneShot(ac_Dead);
        }
    }

//    void HearthSystem()
//    {
//        for (int i = 0; i < maxPlayerHealth; i++)
//        {
//            heart[i].SetActive(false);
//        }

//        for (int i = 0; i < currentPlayerHealth; i++)
//        {
//            heart[i].SetActive(true);
//        }
//    }
}