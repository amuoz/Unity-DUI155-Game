using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtPlayerController : MonoBehaviour {

    public int damageTogive;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().HurtPlayer(damageTogive);
        }
    }

}
