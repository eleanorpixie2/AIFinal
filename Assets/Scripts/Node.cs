using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    //if it is not a barrier
    public bool walkable;
    //the position in the scene
    public Vector3 worldPosition;
    //its x value in the grid
    public int gridX;
    //its y value in the grid
    public int gridY;

    //distance from the start
    public int gCost;
    //distance to the end
    public int hCost;
    //the total cost
    public int fCost
    {
        get { return gCost + hCost; }
    }

    //the parent node
    public Node parent;

    public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }

}
