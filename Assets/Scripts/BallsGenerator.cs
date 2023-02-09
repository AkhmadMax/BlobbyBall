using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     JON23: The class can be removed and delegate its responsibilities to GameManager
/// </summary>
public class BallsGenerator : MonoBehaviour
{
    // JON23: Use [SerializeField] instead of making the variable public
    public GameObject ballPrefab;
    float counter = 2;
    GameObject currentBall;

    // JON23: duplicates with BallBehaviour, need to have only one source of true
    Vector3 ballInitPosP1 = new Vector3(-1.2f, 1.5f, 0);
    Vector3 ballInitPosP2 = new Vector3(1.2f, 1.5f, 0);

    // JON23: Clean up
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

    // JON23: SetBallForPlayer1 and SetBallForPlayer2 are just two same methods that should be one. ballInitPos can be an argument for SetBall method
    public void SetBallForPlayer1()
    {
        GameObject.Destroy(currentBall);
        currentBall = Instantiate(ballPrefab, ballInitPosP1, transform.rotation);
    }

    // JONS23: This method is never called
    public void SetBallForPlayer2()
    {
        GameObject.Destroy(currentBall);
        currentBall = Instantiate(ballPrefab, ballInitPosP2, transform.rotation);
    }
}
