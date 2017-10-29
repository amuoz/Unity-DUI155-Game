using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterController : MonoBehaviour {

    public delegate void KillHandler(MonsterController monster);
    public event KillHandler OnKill;

    public float speed = 2f;
    public float horizontalLimit = 2.8f;
    public int tileSize = 32;

    public MonsterController next;

    private float movingDirection = 1f;

    public float waitToReload;
    private bool reloading;

	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody2D>().velocity = new Vector2(speed * movingDirection, 0);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (movingDirection > 0 && transform.position.x > horizontalLimit)
        {
            MoveDown();
        }
        else if (movingDirection < 0 && transform.position.x < -horizontalLimit)
        {
            MoveDown();
        }

        if (reloading)
        {
            waitToReload -= Time.deltaTime;
            if (waitToReload < 0)
            {
                SceneManager.LoadScene("Game");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Box")
        {
            MoveDown();
        }
        else if (collision.tag == "Bullet")
        {
            Destroy(collision.gameObject);
            if (OnKill != null)
            {
                OnKill(this);
            }
        }
        else if (collision.tag == "Player")
        {
            //Destroy(collision.gameObject);
            collision.gameObject.SetActive(false);
            reloading = true;
        }
    }
    
    void MoveDown()
    {
        // cuando llega al limite cambiamos direccion
        movingDirection *= -1;
        transform.position = new Vector2(transform.position.x, transform.position.y - tileSize / 100f);
        GetComponent<Rigidbody2D>().velocity = new Vector2(speed * movingDirection, 0);
    }

    public void ChangeDirection()
    {
        movingDirection *= -1;
        GetComponent<Rigidbody2D>().velocity = new Vector2(speed * movingDirection, 0);
    }
}
