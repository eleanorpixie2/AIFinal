using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AStarMove : MonoBehaviour
{

    public int speedRangeMax;
    private Vector3 targetPoint;
    public Grid grid;
    public int maxSpeed;
    public float curSpeed;
    int nodeIndex;
    float accel = 1.8f;
    float inertia = 0.9f;
    public enum MovementState { Accelerating, Slowing, Stop };
    private MovementState _movementState = MovementState.Accelerating;
    // Start is called before the first frame update
    void Start()
    {
        SetRunSpeed();
        nodeIndex = 0;
        if (grid.finalPath != null)
            targetPoint = grid.finalPath[nodeIndex].position;
        SetNewTargetPoint();
    }

    // Update is called once per frame
    void Update()
    {
        if(targetPoint==null)
            targetPoint = grid.finalPath[nodeIndex].position;

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
        if ((Mathf.Abs(targetPoint.x) - Mathf.Abs(transform.position.x))<=0)
        {
            int i=0;
            if (grid.finalPath != null)
            {
                i = grid.finalPath.Count - 1;
                if (targetPoint != grid.finalPath[i].position)
                {
                    SetNewTargetPoint();
                }
            }
        }
    }

    public void Accell()
    {

        curSpeed += Time.deltaTime * accel;
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
        nodeIndex++;
        if (grid.finalPath != null)
            targetPoint = grid.finalPath[nodeIndex].position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Goal")
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
