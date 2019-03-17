using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public enum PlayerNumber { Player1,Player2,Player3,Player4};
    public PlayerNumber _playerNumber;
    public float lapTime;
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
