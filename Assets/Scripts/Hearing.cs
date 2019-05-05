using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hearing : MonoBehaviour
{
    //state for hearing
    public enum BlockingState { driving,blocking};
    BlockingState currentState;
    //the object to block
    private GameObject enemyobject;
    //parent object of this gameobject
    public GameObject parentObject;
    //the starting velocity
    public Vector3 intialVelocity;
    //object rigidbody
    private Rigidbody rgd;
    // Start is called before the first frame update
    void Start()
    {
        currentState = BlockingState.driving;
        rgd = parentObject.GetComponent<Rigidbody>();
        intialVelocity = rgd.velocity;
    }

    // Update is called once per frame
    void Update()
    {
        //if blocking, then calculate the side and move
        if(currentState==BlockingState.blocking)
        {
            float zDistance = transform.position.z- enemyobject.transform.position.z;
            if(zDistance>0)
                rgd.velocity += new Vector3(0, 0, 2);
            else if(zDistance<0)
                rgd.velocity += new Vector3(0, 0, -2);

        }
        //if not blocking then reset the velocity
        else if(currentState == BlockingState.driving)
        {
            if(rgd.velocity!=intialVelocity)
            {
                rgd.velocity = intialVelocity;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Hearing")
        {
            enemyobject = other.gameObject;
            currentState = BlockingState.blocking;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Hearing")
        {
            currentState = BlockingState.driving;
        }
    }
}
