using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AStarMove : MonoBehaviour
{
    //the max that the speed can be
    public int speedRangeMax;
    //current way point
    private Vector3 targetPoint;
    //a* grid
    public Grid grid;
    //the max speed for the current run
    public int maxSpeed;
    //the current speed
    public float curSpeed;
    //the index of node in the path
    int nodeIndex;
    //acceleration
    float accel = 1.8f;
    //friction
    float inertia = 0.9f;
    //the current grid point
    private Transform currentGridPoint;
    //movement states
    public enum MovementState { Accelerating, Slowing, Stop };
    private MovementState _movementState = MovementState.Accelerating;
    // Start is called before the first frame update
    void Start()
    {
        SetRunSpeed();
        nodeIndex = 0;
        if (grid.path != null)
            targetPoint = grid.path[nodeIndex].worldPosition;
        //SetNewTargetPoint();
    }

    // Update is called once per frame
    void Update()
    {
        if(targetPoint==null)
            targetPoint = grid.path[nodeIndex].worldPosition;
        //call appropriate method based on movement state
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
            if (grid.path != null && grid.path.Count>0)
            {
                i = grid.path.Count - 1;
                if (targetPoint != grid.path[i].worldPosition && !GetComponent<Stats>().reachedGoal)
                {
                    SetNewTargetPoint();
                }
            }
        }
    }

    public void Accell()
    {
        //calculate velocity
        curSpeed += Time.deltaTime * accel;
        //move car
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, curSpeed * Time.deltaTime);
        //look at goal
        transform.LookAt(targetPoint, transform.up);
        //set rotation
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        //make sure speed doesn't go past the max
        if (curSpeed > maxSpeed)
        {

            curSpeed = maxSpeed;
        }

    }
    //deccelerate the car
    public void Slow()
    {
        //apply a breaking friction
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
        maxSpeed = rnd.Next(18, speedRangeMax);
    }

    //set new way point from path
    void SetNewTargetPoint()
    {
        nodeIndex++;
        if (grid.path != null && nodeIndex<grid.path.Count)
            targetPoint = grid.path[nodeIndex].worldPosition;
        //make sure that at least the goal is set
        else if(nodeIndex>=grid.path.Count)
        {
            targetPoint = grid.path[grid.path.Count - 1].worldPosition;
        }
    }

    //stop after reaching the goal
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
