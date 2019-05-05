using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaypointMove : MonoBehaviour
{
    //waypoint sets
    public List<GameObject> set1;
    public List<GameObject> set2;
    public List<GameObject> set3;
    public List<GameObject> set4;
    public List<GameObject> set5;
    public List<GameObject> set6;
    public List<GameObject> set7;
    public List<GameObject> set8;
    //goal
    public GameObject goal;
    //max that the speed can be
    public int speedRangeMax;

    //the current set of way points
    private int setNumber;
    //the current waypoint
    public Transform targetPoint;
    //the max speed for the current run
    public int maxSpeed;
    //the current speed
    public float curSpeed;
    //the amount to accelerate
    float accel = 1.8f;
    //the friction to apply when breaking
    float inertia = 0.9f;
    //states of the car moving
    public enum MovementState { Accelerating,Slowing,Stop};
    //the starting state
    private MovementState _movementState = MovementState.Accelerating;
    // Start is called before the first frame update
    void Start()
    {
        setNumber = 1;
        SetRunSpeed();
        SetNewTargetPoint();

    }

    // Update is called once per frame
    void Update()
    {
        //call appropriate method based on state
        switch (_movementState)
        {
            case MovementState.Accelerating:
                Accell();
                break;
            case MovementState.Slowing:
                Slow();
                break;
            case MovementState.Stop:
                curSpeed = 0;
                break;
        }
        //s=Vo*t + (a(t^2)/2)
        //t=(Vo/a) or (Vo/V1)/a
        //if close, set new waypoint
        if ((Mathf.Abs(targetPoint.position.x) - Mathf.Abs(transform.position.x)) < 3)
        {
            if (setNumber < 9)
            {
                setNumber++;
                SetNewTargetPoint();
            }
        }
    }

    public void Accell()
    {
        //calculate the velocity
        curSpeed+=Time.deltaTime*accel;
        //move the car
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, curSpeed * Time.deltaTime);
        //rotate towards the current point
        if (targetPoint.rotation.y != 0)
            transform.LookAt(targetPoint, transform.up);
        //make sure speed doesn't go over the max
        if (curSpeed > maxSpeed)
        {
            
            curSpeed = maxSpeed;
        }

    }
    //slow the car down with friction
    public void Slow()
    {

        curSpeed *= inertia;
        transform.Translate(Time.deltaTime * curSpeed, 0, 0);

        if (curSpeed <= 1.0)
        {
            curSpeed = 0;
        }
    }

    //set random max speed
    private void SetRunSpeed()
    {
        System.Random rnd = new System.Random(System.Guid.NewGuid().GetHashCode());
        maxSpeed = rnd.Next(15, speedRangeMax);
    }

    //set new waypoint randomly from current set
    void SetNewTargetPoint()
    {
        System.Random rnd = new System.Random(System.Guid.NewGuid().GetHashCode());
        int setIndex = rnd.Next(0,4);
        switch(setNumber)
        {
            case 1:
                targetPoint = set1[setIndex].transform;
                break;
            case 2:
                targetPoint = set2[setIndex].transform;
                break;
            case 3:
                targetPoint = set3[setIndex].transform;
                break;
            case 4:
                targetPoint = set4[setIndex].transform;
                break;
            case 5:
                targetPoint = set5[setIndex].transform;
                break;
            case 6:
                targetPoint = set6[setIndex].transform;
                break;
            case 7:
                targetPoint = set7[setIndex].transform;
                break;
            case 8:
                targetPoint = set8[setIndex].transform;
                break;
            case 9:
                targetPoint = goal.transform;
                break;
        }
    }

    //slow if the goal has been reached
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Goal")
        {
            GetComponent<Stats>().reachedGoal = true;
            _movementState = MovementState.Slowing;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Goal")
        {
            _movementState = MovementState.Stop;
        }
    }
}
