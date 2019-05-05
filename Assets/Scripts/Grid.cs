using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    //the layer that all the barriers and obstacles are on
    public LayerMask barrierLayer;
    //the size of the grid in unity scene units
    public Vector2 gridWorldSize;
    //radius of each node
    public float nodeRadius;
    //array of nodes
    Node[,] grid;

    //the final path
    public List<Node> path;

    //diameter of node
    float nodeDiameter;
    //the size of the x and y max of the gird
    int gridSizeX, gridSizeY;

    void Awake()
    {
        //calculate the diameter and gride maxes
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    //make the gird with nodes
    void CreateGrid()
    {
        //set the grid to a 2 dimensional array
        grid = new Node[gridSizeX, gridSizeY];
        //calculate the bottom left of the grid in the scene
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        //for each x value in the grid
        for (int x = 0; x < gridSizeX; x++)
        {
            //for each y value in the grid
            for (int y = 0; y < gridSizeY; y++)
            {
                //calculate the world point of the node
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                //check if the space is an obstacle or not
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, barrierLayer));
                //add the node to the grid
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    //get the surrounding nodes of the node passed in
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        //check each direction
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                //if both are 0 it is the current node so pass the rest of the code
                if (x == 0 && y == 0)
                    continue;

                //check the spaces either 1 above or -1 below the current node
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                //as long as the node is withing the grid, add it as a neighboring node
                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        //return the list of neighbor nodes
        return neighbours;
    }

    //get the node from a position in the scene
    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        //calculate the x position
        float percentX = (worldPosition.x - transform.position.x) / gridWorldSize.x + 0.5f - (nodeRadius / gridWorldSize.x);
        //calculate the y position
        float percentY = (worldPosition.z - transform.position.z) / gridWorldSize.y + 0.5f - (nodeRadius / gridWorldSize.y);
        //clamp the values between 0 and 1
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        //calculate the whole number of the node position in the grid
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        //return the node from the grid
        return grid[x, y];
    }

    //draw the gizmos to see the gird in the scene
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (grid != null)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                if (path != null)
                    if (path.Contains(n))
                        Gizmos.color = Color.black;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }
}
