using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReachedGoal : MonoBehaviour
{
    public Text whoWonText;
    private bool someoneWon;
    // Start is called before the first frame update
    void Start()
    {
        someoneWon = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player"&&!someoneWon)
        {
            someoneWon = true;
            whoWonText.text=other.GetComponent<Stats>()._playerNumber.ToString()
                +" won!\n Lap Time:"+ other.GetComponent<Stats>().lapTime;
        }
    }
}
