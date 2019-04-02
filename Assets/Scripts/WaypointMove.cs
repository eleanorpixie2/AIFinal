using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointMove : MonoBehaviour
{
    public List<GameObject> set1;
    public List<GameObject> set2;
    public List<GameObject> set3;
    public List<GameObject> set4;
    public List<GameObject> set5;
    public List<GameObject> set6;
    public List<GameObject> set7;
    public List<GameObject> set8;
    public GameObject goal;
    public int speedRangeMax;

    private int setNumber;
    private Vector3 targetPoint;
    public int maxSpeed;
    private float curSpeed;
    private Rigidbody rgd;
    private float curTime = 0;
    private float second = 0;
    // Start is called before the first frame update
    void Start()
    {
        setNumber = 1;
        SetRunSpeed();
        rgd = GetComponent<Rigidbody>();
        rgd.drag = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.LookAt(targetPoint);
        curTime += Time.deltaTime;
        //If not at goal then speed up to max speed
        if (curSpeed <= maxSpeed && !GetComponent<Stats>().reachedGoal && curTime>=second+.45f)
        {
            curSpeed += 1f;
            second+=.45f;
        }
        rgd.velocity += transform.forward * curSpeed;

        //s=Vo*t + (a(t^2)/2)
        //t=(Vo/a) or (Vo/V1)/a
        //rgd.AddForce(new Vector3(-maxSpeed,0,0),ForceMode.Acceleration);
        if(transform.position.x== targetPoint.x
            || transform.position.x >= targetPoint.x)
        {
            setNumber++;
            SetNewTargetPoint();
        }
    }

    private void SetRunSpeed()
    {
        System.Random rnd = new System.Random(System.Guid.NewGuid().GetHashCode());
        maxSpeed = rnd.Next(15, speedRangeMax);
    }

    void SetNewTargetPoint()
    {
        System.Random rnd = new System.Random(System.Guid.NewGuid().GetHashCode());
        int setIndex = rnd.Next(0,4);
        switch(setNumber)
        {
            case 1:
                targetPoint = set1[setIndex].transform.position;
                break;
            case 2:
                targetPoint = set2[setIndex].transform.position;
                break;
            case 3:
                targetPoint = set3[setIndex].transform.position;
                break;
            case 4:
                targetPoint = set4[setIndex].transform.position;
                break;
            case 5:
                targetPoint = set5[setIndex].transform.position;
                break;
            case 6:
                targetPoint = set6[setIndex].transform.position;
                break;
            case 7:
                targetPoint = set7[setIndex].transform.position;
                break;
            case 8:
                targetPoint = set8[setIndex].transform.position;
                break;
            case 9:
                targetPoint = goal.transform.position;
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Goal")
        {
            GetComponent<Stats>().reachedGoal = true;
            curSpeed= 0;
        }
    }
}
