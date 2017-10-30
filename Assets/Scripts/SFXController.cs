using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXController : MonoBehaviour {

    private static bool SFXControllerExists;

    public AudioSource playerHurt;
    public AudioSource playerDead;
    public AudioSource playerAttack;

    // Use this for initialization
    void Start () {
        if (!SFXControllerExists)
        {
            SFXControllerExists = true;
            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
