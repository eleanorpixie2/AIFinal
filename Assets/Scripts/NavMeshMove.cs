using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class NavMeshMove : MonoBehaviour
{
    public Transform goal;
    NavMeshAgent agent;
    public int speedRangeMax;

    public int speed;
    public List<GameObject> wayPoints;
    private GameObject currentPoint;
    // Start is called before the first frame update
    void Start()
    {
        //intialize agent and goal point
        agent = GetComponent<NavMeshAgent>();
        goal.position = new Vector3(goal.position.x, transform.position.y, goal.transform.position.z);
        agent.destination = goal.position;
        agent.updateRotation=true;
        if (wayPoints.Count > 0)
        {
            wayPoints[0].transform.position= new Vector3(wayPoints[0].transform.position.x, transform.position.y, wayPoints[0].transform.position.z);
            agent.destination = wayPoints[0].transform.position;
            currentPoint = wayPoints[0];
        }
        
        //Set the max speed for this run, is random within a range every run
        SetRunSpeed();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //If not at goal then speed up to max speed
        if (agent.speed <= speed && !GetComponent<Stats>().reachedGoal)
        {
            agent.speed += agent.acceleration;
        }
        //if stopped, reset speed
        if(agent.speed==0 && !GetComponent<Stats>().reachedGoal)
        {
            agent.speed = speed;
        }
        transform.LookAt(agent.steeringTarget);
        //for curved track, set new goals
        if (wayPoints.Count>0)
        {
            //check distance
            if ((Mathf.Abs(currentPoint.transform.position.x) - Mathf.Abs(transform.position.x)) < 5)
            {
                int index = wayPoints.FindIndex(wy=>wy==currentPoint)+1;
                if(index<wayPoints.Count)
                {
                    currentPoint = wayPoints[index];
                    currentPoint.transform.position = new Vector3(currentPoint.transform.position.x, transform.position.y, currentPoint.transform.position.z);
                    agent.destination = currentPoint.transform.position;
                }
                //if index is more than the number of waypoints then set to goal
                else if( index>=wayPoints.Count)
                {
                    currentPoint = goal.gameObject;
                    goal.transform.position = new Vector3(goal.transform.position.x, transform.position.y, goal.transform.position.z);
                    agent.destination = goal.transform.position;
                }
                print(currentPoint.transform.position);
            }
            //if passed last way point then set to goal
            if(Mathf.Abs(transform.position.x)> Mathf.Abs(currentPoint.transform.position.x))
            {
                goal.transform.position = new Vector3(goal.transform.position.x, transform.position.y, goal.transform.position.z);
                agent.destination = goal.transform.position;
            }
        }
    }

    //set random max speed
    private void SetRunSpeed()
    {
        System.Random rnd = new System.Random(Guid.NewGuid().GetHashCode());
        speed = rnd.Next(15, speedRangeMax);
    }



    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Rail"&&!GetComponent<Stats>().reachedGoal)
        {
            agent.speed = 0;
        }
        else if(other.tag=="Goal")
        {
            agent.speed = 0;
            GetComponent<Stats>().reachedGoal = true;
            //agent.transform.position = new Vector3(other.transform.position.x-3, agent.transform.position.y, other.transform.position.z-3);
        }
        else if(other.tag=="Barrier")
        {
            agent.speed += 10;
        }
    }

}
