using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     JON23:  1. There is no abstract class or interface for a player entity.
///             2. PlayerController shouldn't know anything about NPC (Loose coupling)
///             3. Maybe a BlobCharacter abstract class could be a base class, and Player and NPC could inherit from it
///         
/// </summary>
public class PlayerController : MonoBehaviour
{
    // JONS23: For better incapsulation theses variables should be set to private, and exposed in Unity Inspector using [SerializeField] attribute
    public float speed = 1;
    public float jumpVelocity = 1;

    // JON23: this variable should be just private
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

    // JON23: Input handling should go to a separate InputController class
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

    // JON23: PlayerController shouldnt touch NPC
    NPC npc;
    
    
    public Vector2 initPos;

    // JON23: It is strange that this condition is in PlayerController. It should be somewhere else, but not here
    bool jumpIsPressed;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // JON23: This is very bad, easily brekable and hard to trace the issue later on. Even simple "if(groundCheck)" is not present
         groundCheck = transform.Find("GroundCheck");

        animator = GetComponent<Animator>();
        npc = GetComponent<NPC>();
    }

    private void Start()
    {
        initPos = transform.position;
    }

    /// <summary>
    ///     JON23:  1.  The method is just too big and contains too much hardcoded logic. It urges for decomposition.
    ///             2.  Code should be wrapped into a method or methods, instead of directly writing it in FixedUpdate
    ///             3.  The series of methods like MoveLeft(), MoveRight(), Jump() are part of the class but never called from it, instead the are called from NPC script.
    ///                 These methods can go to Blob Character base class and can be utilized by both Player and NPC scripts
    ///             
    /// </summary>
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
                // JON23: Mystic numnbers need to be sustituted with meaningful variables or constants
                jumpCounter = 5f / 60f;
            }
        }

        // JON23: This logic can go to base Blob character class
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
        // JON23: Redundant code
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

    // JON23: Unused methods need to be deleted
    IEnumerator PressJump()
    {
        jumpIsPressed = true;
        yield return new WaitForSeconds(0.6f);
    }
}
