using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeManager : MonoBehaviour {

    public VolumeController[] vcObjects;

	// Use this for initialization
	void Start () {
        vcObjects = FindObjectsOfType<VolumeController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
