using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallsGenerator : MonoBehaviour
{
    public GameObject ballPrefab;
    float counter = 2;
    GameObject currentBall;

    Vector3 ballInitPosP1 = new Vector3(-1.2f, 1.5f, 0);
    Vector3 ballInitPosP2 = new Vector3(1.2f, 1.5f, 0);

    private void Update()
    {
        //counter -= Time.deltaTime;
        //if(counter <= 0)
        //{
        //    GameObject.Destroy(currentBall);
        //    currentBall = Instantiate(ballPrefab, transform.position, transform.rotation);
        //    currentBall.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(0.6f, 1f), Random.Range(0.7f, 1f)) * Random.Range(50, 70));
        //    counter = 3;
        //}
    }

    private void Start()
    {
        SetBallForPlayer1();
    }

    public void SetBallForPlayer1()
    {
        GameObject.Destroy(currentBall);
        currentBall = Instantiate(ballPrefab, ballInitPosP1, transform.rotation);
    }

    public void SetBallForPlayer2()
    {
        GameObject.Destroy(currentBall);
        currentBall = Instantiate(ballPrefab, ballInitPosP2, transform.rotation);
    }
}
