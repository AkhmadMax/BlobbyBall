using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 1;
    public float jumpVelocity = 1;

    public bool grounded = true;
    bool groundedLastFrame = true;
    Transform groundCheck;
    public LayerMask goundCheckMask;

    float fallMtpl = 2.5f;
    float jumpMult = 2.0f;

    Rigidbody2D rb;
    Animator animator;

    bool jumpInProcess;
    float jumpCounter = 5f / 60f;

    public enum Controls
    {
        Arrows,
        WASD
    }

    public enum Player
    {
        Player1,
        Player2
    }

    public Controls controls;

    NPC npc;
    public Vector2 initPos;
    bool jumpIsPressed;
    // Use this for initialization
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        groundCheck = transform.Find("GroundCheck");
        animator = GetComponent<Animator>();
        npc = GetComponent<NPC>();
    }

    private void Start()
    {
        initPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        groundedLastFrame = grounded;
        grounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, goundCheckMask);

        if(!npc)
        {
            if (controls == Controls.Arrows)
                jumpIsPressed = Input.GetKey(KeyCode.UpArrow);
            else
                jumpIsPressed = Input.GetKey(KeyCode.W);
        }
        

        if (rb.velocity.y < 0)
            rb.gravityScale = fallMtpl;
        else if (rb.velocity.y > 0 && !jumpIsPressed)
            rb.gravityScale = jumpMult;
        else
            rb.gravityScale = 1;

        if(controls == Controls.Arrows)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && grounded)
                Jump();
        }
        else if (controls == Controls.WASD)
        {
            if (Input.GetKeyDown(KeyCode.W) && grounded)
                Jump();
        }
        

        if (!groundedLastFrame && grounded)
            jumpInProcess = false;

        if (jumpInProcess && grounded)
        {
            jumpCounter -= Time.deltaTime;

            if (jumpCounter <= 0 && grounded)
            {
                rb.velocity = Vector2.up * jumpVelocity;
                jumpCounter = 5f / 60f;
            }
        }

        animator.SetBool("JumpInProcess", jumpInProcess);
        animator.SetBool("Grounded", grounded);
        animator.SetFloat("vSpeed", rb.velocity.y);

        float move = 0;

        if(controls == Controls.Arrows)
            move = Input.GetAxis("Horizontal");
        else if(controls == Controls.WASD)
            move = Input.GetAxis("HorizontalAD");

        if(!npc)
        rb.velocity = new Vector2(move * speed, rb.velocity.y);
    }

    public void MoveRight()
    {
        rb.velocity = new Vector2(1 * speed, rb.velocity.y);
    }

    public void MoveLeft()
    {
        rb.velocity = new Vector2(-1 * speed, rb.velocity.y);
    }

    public void Stop()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    public void Jump()
    {
        StopAllCoroutines();
        jumpInProcess = true;
        jumpIsPressed = false;
    }

    public void LongJump()
    {
        jumpInProcess = true;
        jumpIsPressed = true;

        //StartCoroutine(PressJump());
    }

    IEnumerator PressJump()
    {
        jumpIsPressed = true;
        yield return new WaitForSeconds(0.6f);
    }
}
