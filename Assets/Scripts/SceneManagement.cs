using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagement : MonoBehaviour
{
   //Buttons used in menu
    public Button StartButton;
    public Button ExitButton;

    public enum Tracks { Straight,Curved};
    private Tracks currentTrack = Tracks.Straight;
    private GameObject gameSound;
    // Use this for initialization
    void Start () {
        //dont destroy this game object
        DontDestroyOnLoad(this);

        //set default resolution
        Camera.main.aspect = (16f / 9f);
        //create code for buttons, buttons only work if there is an object attached to it
        if (StartButton != null)
        {
            Button btn = StartButton.GetComponent<Button>();
            btn.onClick.AddListener(TaskOnClick5);
        }
        if (ExitButton != null)
        {
            Button btn1 = ExitButton.GetComponent<Button>();
            btn1.onClick.AddListener(TaskOnClick1);
        }

        if(GameObject.FindGameObjectsWithTag("Scene").Length>=2)
        {
            Destroy(GameObject.FindGameObjectsWithTag("Scene")[0]);
        }
    }

    bool loadNextScene = false;
	// Update is called once per frame
	void Update ()
    {
        if (gameSound == null)
            gameSound = GameObject.FindGameObjectWithTag("MasterGameSound");
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnToMenu();
        }
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if(players!=null && players.Length>0)
        {
            foreach(GameObject p in players)
            {
                if(p.GetComponent<Stats>().reachedGoal)
                {
                    loadNextScene = true;
                }
                else
                {
                    loadNextScene = false;
                }
            }
            if(loadNextScene)
            {
                if (WaitForNextRound())
                {
                    switch (currentTrack)
                    {
                        case Tracks.Straight:
                            CurvedTrack();
                            break;
                        case Tracks.Curved:
                            StraightTrack();
                            break;
                    }
                }
            }
        }
	}

    int waitTime = 3;
    float currentTime = 0;
    bool WaitForNextRound()
    {
        if(currentTime>=waitTime)
        {
            currentTime = 0;
            return true;
        }
        else
        {
            currentTime += Time.deltaTime;
            return false;
        }
    }

    //exits game
    void TaskOnClick1()
    {
        Application.Quit();
    }

    //returns to start menu
    void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
        currentTrack = Tracks.Straight;
        if (gameSound != null)
        {
            Destroy(gameSound);
        }
    }


    //loads game
    public void TaskOnClick5()
    {
        SceneManager.LoadScene("SampleScene");
        currentTrack = Tracks.Straight;
    }


    //loads win scene
    public void StraightTrack()
    {
        SceneManager.LoadScene("SampleScene");
        currentTrack = Tracks.Straight;
    }
    //loads gameover scene
    public void CurvedTrack()
    {
        SceneManager.LoadScene("CurvedTrack");
        currentTrack = Tracks.Curved;
    }
}
