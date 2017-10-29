using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed = 1.5f;
    public float horizontalLimit = 2.8f;
    public float verticalLimit = 1.8f;

    // health
    public int maxHealth;
    private int currentHealth;

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
    private Vector2 lastMove;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;
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

            if (inputDirection.x != 0f)
            {
                playerMoving = true;
                lastMove = new Vector2(inputDirection.x, 0f);
                //inputDirection.x = CheckCollitionHorizontal(inputDirection) ? 0f : inputDirection.x;
            }

            if (inputDirection.y != 0f)
            {
                playerMoving = true;
                lastMove = new Vector2(0f, inputDirection.y);
                //inputDirection.y = CheckCollitionVertical(inputDirection) ? 0f : inputDirection.y;
            }

            if (playerMoving)
            {
                transform.Translate(inputDirection * speed * Time.deltaTime);
            }
        }
       
        AnimationHandler(inputDirection);

        // Evitar salirnos de escena
        CheckBoundaries();        
    }

    private bool CheckCollitionHorizontal(Vector2 inputDirection)
    {
        if (inputDirection.x != 0f)
        {
            Vector2 size = new Vector2(GetComponent<BoxCollider2D>().bounds.extents.x + 0.001f, GetComponent<BoxCollider2D>().bounds.extents.y + 0.001f);

            Vector2 oldPos = new Vector2(transform.position.x, transform.position.y);

            // calcular la colision horizontal
            bool isGoingRight = inputDirection.x > 0;
            Vector2 hRayDir = isGoingRight ? Vector2.right : Vector2.left;
            Vector2 hCorner1 = oldPos + new Vector2(hRayDir.x * size.x, (size.y - 0.02f));
            Vector2 hCorner2 = oldPos + new Vector2(hRayDir.x * size.x, -(size.y - 0.02f));
            bool col1 = Physics2D.Raycast(hCorner1, hRayDir.x * Vector2.right, 0.02f);
            bool col2 = Physics2D.Raycast(hCorner2, hRayDir.x * Vector2.right, 0.02f);

            Debug.DrawRay(hCorner1, hRayDir.x * Vector2.right * 0.1f, Color.red, 0.01f);
            Debug.DrawRay(hCorner2, hRayDir.x * Vector2.right * 0.1f, Color.red, 0.01f);

            return col1 || col2;
        }
        return false;
    }

    private bool CheckCollitionVertical(Vector2 inputDirection)
    {
        if (inputDirection.y != 0f)
        {
            Vector2 size = new Vector2(GetComponent<BoxCollider2D>().bounds.extents.x + 0.001f, GetComponent<BoxCollider2D>().bounds.extents.y + 0.001f);

            Vector2 oldPos = new Vector2(transform.position.x, transform.position.y);

            // calcular la colision vertical
            bool isGoingUp = inputDirection.y > 0;
            Vector2 vRayDir = isGoingUp ? Vector2.up : Vector2.down;
            Vector2 vCorner1 = oldPos + new Vector2((size.x - 0.02f), vRayDir.y * size.y);
            Vector2 vCorner2 = oldPos + new Vector2(-(size.x - 0.02f), vRayDir.y * size.y);

            bool col3 = Physics2D.Raycast(vCorner1, vRayDir.y * Vector2.up, 0.02f);
            bool col4 = Physics2D.Raycast(vCorner2, vRayDir.y * Vector2.up, 0.02f);

            Debug.DrawRay(vCorner1, vRayDir.y * Vector2.up * 0.1f, Color.red, 0.01f);
            Debug.DrawRay(vCorner2, vRayDir.y * Vector2.up * 0.1f, Color.red, 0.01f);

            return col3 || col4;
        }
        return false;
    }

    private void AnimationHandler(Vector2 inputDirection)
    {
        anim.SetFloat("MoveX", inputDirection.x);
        anim.SetFloat("MoveY", inputDirection.y);
        anim.SetBool("PlayerMoving", playerMoving);
        anim.SetFloat("LastMoveX", lastMove.x);
        anim.SetFloat("LastMoveY", lastMove.y);
        anim.SetBool("Attacking", attacking);
    }

    // Comprueba si hemos llegado al límite de la escena y reposiciona al personaje
    private void CheckBoundaries()
    {
        if (transform.position.x > horizontalLimit)
        {
            transform.position = new Vector2(horizontalLimit, transform.position.y);
        }
        else if (transform.position.x < -horizontalLimit)
        {
            transform.position = new Vector2(-horizontalLimit, transform.position.y);
        }

        if (transform.position.y > verticalLimit)
        {
            transform.position = new Vector2(transform.position.x, verticalLimit);
        }
        else if (transform.position.y < -verticalLimit)
        {
            transform.position = new Vector2(transform.position.x, -verticalLimit);
        }

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
            gameObject.SetActive(false);
        }
    }

    public void HurtPlayer(int damage)
    {
        currentHealth -= damage;
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
