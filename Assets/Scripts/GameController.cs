using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    // referencia al jugador
    private PlayerController player;


    // scene reload time
    public float timeToReload;
    private float timeToReloadCounter;
    private bool reloading;

    // Use this for initialization
    void Start () {
        player = FindObjectOfType<PlayerController>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        reloadScene();
    }

    public void reloadScene()
    {
        if (!reloading)
        {
            if (player.currentHealth <= 0)
            {
                // wait a few seconds to reload scene
                timeToReloadCounter = timeToReload;
                reloading = true;
                player.gameObject.SetActive(false);
            }
        }
        else
        {
            timeToReloadCounter -= Time.deltaTime;
            if (timeToReloadCounter <= 0)
            {
                SceneManager.LoadScene("Game");
            }
        }
    }
	
}
