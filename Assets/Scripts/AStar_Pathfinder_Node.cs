using System.Collections.Generic;
using UnityEngine;


public class AStar_Pathfinder_Node : MonoBehaviour {

	//Nodes Connected to this one
    public List<AStar_Pathfinder_Node> connectedNodes = new List<AStar_Pathfinder_Node>();

	//Overall Node information
    public int terrainValue = 1;

    public float totalDistance, distanceSoFar, straightDistanceLeft;

    //Draw conection lines for visual feedback
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if(connectedNodes.Count != 0)
        {
            foreach (AStar_Pathfinder_Node node in connectedNodes)
            {
                if (node == null)
                {
                    connectedNodes.Remove(node);
                    break;
                }
                Gizmos.DrawLine(transform.position, node.transform.position);
            }
        }

    }
}
