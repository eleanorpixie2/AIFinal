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
    public float curSpeed;
    float accel = 1.8f;
    float inertia = 0.9f;
    public enum MovementState { Accelerating,Slowing,Stop};
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
        if ((Mathf.Abs(targetPoint.x) - Mathf.Abs(transform.position.x)) < 5)
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

        curSpeed+=Time.deltaTime*accel;
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, curSpeed * Time.deltaTime);
        if (curSpeed > maxSpeed)
        {
            curSpeed = maxSpeed;
        }

    }
    public void Slow()
    {

        curSpeed *= inertia;
        transform.Translate(Time.deltaTime * curSpeed, 0, 0);

        if (curSpeed <= 1.0)
        {
            curSpeed = 0;
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
