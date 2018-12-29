using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starlane : MonoBehaviour {

    public Planet planetA;
    public Planet planetB;
    public float distance;
    public bool lineRendererSet = false;

    private void Start()
    {
        
    }
    // Update is called once per frame
    void Update () {
        if (!lineRendererSet)
        {
            LineRenderer lr = GetComponent<LineRenderer>();
            lr.SetPosition(0, planetA.transform.position);
            lr.SetPosition(1, planetB.transform.position);
            
            lineRendererSet = true;
        }
    }

    public Planet ConnectionFrom(Planet p)
    {
        if (planetA.Equals(p))
        {
            return planetB;
        }
        else
        {
            return planetA;
        }
    }

    public static bool Equals(Starlane starlaneA, Starlane starlaneB)
    {
        if(starlaneA.planetA.transform.position == starlaneB.planetA.transform.position && starlaneA.planetB.transform.position == starlaneB.planetB.transform.position)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UpdateColors()
    {
        LineRenderer lr = this.transform.GetComponent<LineRenderer>();
        switch (planetA.owner)
        {
            case OWNER.PLAYER:
                lr.startColor = Color.blue;
                break;
            case OWNER.AI1:
                lr.startColor = Color.red;
                break;
            case OWNER.NONE:
                lr.startColor = Color.white;
                break;
            default:
                break;
        }


        switch (planetB.owner)
        {
            case OWNER.PLAYER:
                lr.endColor = Color.blue;
                break;
            case OWNER.AI1:
                lr.endColor = Color.red;
                break;
            case OWNER.NONE:
                lr.endColor = Color.white;
                break;
            default:
                break;
        }
    }

    // Use this for initialization
    public static Starlane Create(Planet planetA, Planet planetB)
    {
        GameObject obj = new GameObject("starlane", typeof(Starlane));
        LineRenderer lr = obj.AddComponent<LineRenderer>();
            lr.material = new Material(Shader.Find("Particles/Additive"));
            lr.startWidth = 0.35f;
            lr.endWidth = 0.35f; 
        Starlane starlane = obj.GetComponent<Starlane>();
        starlane.planetA = planetA;
        starlane.planetB = planetB;
        starlane.distance = Vector3.Distance(planetA.transform.position, planetB.transform.position);
        return starlane;
    }
}
