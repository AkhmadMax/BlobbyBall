using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     JON23:  1. Name of the class does not reflect its content fully. The class basically implements AI of NPC
///             2. The code is too rigid to extentions and not reactive to modifications of the scene it acts in
///             3. The NPC would break if a second ball would be introduced to the scene
/// </summary>
public class NPC : MonoBehaviour {

    Transform ball;
    PlayerController controller;

    void Awake()
    {
        controller = GetComponent<PlayerController>();
    }

    private void Start()
    {
        ball = BallBehaviour.Instance.transform;
    }

    /// <summary>
    ///    JON23:   1.  The two blocks of code for left and right side logic is basically a copy-paste . It contradicts DRY principle
    ///             2.  Throwing code directly into Update methods is bad for code readability. What happens there exactly?
    ///             3.  The code need to be decomposed at least to several methods
    ///             4.  I see I've put comments to help myself navigate in the code. Comments tend to outdate, instead I should have just split the code into smaller pieces
    ///             5.  There are arbitrary numbers like 0.2, 0.3, 0.5 without any name or meaning. It drastically decreases quality of the code. Should be replaced with meaningful variables
    ///                 I can see that these mystic numbers depend on the size of the rigidbody. It means that if the player size changes this values will stop working as expected
    /// </summary>
    void Update()
    {

        if(controller.initPos.x < 0) // if NPC plays on left field
        {
            Vector2 toBall = ball.position - transform.position;

            float wantedPosX;
            if (ball.position.x > 0) // if the ball on enemy's side then move to initPos
            {
                wantedPosX = controller.initPos.x - 0.5f;

                if (transform.position.x - wantedPosX < 0.1 && transform.position.x - wantedPosX > 0)
                    controller.Stop();
                else if (transform.position.x - wantedPosX > 0)
                    controller.MoveLeft();
                else if (transform.position.x - wantedPosX < 0)
                    controller.MoveRight();
            }
            else // move towards the ball
            {
                if (ball.position.x < -0.5)
                {
                    if (toBall.x <= 0.3f && toBall.x >= 0.2f)
                    {
                        controller.Stop();
                        if (ball.position.y < 1.5)
                            controller.Jump();
                        else
                            controller.LongJump();
                    }
                    else if (toBall.x > 0.2)
                        controller.MoveRight();
                    else if (toBall.x < 0.3)
                        controller.MoveLeft();
                }
                else
                {
                    if (toBall.x <= 0.2f && toBall.x >= 0.1f)
                    {
                        controller.Stop();
                        if (ball.position.y < 1.5)
                            controller.Jump();
                        else
                            controller.LongJump();
                    }
                    else if (toBall.x > 0.1)
                        controller.MoveRight();
                    else if (toBall.x < 0.2)
                        controller.MoveLeft();
                }
            }
        }
        else // if NPC plays on the right side
        {
            Vector2 toBall = transform.position - ball.position;

            float wantedPosX;
            if (ball.position.x < 0) // if the ball on enemy's side then move to initPos
            {
                wantedPosX = controller.initPos.x + 0.5f;

                if (transform.position.x - wantedPosX < 0.1 && transform.position.x - wantedPosX > 0)
                    controller.Stop();
                else if (transform.position.x - wantedPosX > 0)
                    controller.MoveLeft();
                else if (transform.position.x - wantedPosX < 0)
                    controller.MoveRight();
            }
            else // move towards the ball
            {
                if (ball.position.x > 0.5)
                {
                    if (toBall.x <= 0.3f && toBall.x >= 0.2f)
                    {
                        controller.Stop();
                        if (ball.position.y < 1.5)
                            controller.Jump();
                        else
                            controller.LongJump();
                    }
                    else if (toBall.x > 0.2)
                        controller.MoveLeft();
                    else if (toBall.x < 0.3)
                        controller.MoveRight();
                }
                else
                {
                    if (toBall.x <= 0.2f && toBall.x >= 0.1f)
                    {
                        controller.Stop();
                        if (ball.position.y < 1.5)
                            controller.Jump();
                        else
                            controller.LongJump();
                    }
                    else if (toBall.x > 0.1)
                        controller.MoveLeft();
                    else if (toBall.x < 0.2)
                        controller.MoveRight();
                }
            }
        }

    }
}
