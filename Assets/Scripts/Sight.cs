using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : MonoBehaviour
{
    Rigidbody rgd;
    public float checkDistance= 3;
    bool needsToGetAround;
    Vector3 intialSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rgd = GetComponent<Rigidbody>();
        needsToGetAround = false;
        intialSpeed = rgd.velocity;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;
        Debug.DrawRay(transform.position, fwd, Color.red);
        if (Physics.Raycast(transform.position, fwd, out hit,10))
        { 
            print("move");
            if (hit.collider.tag == "Player")
                needsToGetAround = true;
            else
            {
                needsToGetAround = false;
                if (intialSpeed != rgd.velocity)
                {
                    rgd.velocity = intialSpeed;
                }
            }
        }
        else
        {
            needsToGetAround = false;
            if (intialSpeed != rgd.velocity)
            {
                rgd.velocity = intialSpeed;
            }
        }

        if (needsToGetAround)
        {
            GetAroundOtherCar();
        }

    }

    void GetAroundOtherCar()
    {
        rgd.velocity += new Vector3(0,0,-2);
    }

}
