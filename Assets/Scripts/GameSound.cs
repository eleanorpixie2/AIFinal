using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSound : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectsWithTag("GameSound").Length>=1)
        {
            if (GameObject.FindGameObjectWithTag("MasterGameSound") == null)
            {
                DontDestroyOnLoad(this);
                gameObject.tag = "MasterGameSound";
            }
            else
            {
                if (gameObject.tag == "MasterGameSound")
                    Destroy(GameObject.FindGameObjectWithTag("GameSound"));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
