using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtEnemyController : MonoBehaviour {

    public int damageToGive;

    public GameObject damageBurst;
    public GameObject damageNumber;
    public Transform hitPoint;

    // maximum vertical distance to allow before two objects can no longer interact
    public int LayerSize;

    // time the enemy will be stunned for hit variable
    public float stunTime;

    // allows us to say when the attack will deal damage
    public int DMGFrame;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            int depth = -1 * (int) transform.position.y;
            EnemyController enemyController = other.gameObject.GetComponent<EnemyController>();
            PlayerController playerController = FindObjectOfType<PlayerController>();

            if (Mathf.Abs(depth - enemyController.getDepth()) <= LayerSize && playerController.isAttacking())
            {
                enemyController.HurtEnemy(damageToGive, stunTime);

                Instantiate(damageBurst, hitPoint.position, hitPoint.rotation);
                var clone = (GameObject)Instantiate(damageNumber, hitPoint.position, Quaternion.Euler(Vector3.zero));
                clone.GetComponent<FloatingNumbersController>().damageNumber = damageToGive;  
            }
            
        }
    }

}
