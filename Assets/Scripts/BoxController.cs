using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour {

    public float shrinkAmount = 0.2f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Bullet")
        {
            // destruir la bala al colisionar
            Destroy(collision.gameObject);

            transform.localScale = new Vector2(transform.localScale.x - shrinkAmount, transform.localScale.y - shrinkAmount);
            if(transform.localScale.x <= 0)
            {
                // si ha dismuido mucho destruir caja
                Destroy(gameObject);
            }
        }
    }
}
