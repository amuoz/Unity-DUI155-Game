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

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        GetComponent<Rigidbody2D>().velocity = new Vector2(Input.GetAxis("Horizontal") * speed, 0);
        if (transform.position.x > horizontalLimit)
        {
            transform.position = new Vector2(horizontalLimit, transform.position.y);
        }
        else if (transform.position.x < -horizontalLimit)
        {
            transform.position = new Vector2(-horizontalLimit, transform.position.y);
        }

        shootingTimer -= Time.deltaTime;
        // comprobamos primero si podemos disparar
        if(shootingTimer <= 0)
        {
            if (Input.GetAxis("Fire1") == 1f)
            {
                shootingTimer = shootingCooldown;

                GameObject bulletInstance = Instantiate(bulletPrefab);
                bulletInstance.transform.SetParent(transform.parent);
                bulletInstance.transform.position = transform.position;
                bulletInstance.GetComponent<Rigidbody2D>().velocity = new Vector2(0, bulletSpeed);
                Destroy(bulletInstance, 5f);
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
