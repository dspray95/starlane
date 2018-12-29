using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

    public Planet planet;
    public Node previousNode;
    public List<Starlane> connections;
    public List<Node> expandedNodes;
    public float heuristic;
    public float value;
    public bool expanded = false;

    public Node(Planet startPlanet)
    {
        this.planet = startPlanet;
        this.connections = planet.starlanes;
    }

    public Node(Planet currentPlanet, Node previousNode)
    {
        this.planet = currentPlanet;
        this.previousNode = previousNode;
        this.connections = currentPlanet.starlanes;
    }

    public bool Equals(Node n)
    {
        if(Planet.Equals(planet, n.planet))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public List<Node> Expand()
    {
        List<Node> expandedNodes = new List<Node>();
        foreach (Starlane connection in connections)
        {
            Node n = new Node(connection.ConnectionFrom(planet), this);
            expandedNodes.Add(n);
        }
        this.expandedNodes = expandedNodes;
        this.expanded = true;
        return expandedNodes;
    }
}
