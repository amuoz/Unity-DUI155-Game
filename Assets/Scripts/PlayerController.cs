using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    // move speed
    public float speed = 1.5f;
    private float currentMoveSpeed;

    // health
    public int maxHealth;
    public int currentHealth;

    // bullet properties
    public GameObject bulletPrefab;
    public float bulletSpeed;
    public float shootingCooldown;

    private float shootingTimer;
    private float bulletTimer = 5f;

    // melee attack
    private bool attacking;
    public float attackTime;
    private float attackTimeCounter;

    //Animator component
    private Animator anim;
    private Rigidbody2D myRigidBody;

    private bool playerMoving;

    private SFXController sfx;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();
        sfx = FindObjectOfType<SFXController>();

        currentHealth = maxHealth;

        currentMoveSpeed = speed;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        WalkHandler();
        MeleeHandler();
        ShootHandler();
        HealthHandler();
    }

    // Manejador de movimiento
    private void WalkHandler()
    {
        playerMoving = false;

        Vector2 inputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (!attacking)
        {
            //This will clamp how far up/down/left/right we can go in LOCAL space
            transform.position = new Vector2(Mathf.Clamp(transform.position.x, -29, 29), Mathf.Clamp(transform.position.y, -14, -5));

            if (inputDirection.x != 0f || inputDirection.y != 0f)
            {
                playerMoving = true;

            }

            //Checks to see which way our player is going and flips their facing direction
            if (inputDirection.x > 0)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            else if (inputDirection.x < 0)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            }

            if (playerMoving)
            {
                Vector2 movimiento = new Vector2(Mathf.Abs(inputDirection.x), inputDirection.y);
                transform.Translate(movimiento * currentMoveSpeed * Time.deltaTime);
            }
        }
       
        AnimationHandler(inputDirection);   
    }

    private void AnimationHandler(Vector2 inputDirection)
    {
        anim.SetBool("PlayerMoving", playerMoving);
        anim.SetBool("Attacking", attacking);
    }

    // Manejador de disparo
    private void ShootHandler()
    {
        // Input on X (Horizontal)
        float hAxis = Input.GetAxis("FireHorizontal");

        // Input on Y (Vertical)
        float vAxis = Input.GetAxis("FireVertical");

        // decrementar el timer acorde a deltaTime
        shootingTimer -= Time.deltaTime;

        // comprobamos primero si podemos disparar
        if (shootingTimer <= 0)
        {
            if (hAxis != 0f || vAxis != 0f)
            {
                shootingTimer = shootingCooldown;

                GameObject bulletInstance = Instantiate(bulletPrefab);
                bulletInstance.transform.SetParent(transform.parent);
                bulletInstance.transform.position = transform.position;

                bulletInstance.GetComponent<Rigidbody2D>().velocity = new Vector2(hAxis * bulletSpeed, vAxis * bulletSpeed);

                Destroy(bulletInstance, bulletTimer);
            }
        }
    }

    public void HealthHandler()
    {
        if(currentHealth <= 0)
        {
            sfx.playerDead.Play();
            gameObject.SetActive(false);     
        }
    }

    public void HurtPlayer(int damage)
    {
        currentHealth -= damage;

        // añadir efecto de flash

        sfx.playerHurt.Play();
    }

    public void SetMaxHealth()
    {
        currentHealth = maxHealth;
    }

    private void MeleeHandler()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            attackTimeCounter = attackTime;
            attacking = true;
            myRigidBody.velocity = Vector2.zero;

            sfx.playerAttack.Play();
        }

        if(attackTimeCounter >= 0)
        {
            attackTimeCounter -= Time.deltaTime;
        }

        if(attackTimeCounter <= 0)
        {
            attacking = false;
            anim.SetBool("Attacking", attacking);
        }
    }

}
