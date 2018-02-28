using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            if (instance == null)
                instance = FindObjectOfType<GameController>();

            return instance;
        }
    }

    private void Start()
    {
        StartNewGame(true, true);
    }

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

    public void StartNewGame(bool npc)
    {
        if (npc)
            StartNewGame(true, false);
        else
            StartNewGame(false, false);

    }

    public void StartNewGame(bool npc1, bool npc2)
    {
        Destroy(player1);
        Destroy(player2);

        if (npc1)
            player1 = Instantiate(npc, player1InitPos, Quaternion.identity);
        else
        {
            player1 = Instantiate(humanControlled, player1InitPos, Quaternion.identity);
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

    public void Game(PlayerController.Player player)
    {
        Debug.Log(player + " won");
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            WelcomeScreen.Instane.Show();
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
