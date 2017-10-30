using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public Slider healthBar;
    public Text HPText;
    public PlayerController playerController;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        healthBar.maxValue = playerController.maxHealth;
        healthBar.value = playerController.currentHealth;
        HPText.text = "HP: " + playerController.currentHealth + "/" + playerController.maxHealth;
    }
}
