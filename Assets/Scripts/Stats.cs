using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    //player numbers
    public enum PlayerNumber { Yellow,Red,Green,Blue};
    public PlayerNumber _playerNumber;
    // the time of the lap
    public float lapTime;
    //checks if the car has reached the goal
    public bool reachedGoal;
    // Start is called before the first frame update
    void Start()
    {
        reachedGoal = false;
        TrackLapTime();
    }

    // Update is called once per frame
    void Update()
    {
        if(!reachedGoal)
        {
            TrackLapTime();
        }
    }

    private void TrackLapTime()
    {
        lapTime += Time.deltaTime;
    }
}
