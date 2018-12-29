using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour {

    public Transform planet;
    public Material matPlanet;
    public List<Starlane> starlanes = new List<Starlane>();
    public List<Planet> planets = new List<Planet>();
    public Planet planetA;
    public Planet planetB;
	// Use this for initialization
	void Start () {
        Generator generator = new Generator(planet);
        //starlanes = generator.GetStarlanes()
        planets = generator.GetWorlds(10, planet);
        starlanes = generator.GetStarlanes(planets);
        planetA = planets[0];
        planetB = planets[1];
        AStar.SolveRoute(planetA, planetB, planets);
        generator.AssignWorlds(2, planets);
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonDown (0)) {    
             var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
             RaycastHit hit;
 
             if (Physics.Raycast(ray, out hit, 100)) {
                 // whatever tag you are looking for on your game object
                 if(hit.collider.tag == "Star") {                         
                     Debug.Log("---> Hit: ");                        
                 }
             }    
         }

    }
}
