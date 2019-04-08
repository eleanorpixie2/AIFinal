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
        //fwd vector
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        //raycast result
        RaycastHit hit;
        Debug.DrawRay(transform.position, fwd, Color.red,.5f,true);
        //do a raycast
        if (Physics.Raycast(transform.position, fwd, out hit,10))
        { 
            print("move");
            //if player then move
            if (hit.collider.tag == "Player")
                DetermineDirection(hit.collider.gameObject);
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
        if (direction > 0)
            currentState = GetAroundFSM.MoveLeft;
        else if (direction < 0)
            currentState = GetAroundFSM.MoveRight;
    }

    //move to the right
    void GetAroundOtherCarRight()
    {
        rgd.velocity += new Vector3(1,0,2);
    }
    //move to the left
    void GetAroundOtherCarLeft()
    {
        rgd.velocity += new Vector3(1, 0, -2);
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
}
