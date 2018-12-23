using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder {

    public Transform planet;

    public Builder(Transform planet)
    {
        this.planet = planet;
    }
    public void BuildPlanets(List<Planet> worlds)
    {
        foreach(Planet planet in worlds)
        {
            Transform.Instantiate(planet);
        }
    }
}
