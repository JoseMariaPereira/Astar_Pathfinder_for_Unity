using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AStar_Pathfinder_Node))]
public class AStar_Pathfinder_Editor : Editor {
    //Add a button to connect all selected nodes
    [MenuItem("AStar/ConnectSelectedObjects")]
    static void ConnectSelectedObjects()
    {
        //close if there are not enough targets
        if (Selection.transforms.Length <= 1)
        {
            Debug.Log("Not enough valid nodes selected");
            return;
        }
        //Get every selected node and connect them with each other, exlude objects that dont have a node and add them to the connected nodes list
        foreach (Transform trans in Selection.transforms)
        {
            AStar_Pathfinder_Node node = trans.GetComponent<AStar_Pathfinder_Node>();
            if (node != null)
            {
                foreach (Transform t in Selection.transforms)
                {
                    AStar_Pathfinder_Node tnode = t.GetComponent<AStar_Pathfinder_Node>();
                    if (tnode != node && !node.connectedNodes.Contains(tnode) && tnode != null)
                    {
                        node.connectedNodes.Add(tnode);
                    }
                }
            }
        }
    }
    [MenuItem("AStar/DisconnectSelectedObjects")]
    static void DisconnectSelectedObjects()
    {
        //Same as before but removing the nodes from the conected list
        if (Selection.transforms.Length <= 1)
        {
            Debug.Log("Not enough valid nodes selected");
            return;
        }
        foreach (Transform trans in Selection.transforms)
        {
            AStar_Pathfinder_Node node = trans.GetComponent<AStar_Pathfinder_Node>();
            if(node != null)
            {
                foreach (Transform t in Selection.transforms)
                {
                    AStar_Pathfinder_Node tnode = t.GetComponent<AStar_Pathfinder_Node>();
                    if(tnode != node && node.connectedNodes.Contains(tnode) && tnode !=null)
                    {
                        node.connectedNodes.Remove(tnode);
                    }
                }
            }
        }
    }
    //Add a Button to the astar pathfinder node to create new nodes conectet to the selected one
    override public void OnInspectorGUI()
    {
        AStar_Pathfinder_Node pathfinderButton = (AStar_Pathfinder_Node)target;
        if(GUILayout.Button("Add new node"))
        {
            AddNode(pathfinderButton);
        }
        DrawDefaultInspector();
    }
    //Function to add a new node defining every variable
    public void AddNode(AStar_Pathfinder_Node aStar_Pathfinder_Node)
    {
        GameObject newNode = Instantiate(aStar_Pathfinder_Node.gameObject, aStar_Pathfinder_Node.transform.position, Quaternion.identity);
        AStar_Pathfinder_Controller controllerParent = (AStar_Pathfinder_Controller)FindObjectOfType(typeof(AStar_Pathfinder_Controller));
        newNode.transform.SetParent(controllerParent.transform);
        newNode.name = "Node";
        AStar_Pathfinder_Node nodePath = newNode.GetComponent<AStar_Pathfinder_Node>();
        aStar_Pathfinder_Node.connectedNodes.Add(nodePath);
        nodePath.connectedNodes.Clear();
        nodePath.connectedNodes.Add(aStar_Pathfinder_Node);
        Selection.activeGameObject = newNode;
    }
}
