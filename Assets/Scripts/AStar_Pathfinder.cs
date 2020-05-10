using System.Collections.Generic;
using UnityEngine;

public class AStar_Pathfinder : MonoBehaviour {
	
	AStar_Pathfinder_Controller aStarController;

    List<AStar_Pathfinder_Node> openList = new List<AStar_Pathfinder_Node>();//list of nodes to check pathfinding
    List<AStar_Pathfinder_Node> closedList = new List<AStar_Pathfinder_Node>();//list of nodes already checked trough pathfinding

    public bool cellBased = false;//if making a cellbased game

    public List<AStar_Pathfinder_Node> closestPath = new List<AStar_Pathfinder_Node>();//Adding the nodes to the shortes path

    void Start ()
	{
		aStarController = (AStar_Pathfinder_Controller)FindObjectOfType(typeof(AStar_Pathfinder_Controller));
		if(aStarController == null)
		{
			Debug.LogWarning("No AStar Controller found");
            return;
		}
        //Testing script on start
        Pathfind(aStarController.startPosition, aStarController.targetPosition);
	}

	void Pathfind (AStar_Pathfinder_Node startingNode, AStar_Pathfinder_Node endingNode)
	{
        //Clear all lists just in case
        openList.Clear();
        closedList.Clear();
        closestPath.Clear();
        //add the starting node to the openlist to start checking in it
        openList.Add(startingNode);
        //start loop with the starting node and the endnode
        PathfindLoop(startingNode, endingNode);
	}

    //loop while openlist is not empty
    void PathfindLoop(AStar_Pathfinder_Node currentNode, AStar_Pathfinder_Node endingNode)
    {
        //add the current node to closed node list and remove from openlist so it wont check it again at a further stage
        openList.Remove(currentNode);
        closedList.Add(currentNode);
        //if you are at the end, bravo, you are done
        if(currentNode == endingNode)
        {
            //To get the list, go backwards through the shortest path "g" of each node and stop when you get to the startingnode
            closestPath.Add(endingNode);
            GetClosestPath(endingNode);
            return;
        }
        //Check every connected nodes in the current one
        foreach (AStar_Pathfinder_Node node in currentNode.connectedNodes)
        {
            //Check if already discarted
            if (closedList.Contains(node))
            {
                continue;
            }
            //Enshure that it is the closest Path

            /*total dinstance so far, the straight distance betwen this node and the end and get the total length
            (reminding from this node onwards is a straight line witch should not happen,
            but this way we get an overall understanding on its world position relative to the end)--*/

            //this g is for literal distance
            float distanceSoFar = currentNode.distanceSoFar + Vector3.Distance(currentNode.transform.position, node.transform.position);
            //this g is cellSpace oriented distance
            if (cellBased)
            {
                distanceSoFar = currentNode.distanceSoFar + node.terrainValue;
            }

            float straightDistanceLeft = Vector3.Distance(node.transform.position, endingNode.transform.position);

            float totalDistance = distanceSoFar + straightDistanceLeft;

            //check if this node is already in the openlist and if there is a closer path to it
            if (openList.Contains(node))
            {
                if(totalDistance > node.totalDistance)
                {
                    continue;
                }
            }
            //add node to the opnelist and the pathStats
            node.distanceSoFar = distanceSoFar;
            node.straightDistanceLeft = straightDistanceLeft;
            node.totalDistance = totalDistance;
            openList.Add(node);
        }
        //find witch node has the best f potential to maximize pathfinding efficiency
        AStar_Pathfinder_Node nextNode = null;
        float i = 0;
        foreach (AStar_Pathfinder_Node node in openList)
        {
            if (i == 0 || node.totalDistance < i)
            {
                nextNode = node;
                i = node.totalDistance;
            }
        }
        //aaaand start loop again, pretty simple
        PathfindLoop(nextNode, endingNode);
    }

    //once you reach the end of your loop to find the shortest path, invert the loop order using the data stored on every node until the end usgin the g variable stored, witch leads to the shortest way to the start
    void GetClosestPath(AStar_Pathfinder_Node currentNode)
    {
        AStar_Pathfinder_Node nextNode = null;
        float g = -1;
        float nodeTotalG = -1;
        //Find the closest node with the lowes (g stat + your distance with the node) so you get the shortest path to the start
        foreach (AStar_Pathfinder_Node node in currentNode.connectedNodes)
        {
            //for literal distance
            nodeTotalG = node.distanceSoFar + Vector3.Distance(node.transform.position, currentNode.transform.position);
            //for cellbased distance
            if (cellBased)
            {
                nodeTotalG = node.distanceSoFar;
            }

            if(g == -1 || nodeTotalG < g)
            {
                nextNode = node;
                g = nextNode.distanceSoFar;
            }
        }
        //add node to the closest path list
        closestPath.Add(nextNode);
        //keep on going till g == 0
        if(nextNode.distanceSoFar != 0)
        {
            GetClosestPath(nextNode);
        }
    }

}
