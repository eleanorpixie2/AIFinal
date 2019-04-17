using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : MonoBehaviour
{
    Rigidbody rgd;
    bool needsToGetAround;
    Vector3 intialSpeed;

    public enum GetAroundFSM { MoveRight,MoveLeft,ContinueDriving};
    private GetAroundFSM currentState;
    public enum MovementType { NavMesh,WayPoints,AStar};
    public MovementType _movementType;
    // Start is called before the first frame update
    void Start()
    {
        rgd = GetComponent<Rigidbody>();
        intialSpeed = rgd.velocity;
        currentState = GetAroundFSM.ContinueDriving;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 fwd=Vector3.zero;
        //fwd vector

        Vector3 start = new Vector3(transform.position.x - 1, transform.position.y + .5f, transform.position.z + .09f);
        fwd = transform.TransformDirection(Vector3.forward);

        //raycast result
        RaycastHit hit;
        //do a raycast
        if (Physics.Raycast(transform.position, fwd, out hit))
        {
            //if player then move
            if (hit.collider.tag == "Player" && currentState == GetAroundFSM.ContinueDriving)
                DetermineDirection(hit.collider.gameObject);
            //else if (hit.collider.tag == "Player" && _movementType == MovementType.WayPoints && currentState == GetAroundFSM.ContinueDriving)
            //    DetermineDirection(hit.collider.gameObject);
            //otherwise ignore
            else
            {
                if (intialSpeed != rgd.velocity)
                {
                    rgd.velocity = intialSpeed;
                }
                if (currentState != GetAroundFSM.ContinueDriving)
                    currentState = GetAroundFSM.ContinueDriving;
            }
        }
        //if there is nothing in front of the car
        else
        {
            if (currentState != GetAroundFSM.ContinueDriving)
                currentState = GetAroundFSM.ContinueDriving;
        }
        
        //If the car is just driving
        if(currentState==GetAroundFSM.ContinueDriving)
        {
            if (intialSpeed != rgd.velocity)
            {
                rgd.velocity = intialSpeed;
            }
        }

        //If the car needs to move to the right
        if (currentState==GetAroundFSM.MoveRight)
        {
            GetAroundOtherCarRight();
        }
        //If the car needs to move to the left
        if(currentState == GetAroundFSM.MoveLeft)
        {
            GetAroundOtherCarLeft();
        }

    }

    //determine which way to move
    void DetermineDirection(GameObject other)
    {
        float direction = Math.Abs(other.gameObject.transform.position.z) - Math.Abs(transform.position.z);
        
        //make sure that the other car isn't behind
        if(Math.Abs(other.gameObject.transform.position.x) - Math.Abs(transform.position.x)<0)
            currentState = GetAroundFSM.ContinueDriving;
        //move the opposite direction of car in front
        else if (direction > 2)
            currentState = GetAroundFSM.MoveLeft;
        else if (direction < 2)
            currentState = GetAroundFSM.MoveRight;
        else
            currentState = GetAroundFSM.MoveLeft;
    }

    //move to the right
    void GetAroundOtherCarRight()
    {
        switch (_movementType)
        {
            case MovementType.NavMesh:
                rgd.velocity += new Vector3(-1, 0, 2);
                break;
            case MovementType.WayPoints:
                rgd.velocity = new Vector3(-1, 0, 2);
                break;
        }
    }
    //move to the left
    void GetAroundOtherCarLeft()
    {

        switch(_movementType)
        {
            case MovementType.NavMesh:
                rgd.velocity += new Vector3(-1, 0, -2);
            break;
            case MovementType.WayPoints:
                rgd.velocity = new Vector3(-1, 0, -2);
            break;
        }
    }

    //if hitting the rail then stop and move back and away from the railing
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Rail")
        {
            currentState = GetAroundFSM.ContinueDriving;
            float direction = Math.Abs(other.gameObject.transform.position.z) - Math.Abs(transform.position.z);
            if (direction < 0)
                rgd.velocity += new Vector3(0, 0, 2);
            if (direction > 0)
                rgd.velocity += new Vector3(0, 0, -2);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        Vector3 start = new Vector3(transform.position.x - 1, transform.position.y + .5f, transform.position.z+.09f);
        //fwd.y = start.y;
        Gizmos.DrawRay(start, fwd);
    }
}
