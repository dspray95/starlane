using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar {

	public static Route SolveRoute(Planet start, Planet end, List<Planet> planets)
    {
        //init route solver
        List<Node> frontier = new List<Node>();
        List<Node> closedList = new List<Node>();
        bool goalAchieved = false;
        int iterations = 0;
        Node currentNode = new Node(start);
        frontier = BuildFrontier(currentNode, frontier, closedList, end);

        while(!goalAchieved)
        {
            closedList.Add(currentNode);
            if (GoalCheck(end, currentNode)){
                goalAchieved = true;
                break;
            }

            if (frontier.Contains(currentNode))
            {
                frontier.Remove(currentNode);
            }

            currentNode = GetBestNode(currentNode, frontier);
            frontier = BuildFrontier(currentNode, frontier, closedList, end);

            iterations++;
            if (iterations > 100)
            {
                goalAchieved = true;
                break;
            }
        }
        
        return new Route(BuildPlanetRoute(currentNode));
    }

    public static List<Planet> BuildPlanetRoute(Node finalNode)
    {
        bool buildingList = true;
        Node currentNode = finalNode;
        List<Planet> planetList = new List<Planet>();
        while (buildingList)
        {
            planetList.Add(currentNode.planet);
            if (currentNode.previousNode != null)
            {
                currentNode = currentNode.previousNode;
            }
            else
            {
                buildingList = false;
                break;
            }
        }
        return planetList;
    }

    public static Node GetBestNode(Node currentNode, List<Node> frontier)
    {
        float bestValue = 999999999;
        Node bestNode = currentNode;
        foreach(Node fNode in frontier)
        {
            fNode.value = Vector3.Distance(currentNode.planet.transform.position, fNode.planet.transform.position) + fNode.heuristic;
            if (fNode.value < bestValue)
            {
                bestValue = fNode.value;
                bestNode = fNode;
            }
        }

        return bestNode;
    }

    public static List<Node> BuildFrontier(Node expandingNode, List<Node> frontier, List<Node> closedList, Planet goal)
    {
        List<Node> newFrontier = frontier; 
        List<Node> expandResult = expandingNode.Expand();
        
        foreach(Node nNode in expandResult)
        {
            bool invalidNode = false;
            foreach(Node oldNode in newFrontier)
            {
                if (nNode.Equals(oldNode))
                {
                    invalidNode = true;
                }
                
            }
            foreach(Node closedNode in closedList)
            {
                if (closedNode.Equals(nNode))
                {
                    invalidNode = true;
                }
            }
            if (!invalidNode)
            {
                nNode.heuristic = Vector3.Distance(nNode.planet.transform.position, goal.transform.position);
                frontier.Add(nNode);
            }
        }

        return newFrontier;
    }

    public static bool GoalCheck(Planet goal, Node currentNode)
    {
        return goal.Equals(currentNode.planet);
    }
}
