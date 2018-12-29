using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator {

    Transform tPlanet; 

    public Generator(Transform planet)
    {
        this.tPlanet = planet;
    }

    public List<Planet> GetWorlds(int numWorlds, Transform planetPrefab)
    {
        List<Planet> planets = new List<Planet>();
        numWorlds = numWorlds + Random.Range(-2, 2);

        for (int i = 0; i < numWorlds; i++)
        {
            Planet newPlanet = Planet.Create(planetPrefab);
            bool validatingPosition = (planets.Count > 1 ? true : false);
            do
            {
                newPlanet.transform.position = new Vector3(Random.Range(0, 100), Random.Range(0, 100));
                foreach(Planet otherPlanet in planets)
                {
                    if (Vector3.Distance(newPlanet.transform.position, otherPlanet.transform.position) > 5)
                    {
                        validatingPosition = false;
                    }
                }
            } while (validatingPosition);
            planets.Add(newPlanet);
        }
        return planets;
    }

    public List<Starlane> GetStarlanes(List<Planet> worlds)
    {
        List<Starlane> starlanes = new List<Starlane>();
        foreach (Planet planet in worlds)
        {
            int numConnections = getNumConnections();
            //Setup layermasks - NameToLayer is expensive so just use it once here rather than every time we need it
            LayerMask ignoreLayer = LayerMask.NameToLayer("Ignore Raycast");
            LayerMask defaultLayer = LayerMask.NameToLayer("Default");
            //Add a new planet until our number of starlanes is the correct value
            while (planet.starlanes.Count < numConnections)
            {
                float closestDistance = 9999999; //Weird hardcode, "it just works"
                Planet closestPlanet = null;
                foreach (Planet connectingPlanet in worlds)
                {
                    bool invalidConnection = false;
                    foreach (Starlane starlane in planet.starlanes)
                    {

                        foreach (Starlane connectingStarlane in connectingPlanet.starlanes)
                        {
                            if (Starlane.Equals(starlane, connectingStarlane))
                            {
                                invalidConnection = true;
                            }
                        }
                    }
                    Transform planetSphere = planet.transform.GetChild(0);
                    Transform connectingPlanetSphere = connectingPlanet.transform.GetChild(0);
                    Transform planetAura = planetSphere.GetChild(0);
                    Transform connectingPlanetAura = connectingPlanetSphere.GetChild(0);
                    planetSphere.gameObject.layer = ignoreLayer;
                    connectingPlanetSphere.gameObject.layer = ignoreLayer;
                    planetAura.gameObject.layer = ignoreLayer;
                    connectingPlanetAura.gameObject.layer = ignoreLayer;
                    //Returns true if collision with a planet other than the current and connecting planets occurs
                    if (Physics.Linecast(planet.transform.position, connectingPlanet.transform.position))
                    {
                        invalidConnection = true;
                    }
                    planetSphere.gameObject.layer = defaultLayer;
                    connectingPlanetSphere.gameObject.layer = defaultLayer;
                    planetAura.gameObject.layer = defaultLayer;
                    connectingPlanetAura.gameObject.layer = defaultLayer;
                    float distance = Vector3.Distance(planet.transform.position, connectingPlanet.transform.position);
                    if (distance < closestDistance && !invalidConnection && !planet.Equals(connectingPlanet))
                    {
                        closestPlanet = connectingPlanet;
                        closestDistance = Vector3.Distance(planet.transform.position, connectingPlanet.transform.position);
                    }
                }
                if (closestPlanet != null)
                {
                    Starlane newStarlane = Starlane.Create(planet, closestPlanet);
                    starlanes.Add(newStarlane);
                    planet.starlanes.Add(newStarlane);
                    closestPlanet.starlanes.Add(newStarlane);
                }
                else
                {
                    Debug.Log("<NO CLOSEST PLANET FOUND!>");
                    numConnections = -1;
                    break;
                }
            }
        }
        return starlanes;
    }

    private int getNumConnections()
    {
        int numConnections = 1;
        int roll = Random.Range(0, 100);
        if (roll < 5)
        {
            numConnections = 1;
        }
        else if (roll > 5 && roll < 85)
        {
            numConnections = 2;
        }
        else if (roll > 85 && roll < 95)
        {
            numConnections = 3;
        }
        else
        {
            numConnections = 4;
        }
        return numConnections;
    }

    public void AssignWorlds(int numPlayers, List<Planet> worlds)
    {
        List<Route> longestRoutes = new List<Route>();
        foreach(Planet planet in worlds)
        {
            Route bestRoute = null;
            float bestDistance = -1;
            foreach(Planet destination in worlds)
            {
                if (!planet.Equals(destination))
                {
                    Route route = AStar.SolveRoute(planet, destination, worlds);
                    Debug.Log("Route length: " + route.totalDistance);
                    if (route.totalDistance > bestDistance)
                    {
                        bestDistance = route.totalDistance;
                        bestRoute = route;
                    }
                }
            }
            longestRoutes.Add(bestRoute);
        }
        //Could do this in the previous loop somehow, would be more time efficient if possible 
        Route longestRoute = null;
        float longestDistance = -1;
        foreach(Route route in longestRoutes)
        {
            if(route.totalDistance > longestDistance)
            {
                longestDistance = route.totalDistance;
                longestRoute = route;
            }
        }

        if (longestRoute.route[0].transform.position.y > longestRoute.route[longestRoute.route.Count - 1].transform.position.y)
        {
            longestRoute.route[0].SetOwner(OWNER.AI1);
            longestRoute.route[longestRoute.route.Count - 1].SetOwner(OWNER.PLAYER);
            //ToDo color not changing - needs to use emission color

        }
        else
        {
            longestRoute.route[0].SetOwner(OWNER.PLAYER);
            longestRoute.route[longestRoute.route.Count - 1].SetOwner(OWNER.AI1);
        }

    }
}
