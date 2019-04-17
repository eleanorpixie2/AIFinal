using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    Grid grid;
    public Transform startPositon;
    public Transform targetPosition;
    // Start is called before the first frame update
    void Awake()
    {
        grid = GetComponent<Grid>();
        //FindPath(startPositon.position, targetPosition.position);
    }
    private void Start()
    {
        grid.MakeGrid();
        FindPath(startPositon.position, targetPosition.position);
    }

    // Update is called once per frame
    void Update()
    {
        FindPath(startPositon.position, targetPosition.position);
    }

    private void FindPath(Vector3 position1, Vector3 position2)
    {
        Node startNode = grid.NodeFromWorldPosition(position1);
        Node targetNode = grid.NodeFromWorldPosition(position2);

        List<Node> openList = new List<Node>();
        HashSet<Node> closedList = new HashSet<Node>();

        openList.Add(startNode);
        while(openList.Count>0)
        {
            Node currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].fCost < currentNode.fCost || openList[i].fCost == currentNode.fCost
                    && openList[i].hCost < currentNode.hCost)
                {
                    currentNode = openList[i];
                }
            }
            openList.Remove(currentNode);
            closedList.Add(currentNode);
            if(currentNode==targetNode)
            {
                GetFinalPath(startNode, targetNode);
                break;
            }

            foreach (Node neighborNode in grid.GetNeighborNodes(currentNode))
            {
                if (!neighborNode.isBarrier || closedList.Contains(neighborNode))
                {
                    continue;
                }
                int moveCost = currentNode.gCost + GetManhattanDistance(currentNode, neighborNode);
                if (!openList.Contains(neighborNode) || moveCost < neighborNode.fCost)
                {
                    neighborNode.gCost = moveCost;
                    neighborNode.hCost = GetManhattanDistance(neighborNode, targetNode);
                    neighborNode.parent = currentNode;

                    if(!openList.Contains(neighborNode))
                    {
                        openList.Add(neighborNode);
                    }
                }

            }
        }
    }

    private int GetManhattanDistance(Node currentNode, Node neighborNode)
    {
        int x = Mathf.Abs(currentNode.gridX - neighborNode.gridX);
        int y = Mathf.Abs(currentNode.gridY - neighborNode.gridY);

        return x + y;
    }

    private void GetFinalPath(Node startNode, Node targetNode)
    {
        List<Node> finalPath = new List<Node>();
        Node currentNode = targetNode;
        while(currentNode!=startNode)
        {
            finalPath.Add(currentNode);
            currentNode = currentNode.parent;
        }

        finalPath.Reverse();
        grid.finalPath = finalPath;
    }
}
