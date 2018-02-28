using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
