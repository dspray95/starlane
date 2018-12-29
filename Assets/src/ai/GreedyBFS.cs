using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreedyBFS {

	public static Route SolveForPlanetRoute(Planet start, Planet end)
    {
        List<Planet> routeFinal = new List<Planet>();
        List<Node> closedList = new List<Node>();
        List<Node> frontier = new List<Node>();
        bool goalAchieved = false;
        Planet goal = end;
        Node startNode = new Node(start);
        Node currentNode = startNode;
        int iterations = 0;
        while (!goalAchieved)
        {
            if (frontier.Contains(currentNode))
            {
                frontier.Remove(currentNode);
            }

            if (!currentNode.expanded)
            {
                frontier = SortFrontier(currentNode.Expand(), frontier);
                currentNode.expanded = true;
            }
            closedList.Add(currentNode);
            currentNode = GetBestNode(currentNode, closedList, goal);
            goalAchieved = GoalCheck(currentNode.planet, goal) ? true : false;
            iterations++;
            if(iterations > 50) //ToDo this needs to adjust for seriously bigger map sizes, for most sizes around 10 planets it works fine
            {
                Debug.Log("<GBFS Could Not Find Route!!!>");
                //We couldn't find a connection to the goal planet so we should try to create one
                Starlane starlane = Starlane.Create(currentNode.planet, goal);
                currentNode.planet.starlanes.Add(starlane);
                goal.starlanes.Add(starlane);
                //Throw an error here
                break;
            }
        }
        //BuildRoute();
        return GetRoute(currentNode);
    }

    public static List<Node> ClosedListCheck(Node current, List<Node> closedList)
    {
        int numExpanded = 0;
        foreach (Node n in current.expandedNodes)
        {
            if (n.expanded)
            {
                numExpanded++;
            }
        }
        if (numExpanded >= current.expandedNodes.Count)
        {
            closedList.Add(current);
        }
        return closedList;
    }

    public static Node GetBestNode(Node current, List<Node> closedList, Planet goal) 
    {
        List<Node> frontier = current.Expand();
        Node bestNode = null;
        float bestDistance = 9999999999999;
        foreach(Node n in frontier)
        {
            float currentDistance = Vector3.Distance(n.planet.transform.position, goal.transform.position);
            bool invalidNode = false;
            foreach(Node closedNode in closedList)
            {
                if (n.Equals(closedNode))
                {
                    invalidNode = true;
                }
            }
            if (currentDistance < bestDistance && !invalidNode) 
            {
                bestNode = n;
                bestDistance = currentDistance;
            }
        }
        return bestNode;
    }

    public static List<Node> SortFrontier(List<Node> toAdd, List<Node> currentFrontier)
    {
        List<Node> newFrontier = currentFrontier;
        foreach(Node n in toAdd)
        {
            if (!newFrontier.Contains(n))
            {
                newFrontier.Add(n);
            }
        }

        return newFrontier;
    }

    public static bool GoalCheck(Planet currentPlanet, Planet goal)
    {
        return (Planet.Equals(currentPlanet, goal) ? true : false);
    }

    private static Route GetRoute(Node finalNodel)
    {
        bool traversingNodes = true;
        List<Node> nodes = new List<Node>();
        Node currentNode = finalNodel;
        nodes.Add(currentNode);
        while (traversingNodes)
        {
            if(currentNode.previousNode != null)
            {
                currentNode = currentNode.previousNode;
                nodes.Add(currentNode);
            }
            else
            {
                traversingNodes = false;
            }
        }

        List<Planet> route = new List<Planet>();
        int nodeCount = nodes.Count - 1;
        for(int i = 0; i < nodes.Count; i++)
        {
            route.Add(nodes[nodeCount].planet);
            nodeCount--;
        }
        return new Route(route);
    }

}
