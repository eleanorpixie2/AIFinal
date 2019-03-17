using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Touch : MonoBehaviour
{
    Quaternion startingRotation;
    // Start is called before the first frame update
    void Start()
    {
        startingRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            transform.rotation=Quaternion.Euler(0,-120,0);
            other.GetComponent<NavMeshAgent>().speed -= 5;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag=="Player")
        {
            Debug.Log("Pushing");
            transform.rotation = Quaternion.Euler(0, -120, 0);
            other.GetComponent<NavMeshAgent>().speed -= 5;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            transform.rotation = startingRotation;
        }
    }
}
