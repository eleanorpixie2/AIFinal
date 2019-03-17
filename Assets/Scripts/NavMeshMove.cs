using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshMove : MonoBehaviour
{
    public Transform goal;
    NavMeshAgent agent;
    public float speed;
    private bool reachedGoal;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        goal.position = new Vector3(goal.position.x, transform.position.y, goal.transform.position.z);
        agent.destination = goal.position;
        reachedGoal = false;
        //agent.updateRotation = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (agent.speed <= speed && !reachedGoal)
        {
            agent.speed += agent.acceleration;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Rail"&&!reachedGoal)
        {
            agent.speed = 0;
        }
        else if(other.tag=="Goal")
        {
            agent.speed = 0;
            reachedGoal = true;
            agent.transform.position = new Vector3(other.transform.position.x-3, agent.transform.position.y, other.transform.position.z-3);
        }
    }

}
