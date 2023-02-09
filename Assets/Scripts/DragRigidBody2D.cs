using UnityEngine;
using System.Collections;
using System;

/// <summary>
///     JON23:  It's a debugging script that helps to tweak NPC behaviour
///             Prbably some safeguarding from making it into production could help
///             For example adding if(Application.isEditor) or Conditional compilation
/// </summary>
public class DragRigidBody2D : MonoBehaviour
{
    // add this to your player. Make sure there is a rigidbody2d attached to it.
    private Vector2 direction;
    public float force = 1000;
    private Rigidbody2D r;

    Vector3 target;
    private bool dragging;

    void Start()
    {
        r = transform.GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        //if (Input.touchCount > 0)
        //{
        //    Debug.Log("asd");

        //    target = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
        //    direction = (target - transform.position).normalized;
        //    r.AddForce(direction * Time.deltaTime * force);
        //}

        if (dragging)
            Drag();

        //Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition).x);
    }

    private void Drag()
    {
        Vector3 mouse = Input.mousePosition;
        mouse.z = 5;
        Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(mouse);
        //mouseWorldPoint.z = 0;
        transform.position = mouseWorldPoint;
    }

    private void OnMouseUp()
    {
        dragging = false;
        r.isKinematic = false;
    }

    private void OnMouseDown()
    {
        dragging = true;

    }

}