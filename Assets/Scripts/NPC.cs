﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {

    private Transform ball;
    private PlayerController controller;
    private float receivingPosition;
    private Vector2 forwardVector;

    private float ReceivingPosition { get { return receivingPosition; } }

    // NPC parameters that affect its behaviour
    private const float receivingPositionOffset = 0.5f;
    private const float nearNetZone = 0.5f;
    private const float tolerance = 0.1f;
    private const float lowHitOffset = 0.25f;
    private const float highHitOffset = 0.15f;

    public struct PositionRange
    {
        float position;
        float tolerance;

        public PositionRange(float position, float tolerance)
        {
            this.position = position;
            this.tolerance = tolerance / 2f;
        }
        public float Min()  {   return this.position - this.tolerance;  }
        public float Max()  {   return this.position + this.tolerance;  }
    }

    private PositionRange highHitRange;
    private PositionRange lowHitRange;

    enum Side {
        Left,
        Right
    }

    private Side playSide;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        controller = GetComponent<PlayerController>();
        ball = BallBehaviour.Instance.transform;

        playSide = controller.initPos.x < 0 ? Side.Left : Side.Right;
        forwardVector = playSide == Side.Left ? Vector2.right : Vector2.left;

        highHitRange = new PositionRange(highHitOffset, tolerance);
        lowHitRange = new PositionRange(lowHitOffset, tolerance);

        SetRecevingPosition(playSide);
    }

    void SetRecevingPosition(Side playSide)
    {
        switch (playSide)
        {
            case Side.Left:
                receivingPosition = controller.initPos.x - receivingPositionOffset;
                break;
            case Side.Right:
                receivingPosition = controller.initPos.x + receivingPositionOffset;
                break;
        }
    }

    private void ApproachReceivingPosition()
    {
        float displacement = transform.position.x - receivingPosition;


        if (Mathf.Abs(displacement) < tolerance)
        {
            controller.Stop();
        }
        else
        {
            if (displacement > 0) controller.MoveLeft();
            if (displacement < 0) controller.MoveRight();
        }
    }

    void Jump()
    {
        controller.Stop();
        if (ball.position.y < 1.5)
            controller.Jump();
        else
            controller.LongJump();
    }

    void MoveForward()
    {
        if (playSide == Side.Left)
            controller.MoveRight();
        
        if(playSide == Side.Right) 
            controller.MoveLeft();
    }

    void MoveBackward()
    {
        if (playSide == Side.Left)
            controller.MoveLeft();

        if (playSide == Side.Right)
            controller.MoveRight();
    }

    void AdjustPosition()
    {
        Vector2 toBall = ball.position - transform.position;

        if (Mathf.Abs(ball.position.x) > nearNetZone)
        {
            if (lowHitRange.Min() <= toBall.x && toBall.x <= lowHitRange.Max())
            {
                Jump();
            }
            else if (toBall.x > lowHitRange.Max()) MoveForward();
            else if (toBall.x < lowHitRange.Min()) MoveBackward();
        }
        else
        {
            if (0.1f <= toBall.x && toBall.x <= 0.2f)
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

    void Update()
    {
        if(controller.initPos.x < 0) // if NPC plays on left field
        {

            if (ball.position.x > 0) // if the ball on enemy's side then move to initPos
            {
                ApproachReceivingPosition();
            }
            else // move towards the ball
            {
                AdjustPosition();
            }
        }
        else // if NPC plays on the right side
        {
            Vector2 toBall = transform.position - ball.position;


            if (ball.position.x < 0) // if the ball on enemy's side then move to initPos
            {
                ApproachReceivingPosition();
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
