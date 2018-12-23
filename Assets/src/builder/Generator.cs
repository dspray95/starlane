using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator {

    List<Transform> worlds = new List<Transform>();
    Transform tPlanet; 

    public Generator(Transform planet)
    {
        this.tPlanet = planet;
    }

    public List<Transform> GetWorlds(int numWorlds)
    {
        numWorlds = numWorlds + (Random.Range(-2, 2));

        for (int i = 0; i <= numWorlds; i++)
        {
            Transform planet = Transform.Instantiate(tPlanet);


            bool validatingPosition = (worlds.Count > 1 ? true : false);

            do
            {
                planet.position = new Vector3(Random.Range(0, 100), Random.Range(0, 100));
                
                foreach (Transform otherPlanet in worlds)
                {
                    if (Vector3.Distance(planet.transform.position, otherPlanet.transform.position) > 5)
                    {
                        validatingPosition = false;
                    }
                }
            } while (validatingPosition);

            worlds.Add(planet);
        }
        foreach(Transform planet in worlds)
        {
            Planet scrPlanet = planet.GetComponent<Planet>();
            int numConnections = Random.Range(1, 4);
            LayerMask ignoreLayer = LayerMask.NameToLayer("Ignore Raycast");
            LayerMask planetLayer = LayerMask.NameToLayer("Default");

            while (scrPlanet.connections.Count < numConnections)
            {
                float closestDistance = 999999; //Weird hardcode, probably a neater way to achieve this... "it just works"
                Transform closestPlanet = null;
                foreach (Transform connectingPlannet in worlds)
                {
                    Transform a = planet;
                    Transform b = connectingPlannet;

                    bool invalidConnection = false; //Contains doesn't seem to be working for some reason...
                    foreach(Transform connection in scrPlanet.connections)
                    {
                        //Testing that the connection is valid here
                        //Do some layer mask trickery to make the line cast not collide with the source planet
                        planet.gameObject.layer = ignoreLayer;
                        connectingPlannet.gameObject.layer = ignoreLayer;
                        if (connection.Equals(connectingPlannet) || connectingPlannet.GetComponent<Planet>().connections.Contains(planet))
                        {
                            invalidConnection = true;
                        }
                        else if(Physics.Linecast(planet.position, connectingPlannet.position)){ //Returns true if collision with a planet other than the current and connecting planets occurs
                            invalidConnection = true;
                        }
                        //reset layer mask for later collision detections 
                        planet.gameObject.layer = planetLayer;
                        connectingPlannet.gameObject.layer = planetLayer;
                    }
                    if (Vector3.Distance(planet.position, connectingPlannet.position) < closestDistance && !invalidConnection && !planet.Equals(connectingPlannet))
                    {
                        closestPlanet = connectingPlannet;
                        closestDistance = Vector3.Distance(planet.position, connectingPlannet.position);
                    }

                }
                scrPlanet.connections.Add(closestPlanet);
            }
        }
        return worlds;
    }

}
