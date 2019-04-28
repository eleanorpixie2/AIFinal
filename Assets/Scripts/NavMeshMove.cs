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
    public WaypointMove wayPoint;
    // Start is called before the first frame update
    void Start()
    {
        //intialize agent and goal point
        agent = GetComponent<NavMeshAgent>();
        goal.position = new Vector3(goal.position.x, transform.position.y, goal.transform.position.z);
        agent.destination = goal.position;
        agent.updateRotation=true;
        
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
    }

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
    }

}
