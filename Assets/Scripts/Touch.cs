using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Touch : MonoBehaviour
{
    //Origanl rotation, will come back after no longer pushing
    Quaternion startingRotation;

    private GameObject otherPlayer;

    //Direction to push in
   enum PushDirection { Right,Left,Front,None};
    //Current pushing direction
    PushDirection currentDirection = PushDirection.None;

    // Start is called before the first frame update
    void Start()
    {
        //intialize starting rotation
        startingRotation = transform.rotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //based on pushing direction, rotate in that direction
        if(currentDirection==PushDirection.Right)
        {
            otherPlayer.GetComponent<Rigidbody>().AddForce(0, 2,7);
            otherPlayer.GetComponent<Rigidbody>().drag += .5f;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, -120, 0), .5f);
        }
        else if(currentDirection == PushDirection.Left)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, -60, 0), .5f);
            otherPlayer.GetComponent<Rigidbody>().AddForce(0, -2, -7);
            //GetComponent<Rigidbody>().velocity += new Vector3(0, 0, -.05f);
            otherPlayer.GetComponent<Rigidbody>().drag += .5f;
        }
        else if (currentDirection == PushDirection.Front)
        {
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, -60, 0), .5f);
            //To-Do add rotation to front player
            otherPlayer.GetComponent<Rigidbody>().AddForce(-7, 2, 0);
        }
        else
        {
            transform.rotation = startingRotation;
        }

    }


    //Determine which way to push the other car
    private void DetermineWhatSideToPushBack(Collider other)
    {
        float zDistance = Math.Abs(transform.position.z)-Math.Abs(other.transform.position.z);
        float xDistance = Math.Abs(transform.position.x) - Math.Abs(other.transform.position.x);
        otherPlayer = other.gameObject;
        if(zDistance>0)
        {
            currentDirection = PushDirection.Right;
        }
        else if(zDistance<0)
        {
            currentDirection = PushDirection.Left;
        }
        if(xDistance>0)
        {
            currentDirection = PushDirection.Front;
        }
    }

    //If hitting another car
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            DetermineWhatSideToPushBack(other);
        }
    }
    //If no longer pushing a car
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            currentDirection = PushDirection.None;
            otherPlayer.GetComponent<Rigidbody>().drag = 0f;
            transform.rotation = Quaternion.Slerp(transform.rotation,startingRotation,.2f);
        }
    }
}
