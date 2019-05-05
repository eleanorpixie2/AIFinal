using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    //the gameobject that is going to be moving
    public Transform agent;
    //the goal position
    public Transform target;
    //the grid reference
    Grid grid;

    void Awake()
    {
        //get the grid
        grid = GetComponent<Grid>();
    }

    void Update()
    {
        //calculate the path
        FindPath(agent.position, target.position);
    }

    //find the path based on the current position of the agent and the position of the current goal
    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        //get the nodes based on their positions in the scene
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        //list of the nodes that haven't been moved to yet and aren't barriers
        List<Node> openNodes = new List<Node>();
        //list of nodes that have been moved to
        HashSet<Node> closedNodes = new HashSet<Node>();
        //add the agent's position as the first node in the set
        openNodes.Add(startNode);

        //while there are open nodes, calculate a path
        while (openNodes.Count > 0)
        {
            //the current node
            Node currentNode = openNodes[0];
            //for each node in the open nodes list
            for (int i = 1; i < openNodes.Count; i++)
            {
                //if the f cost is less than the current node or equal to then try to set it as the new current node
                if (openNodes[i].fCost < currentNode.fCost || openNodes[i].fCost == currentNode.fCost)
                {
                    //check to see if the h cost is less than the current node
                    //if so then set it as the new current node
                    if (openNodes[i].hCost < currentNode.hCost)
                        currentNode = openNodes[i];
                }
            }
            //remove the current node from the list
            openNodes.Remove(currentNode);
            //add the current node to the closed node list
            closedNodes.Add(currentNode);

            //if the current node is the goal then calculate the final path and break the loop
            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }

            //for each node in the list if neighboring nodes check to see if they are walkable
            //or haven't previously been moved to
            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                //if a barrier or has been previously been moved to then skip past the rest of the code
                if (!neighbour.walkable || closedNodes.Contains(neighbour))
                {
                    continue;
                }

                //recalculate the cost between the node and the neighboring node
                int newCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                //if the cost is lower then the neighboring node cost or the open nodes list doesnt contain it already
                if (newCostToNeighbour < neighbour.gCost || !openNodes.Contains(neighbour))
                {
                    //calculate the current costs of the node
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    //set the parent to the current node
                    neighbour.parent = currentNode;

                    //add to the open list if it has not been add previously
                    if (!openNodes.Contains(neighbour))
                        openNodes.Add(neighbour);
                }
            }
        }
    }

    //calculate the final path
    void RetracePath(Node startNode, Node endNode)
    {
        //list of nodes for the final path
        List<Node> path = new List<Node>();
        //start at the end node
        Node currentNode = endNode;

        //while the current node doesn't equal the agent retrace the nodes by the parent values
        while (currentNode != startNode)
        {
            //add the current node to the path
            path.Add(currentNode);
            //set the current node to its parent node
            currentNode = currentNode.parent;
        }
        //reorder the path so that it starts from the agent
        path.Reverse();

        //set the grid's final path to this path
        grid.path = path;

    }

    //get the distance between 2 nodes
    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }

}
