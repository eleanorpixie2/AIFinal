using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hearing : MonoBehaviour
{
    public enum BlockingState { driving,blocking};
    BlockingState currentState;
    private GameObject enemyobject;
    public GameObject parentObject;
    public Vector3 intialVelocity;
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
        if(currentState==BlockingState.blocking)
        {
            float zDistance = transform.position.z- enemyobject.transform.position.z;
            if(zDistance>0)
                rgd.velocity += new Vector3(0, 0, 2);
            else if(zDistance<0)
                rgd.velocity += new Vector3(0, 0, -2);

        }
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
        if (other.gameObject.GetComponentInChildren<Hearing>() != null)
        {
            enemyobject = other.gameObject;
            currentState = BlockingState.blocking;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponentInChildren<Hearing>() != null)
        {
            currentState = BlockingState.driving;
        }
    }
}
