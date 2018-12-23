using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour {

    public Transform planet; 

	// Use this for initialization
	void Start () {
        Generator generator = new Generator(planet);
        generator.GetWorlds(10);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
