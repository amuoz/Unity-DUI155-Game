using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour {

    public float speed;
    // modificador de velocidad
    private float speedMod;

    private Rigidbody2D myRigidBody;

    public float timeBetweenMove;
    private float timeBetweenMoveCounter;
    public float timeToMove;
    private float timeToMoveCounter;

    private bool moving;

    private Vector2 moveDirection;
    private SpriteRenderer sprite;
    private Animator anim;

    public float waitToReload;

    // health
    public int maxHealth;
    private int currentHealth;

    private bool attacking;
    private bool hit;
    private float stunTimeCounter;

    // knockback
    private bool onGround;
    private float groundY;

    // Use this for initialization
    void Start () {
        myRigidBody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        timeBetweenMoveCounter = Random.Range(timeBetweenMove * 0.75f, timeBetweenMove * 1.25f);
        timeToMoveCounter = Random.Range(timeToMove * 0.75f, timeToMove * 1.25f);

        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        WalkHandler();
        HealthHandler();
    }

    private void WalkHandler()
    {
        if(!attacking && !hit)
        {
            if (moving)
            {
                //This will clamp how far up/down/left/right we can go in LOCAL space
                transform.position = new Vector2(transform.position.x, Mathf.Clamp(transform.position.y, -15, -6));

                timeToMoveCounter -= Time.deltaTime;
                //myRigidBody.velocity = moveDirection;
                transform.Translate(moveDirection * speed * Time.deltaTime);

                // depth of the sprite segun valor de Y
                sprite.sortingOrder = -1 * (int)transform.position.y;

                if (timeToMoveCounter < 0f)
                {
                    moving = false;
                    //timeBetweenMoveCounter = timeBetweenMove;
                    timeBetweenMoveCounter = Random.Range(timeBetweenMove * 0.75f, timeBetweenMove * 1.25f);
                }
            }
            else
            {
                myRigidBody.velocity = Vector2.zero;
                timeBetweenMoveCounter -= Time.deltaTime;

                if (timeBetweenMoveCounter < 0f)
                {
                    moving = true;
                    //timeToMoveCounter = timeToMove;
                    timeToMoveCounter = Random.Range(timeToMove * 0.75f, timeToMove * 1.25f);

                    moveDirection = new Vector2(Random.Range(-0.5f, 0.5f) * speed, Random.Range(-0.5f, 0.5f) * speed);
                }
            }
        }

        if (hit)
        {
            stunTimeCounter -= Time.deltaTime;
            if (stunTimeCounter <= 0)
            {
                hit = false;
            }
        }
        
    }

    private void HealthHandler()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void HurtEnemy(int damage, float stunTime)
    {
        currentHealth -= damage;

        hit = true;
        stunTimeCounter = stunTime;
        anim.SetTrigger("enemyHit");
    }

    public int getDepth()
    {
        return sprite.sortingOrder;
    }

}