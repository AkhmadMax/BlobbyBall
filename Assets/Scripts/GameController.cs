using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///     JON23:  1. Rename the class to GameManager
/// </summary>
public class GameController : MonoBehaviour {

    public Text scoreP1Label;
    public Text scoreP2Label;

    int scoreP1 = 0;
    int scoreP2 = 0;

    int pointsToWin = 11;

    static GameController instance;

    public GameObject npc;
    public GameObject humanControlled;

    private GameObject player1;
    private GameObject player2;

    public Material player1Mtl;
    public Material player2Mtl;

    Vector3 player1InitPos = new Vector3(-1.2f, 0, 0);
    Vector3 player2InitPos = new Vector3(1.2f, 0, 0);

    public static GameController Instance
    {
        get 
        {
            // JON23: Add some error handling for cases when GameController is not present in the scene at all
            if (instance == null)
                instance = FindObjectOfType<GameController>();

            return instance;
        }
    }

    private void Start()
    {
        StartNewGame(true, true);
    }

    // JON23: Reneme to AssignPoint
    public void Point(PlayerController.Player player)
    {
        switch(player)
        {
            case PlayerController.Player.Player1:
                scoreP1++;
                if (scoreP1 == pointsToWin)
                    Game(player);
                NewRound(player);
                scoreP1Label.text = scoreP1.ToString();
                break;
            case PlayerController.Player.Player2:
                scoreP2++;
                if (scoreP2 == pointsToWin)
                    Game(player);
                NewRound(player);
                scoreP2Label.text = scoreP2.ToString();
                break;
        }
    }

    // JON23:   This method is called directly from OnClick() handler in UI
    //          Also the same button directly calls UI screen to hide
    //          To my taste it would be bettwe if GameManager would hadle it alltogether by itself
    public void StartNewGame(bool npc)
    {
        if (npc)
            StartNewGame(true, false);
        else
            StartNewGame(false, false);

    }

    public void StartNewGame(bool npc1, bool npc2)
    {
        // JON23:   Destroying and Instantiaton comparably heavy operations that could lead to hickups
        //          In this case it doesn't matter, but if the character prefabs become more complex it can start affect performance
        Destroy(player1);
        Destroy(player2);

        if (npc1)
            player1 = Instantiate(npc, player1InitPos, Quaternion.identity);
        else
        {
            player1 = Instantiate(humanControlled, player1InitPos, Quaternion.identity);
            // JON23: Ideally it's better to avoid calling GetComponent in runtime and access cached references
            player1.GetComponent<PlayerController>().controls = PlayerController.Controls.WASD;
            player1.GetComponent<Renderer>().material = player1Mtl;
        }

        if (npc2)
            player2 = Instantiate(npc, player2InitPos, Quaternion.identity);
        else
        {
            player2 = Instantiate(humanControlled, player2InitPos, Quaternion.identity);
            player2.GetComponent<Renderer>().material = player2Mtl;
        }

        scoreP1 = 0;
        scoreP2 = 0;

        NewRound(PlayerController.Player.Player1);
    }

    private void NewRound(PlayerController.Player player)
    {
        BallBehaviour.Instance.ResetPosition(player);
    }

    // JON23: Obviously it is just unfinished logic, the game is endless at this point
    public void Game(PlayerController.Player player)
    {
        Debug.Log(player + " won");
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            // Better to wrap into a method PauseGame, many things can be called when game is paused along with Showing the UI Panel
            WelcomeScreen.Instane.Show();
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
