using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    // referencia a prefab de la caja
    public GameObject boxPrefab;
    public int tileSize = 32;

	// Use this for initialization
	void Start () {
        GameObject boxInstance = Instantiate(boxPrefab);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
