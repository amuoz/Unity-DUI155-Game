using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour {

    // health
    public int maxHealth;
    private int currentHealth;

    // Use this for initialization
    void Start () {
        currentHealth = maxHealth;
    }
	
	// Update is called once per frame
	void Update () {
		if(currentHealth <= 0)
        {
            Destroy(gameObject);
        }
	}

    public void HealthHandler()
    {
        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void HurtEnemy(int damage)
    {
        currentHealth -= damage;
    }

    public void SetMaxHealth()
    {
        currentHealth = maxHealth;
    }

}
