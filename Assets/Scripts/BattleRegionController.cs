using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRegionController : MonoBehaviour {

    private float regionHeight;
    private float regionWidth;

    private float leftEdge;
    private float rightEdge;
    private float topEdge;
    private float bottomEdge;

    private bool active;

    private bool hasEnemies;

    public BoxCollider2D region;

    // Use this for initialization
    void Start () {
        region = GetComponent<BoxCollider2D>();

        regionHeight = region.size.y;
        regionWidth = region.size.x;

        leftEdge = transform.position.x - regionWidth / 2;
        rightEdge = transform.position.x + regionWidth / 2;
        topEdge = transform.position.y + regionHeight / 2;
        rightEdge = transform.position.y - regionHeight / 2;

        active = false;
        hasEnemies = false;
	}
	
	// Update is called once per frame
	void Update () {
        EnemyController enemy = FindObjectOfType<EnemyController>();
        // comprobar que hay enemigos dentro de la region
        //if (enemy != null && region.bounds.Contains(enemy.transform.position))
        if (enemy != null)
        {
            hasEnemies = true;
        }
        else
        {
            hasEnemies = false;
            active = false;
        }
	}

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            // check si hay enemigos
            if (hasEnemies && !active && region.bounds.Contains(other.transform.position))
            {
                Debug.Log("Player entra en battle region");
                active = true;
                PlayerController player = FindObjectOfType<PlayerController>();
                player.setBattleRegion(this);
            }
            
        }
    }

    public bool isActive()
    {
        return active;
    }
}
