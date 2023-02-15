using UnityEngine;

public class NPC : MonoBehaviour
{
    #region NPC Behaviour Parameters
    // The parameters can be used to create different types of NPC playstyle
    private const float nearNetZone = 0.5f;
    private const float tolerance = 0.1f;
    private const float lowHitOffset = 0.25f;
    private const float highHitOffset = 0.15f;
    private const float receivingPositionOffset = -0.5f;
    #endregion

    // References to externall classes
    private Transform ball;
    private PlayerController controller;

    // Variables initialized during runtime
    private PlaySide playSide;
    private float receivingPosition;
    private Vector2 forwardVector;
    private PositionRange highHitRange;
    private PositionRange lowHitRange;

    enum PlaySide
    {
        Left,
        Right
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        controller = GetComponent<PlayerController>();
        ball = BallBehaviour.Instance.transform;

        playSide = controller.initPos.x < 0 ? PlaySide.Left : PlaySide.Right;
        forwardVector = playSide == PlaySide.Left ? Vector2.right : Vector2.left;

        highHitRange = new PositionRange(highHitOffset, tolerance);
        lowHitRange = new PositionRange(lowHitOffset, tolerance);

        receivingPosition = controller.initPos.x + receivingPositionOffset * forwardVector.x;
    }

    void Update()
    {
        if (BallOnEnemySide()) // if the ball on enemy's side then improve position to receive it
        {
            ApproachReceivingPosition();
        }
        else // move towards the ball and hit
        {
            Play();
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

    void Play()
    {
        float toBall = GetDistanceToBall();

        if (Mathf.Abs(ball.position.x) > nearNetZone)
        {
            AdjustPositionOrJump(toBall, lowHitRange);
        }
        else
        {
            AdjustPositionOrJump(toBall, highHitRange);
        }
    }

    void AdjustPositionOrJump(float displacement, PositionRange positioning)
    {
        if (positioning.Min() <= displacement && displacement <= positioning.Max())
        {
            Jump();
        }
        else if (displacement > positioning.Max()) MoveForward();
        else if (displacement < positioning.Min()) MoveBackward();
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
        if (playSide == PlaySide.Left)
            controller.MoveRight();

        if (playSide == PlaySide.Right)
            controller.MoveLeft();
    }

    void MoveBackward()
    {
        if (playSide == PlaySide.Left)
            controller.MoveLeft();

        if (playSide == PlaySide.Right)
            controller.MoveRight();
    }

    bool BallOnEnemySide()
    {
        float subjectiveBallPosition = ball.position.x * forwardVector.x;
        if (subjectiveBallPosition > 0)
            return true;
        return false;
    }

    float GetDistanceToBall()
    {
        float npcHPos = transform.position.x;
        float ballHPos = ball.position.x;
        float directedDistance = (ballHPos - npcHPos) * forwardVector.x;
        return directedDistance;
    }

    public struct PositionRange
    {
        float position;
        float tolerance;

        public PositionRange(float position, float tolerance)
        {
            this.position = position;
            this.tolerance = tolerance / 2f;
        }
        public float Min() { return this.position - this.tolerance; }
        public float Max() { return this.position + this.tolerance; }
    }
}
