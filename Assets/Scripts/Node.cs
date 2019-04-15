using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    //x position in the node array
    public int gridX;
    //y position in the node array
    public int gridY;

    //if the path is obstructed
    public bool isBarrier;
    //the world postion of node
    public Vector3 position;

    //the previous node
    public Node parent;

    //distance from start
    public int gCost;
    //distance from end
    public int hCost;
    //total cost of moving to that node
    public int fCost { get { return gCost + hCost; } }

    public Node( bool barrier, Vector3 pos, int x,int y)
    {
        isBarrier = barrier;
        position = pos;
        gridX = x;
        gridY = y;
    }
}
