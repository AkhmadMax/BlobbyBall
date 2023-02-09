using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     JON23:  1.  The class directly assign points via GameController. 
///                 Would be better to fire an event when the ball hits the ground, 
///                 and GameManager and other classes could subscribe to it and do what they need
///             2.  The class is a Singleton. But what if we have multiple balls in the arcade mode of the game?
///                 Using a Singleton pattern is not justified here
/// </summary>
public class BallBehaviour : MonoBehaviour
{
    public AudioClip[] hit;
    AudioSource source;

    private static BallBehaviour _instance;
    Rigidbody2D rb;

    // JON23: Clean-up
    Collider2D _collider;

    Vector3 ballInitPosP1 = new Vector3(-1.2f, 1.5f, 0);
    Vector3 ballInitPosP2 = new Vector3(1.2f, 1.5f, 0);

    public static BallBehaviour Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<BallBehaviour>();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        rb.gravityScale = 0;
        transform.position = ballInitPosP1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.name == "fieldP1")
        {
            GameController.Instance.Point(PlayerController.Player.Player2);
        }

        if (collision.collider.name == "fieldP2")
        {
            GameController.Instance.Point(PlayerController.Player.Player1);
        }

        if (collision.rigidbody && collision.rigidbody.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerController player = collision.rigidbody.gameObject.GetComponent<PlayerController>();
            rb.velocity = Vector3.zero;
            //rb.angularVelocity = 0;
            rb.gravityScale = 1;
            rb.AddForce(collision.contacts[0].normal * (player.grounded ? 50 : 55));
        }

        source.PlayOneShot(hit[Random.Range(0, hit.Length - 1)]);
    }

    public void ResetPosition(PlayerController.Player player)
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0;
        rb.gravityScale = 0;


        switch (player)
        {
            case PlayerController.Player.Player1:
                transform.position = ballInitPosP1;
                break;
            case PlayerController.Player.Player2:
                transform.position = ballInitPosP2;
                break;
        }
    }
}
