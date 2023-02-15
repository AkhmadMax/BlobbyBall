using System.Collections;
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

    void Play()
    {
        Vector2 toBall = Vector2.zero;
        if (playSide == Side.Left) toBall = ball.position - transform.position;
        if (playSide == Side.Right) toBall = transform.position - ball.position;

        if (Mathf.Abs(ball.position.x) > nearNetZone)
        {
            AdjustPositionOrJump(toBall.x, lowHitRange);
        }
        else
        {
            AdjustPositionOrJump(toBall.x, highHitRange);
        }
    }

    void AdjustPositionOrJump(float targetPosition, PositionRange positioning)
    {
        if (positioning.Min() <= targetPosition && targetPosition <= positioning.Max())
        {
            Jump();
        }
        else if (targetPosition > positioning.Max()) MoveForward();
        else if (targetPosition < positioning.Min()) MoveBackward();
    }

    void Update()
    {
        switch(playSide)
        {
            case Side.Left:
                if (ball.position.x > 0) // if the ball on enemy's side then move to initPos
                {
                    ApproachReceivingPosition();
                }
                else // move towards the ball
                {
                    Play();
                }
                break;
            case Side.Right:
                if (ball.position.x < 0) // if the ball on enemy's side then move to initPos
                {
                    ApproachReceivingPosition();
                }
                else // move towards the ball
                {
                    Play();
                }
                break;
        }
    }
}
