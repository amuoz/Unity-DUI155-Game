using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed = 1.5f;
    public float horizontalLimit = 2.8f;

    // bullet properties
    public GameObject bulletPrefab;
    public float bulletSpeed;
    public float shootingCooldown;

    private float shootingTimer;
    private float bulletTimer = 5f;

    //Animator component
    Animator anim;
    //Rigidbody component
    Rigidbody2D rb;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        WalkHandler();
        ShootHandler();
    }

    // Manejador de movimiento
    private void WalkHandler()
    {
        // Input on X (Horizontal)
        float hAxis = Input.GetAxis("Horizontal");

        // Input on Y (Vertical)
        float vAxis = Input.GetAxis("Vertical");

        AnimationHandler(hAxis, vAxis);

        // Actualizar la velocidad del personaje
        UpdateSpeed(hAxis, vAxis);
        
        // Evitar salirnos de escena
        CheckBoundaries();
    }

    /* 
     * Animation Control
     * hAxis == 0 Iddle
     * hAxis > 0 Right
     * hAxis < 0 Left
     */
    private void AnimationHandler(float hAxis, float vAxis)
    {
        if (hAxis == 0 && vAxis == 0)
        {
            anim.SetInteger("State", 0);
        }
        else if (vAxis > 0)
        {
            anim.SetInteger("State", 3);
        }
        else if (vAxis < 0)
        {
            anim.SetInteger("State", 4);
        }
        else if (hAxis < 0)
        {
            anim.SetInteger("State", 1);
        }
        else if (hAxis > 0)
        {
            anim.SetInteger("State", 2);
        }
    }

    private void UpdateSpeed(float hAxis, float vAxis)
    {
        rb.velocity = new Vector2(hAxis * speed, vAxis * speed);
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
    }

    // Manejador de disparo
    private void ShootHandler()
    {
        // decrementar el timer acorde a deltaTime
        shootingTimer -= Time.deltaTime;
        
        // comprobamos primero si podemos disparar
        if (shootingTimer <= 0)
        {
            if (Input.GetAxis("Fire1") == 1f)
            {
                shootingTimer = shootingCooldown;

                GameObject bulletInstance = Instantiate(bulletPrefab);
                bulletInstance.transform.SetParent(transform.parent);
                bulletInstance.transform.position = transform.position;
                bulletInstance.GetComponent<Rigidbody2D>().velocity = new Vector2(0, bulletSpeed);
                Destroy(bulletInstance, bulletTimer);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Monster")
        {
            Destroy(gameObject);
        }
    }

}
