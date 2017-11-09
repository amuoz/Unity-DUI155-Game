using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    public float speed;
    public float currentSpeed;

    // health
    public int maxHealth;
    public int currentHealth;

    // melee attack
    private bool attacking;
    public float attackTime;
    private float attackTimeCounter;

    // we got hit
    private bool hit;

    // knockback
    private bool onGround;
    private float groundY;

    private bool playerMoving;

    //Animator component
    private Animator anim;
    private Rigidbody2D myRigidBody;
    private SpriteRenderer sprite;
    private SFXController sfx;

    private BattleRegionController battleRegion;
    private CameraController camara;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();
        sfx = FindObjectOfType<SFXController>();
        sprite = GetComponent<SpriteRenderer>();
        camara = FindObjectOfType<CameraController>();


        currentHealth = maxHealth;
        currentSpeed = speed;

        onGround = true;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        WalkHandler();
        MeleeHandler();
        AnimationHandler();
    }

    // Manejador de movimiento
    private void WalkHandler()
    {
        playerMoving = false;

        Vector2 inputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (!attacking && !hit)
        {
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

            //If the player is on the ground move them with XSpeed and YSpeed, otherwise ignore YSpeed
            if (!onGround)
            {
                inputDirection.y = 0f;
            }

            if (playerMoving)
            {
                // al hacer flip sobre la transform original del player nos quedamos con el valor absoluto de X
                Vector2 movimiento = new Vector2(Mathf.Abs(inputDirection.x), inputDirection.y);

                // restringir movimiento a la region de batalla si esta activa
                /*
                if (battleRegion != null && battleRegion.isActive())
                {
                    Vector3 nextMove = new Vector3(inputDirection.x, inputDirection.y, 0);
                    if (!battleRegion.region.bounds.Contains(transform.position + nextMove))
                    {
                        movimiento = Vector2.zero;
                    }
                }
                */

                // restringir movimiento segun la camara
                Vector3 nextMove = new Vector3(inputDirection.x, inputDirection.y, 0) + transform.position;
                if (nextMove.x <= camara.leftEdge 
                    || nextMove.x >= camara.rightEdge 
                    || nextMove.y <= camara.bottomEdge 
                    || nextMove.y >= camara.topEdge)
                {
                    movimiento = Vector2.zero;
                }
                
                transform.Translate(movimiento.normalized * currentSpeed * Time.deltaTime);
                // depth of the sprite segun valor de Y
                sprite.sortingOrder = -1* (int) transform.position.y;
            }
            
            //This will clamp how far up/down/left/right we can go in LOCAL space
            transform.position = new Vector2(transform.position.x, Mathf.Clamp(transform.position.y, -14, -6));
        }
        
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

        if (attackTimeCounter >= 0)
        {
            attackTimeCounter -= Time.deltaTime;
        }

        if (attackTimeCounter <= 0)
        {
            attacking = false;
            anim.SetBool("Attacking", attacking);
        }
    }

    private void AnimationHandler()
    {
        anim.SetBool("PlayerMoving", playerMoving);
        anim.SetBool("Attacking", attacking);
    }

    public void HurtPlayer(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            sfx.playerDead.Play();
        }
        else
        {
            sfx.playerHurt.Play();
        }
    }

    public bool isAttacking()
    {
        return attacking;
    }

    public BattleRegionController getBattleRegion()
    {
        return battleRegion;
    }

    public void setBattleRegion(BattleRegionController region)
    {
        battleRegion = region;
    }

}
