using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Transform startPostion;
    //layer that barriers are on
    public LayerMask barrierMask;
    public LayerMask trackMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public float distance;

    Node[,] grid;
    public List<Node> finalPath;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    private void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        //MakeGrid();
    }

    public Node NodeFromWorldPosition(Vector3 position1)
    {
        float xPoint=((position1.x+gridWorldSize.x/2)/gridWorldSize.x);
        float yPoint =((position1.z + gridWorldSize.y / 2) / gridWorldSize.y);

        xPoint = Mathf.Clamp01(xPoint);
        yPoint = Mathf.Clamp(yPoint,-1,1);

        int x = Mathf.RoundToInt((gridSizeX - 1) * xPoint);
        int y = Mathf.RoundToInt((gridSizeY - 1) * yPoint);

        return grid[x, Mathf.Abs(y)];
    }

    public void MakeGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool notBlocked = true;
                if(Physics.CheckSphere(worldPoint,nodeRadius,barrierMask) /*|| !Physics.CheckSphere(worldPoint, nodeRadius, trackMask)*/)
                {
                    notBlocked = false;
                }

                grid[x,y] = new Node(notBlocked, worldPoint, x, y);
            }
        }
    }

    public List<Node> GetNeighborNodes(Node currentNode)
    {
        List<Node> neighboringNodes = new List<Node>();
        int xCheck, yCheck;

        //right side
        xCheck = currentNode.gridX + 1;
        yCheck = currentNode.gridY;
        if (xCheck >= 0 & xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighboringNodes.Add(grid[xCheck, yCheck]);
            }
        }

        //left side
        xCheck = currentNode.gridX - 1;
        yCheck = currentNode.gridY;
        if (xCheck >= 0 & xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighboringNodes.Add(grid[xCheck, yCheck]);
            }
        }

        //top side
        xCheck = currentNode.gridX;
        yCheck = currentNode.gridY + 1;
        if (xCheck >= 0 & xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighboringNodes.Add(grid[xCheck, yCheck]);
            }
        }

        //bottom side
        xCheck = currentNode.gridX;
        yCheck = currentNode.gridY - 1;
        if (xCheck >= 0 & xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighboringNodes.Add(grid[xCheck, yCheck]);
            }
        }

        return neighboringNodes;

    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
    //    if(grid!=null)
    //    {
    //        foreach(Node n in grid)
    //        {
    //            if(!n.isBarrier)
    //            {
    //                Gizmos.color = Color.yellow;
    //            }
    //            else
    //            {
    //                Gizmos.color = Color.white;
    //            }

    //            if(finalPath!=null)
    //            {
    //                if (finalPath.Contains(n))
    //                    Gizmos.color = Color.red;
                    
    //            }

    //            Gizmos.DrawCube(n.position, Vector3.one * (nodeDiameter - distance));
    //        }
    //    }
    //}
}
