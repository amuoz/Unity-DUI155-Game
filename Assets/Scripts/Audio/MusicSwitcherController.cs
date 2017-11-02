using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSwitcherController : MonoBehaviour {

    private MusicController musicController;

    public int newTrack;

    public bool switchOnStart;

	// Use this for initialization
	void Start () {
        musicController = FindObjectOfType<MusicController>();

        if (switchOnStart)
        {
            musicController.SwitchTrack(newTrack);
            gameObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // cambiar de track al entrar a una zona del mapa concreta
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            musicController.SwitchTrack(newTrack);
            // desactivar el trigger box
            gameObject.SetActive(false);
        }
    }
}
