using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route {

    public List<Planet> route;
    public float totalDistance;

    public Route(List<Planet> route)
    {
        this.route = route;
        float totalDistance = 0;
        for(int i = 1; i < route.Count; i++)
        {
            totalDistance += Vector3.Distance(route[i - 1].transform.position, route[i].transform.position);
        }
        this.totalDistance = totalDistance;
    }

}
