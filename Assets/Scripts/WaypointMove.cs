using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    NavMeshAgent agent;
    private float velocity=0;
    // Start is called before the first frame update
    void Start()
    {
        setNumber = 1;
        SetRunSpeed();
        SetNewTargetPoint();
        rgd = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        agent.destination = targetPoint;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //If not at goal then speed up to max speed
        if (!GetComponent<Stats>().reachedGoal && agent.speed <= maxSpeed)
        {
            //    curSpeed =Mathf.SmoothDamp(transform.position.x,targetPoint.x, ref velocity,11,maxSpeed);
            //    transform.position = new Vector3(curSpeed, transform.position.y, transform.position.z);
            agent.speed += agent.acceleration;
            if (curSpeed < agent.speed)
                curSpeed = agent.speed;
        }
        else
        {
            agent.speed = 0;
        }

            //s=Vo*t + (a(t^2)/2)
            //t=(Vo/a) or (Vo/V1)/a
            //rgd.AddForce(new Vector3(-maxSpeed,0,0),ForceMode.Acceleration);
            if (transform.position.x== targetPoint.x
            || transform.position.x <= targetPoint.x || Mathf.Abs(transform.position.x-targetPoint.x)<2)
        {
            if (setNumber < 9)
            {
                setNumber++;
                SetNewTargetPoint();
                agent.destination = targetPoint;
                agent.speed = curSpeed;
            }
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
