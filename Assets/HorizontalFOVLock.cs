using UnityEngine;

// Lock the cameras horizontal field of view so it will frame the same view in the horizontal regardless of aspect ratio.

// JON23:   1. Needs to be moved from Assets to Assets/Scripts folder
//          2. The functiopnality better to be moved to CameraController

[RequireComponent(typeof(Camera))]
public class HorizontalFOVLock : MonoBehaviour
{

    public float fixedHorizontalFOV = 60;

    // JON23: clean-up
    Camera cam;

    void Awake()
    {
        // JON23: breaking this down into multiple lines would help readability
        GetComponent<Camera>().fieldOfView = 2 * Mathf.Atan(Mathf.Tan(fixedHorizontalFOV * Mathf.Deg2Rad * 0.5f) / GetComponent<Camera>().aspect) * Mathf.Rad2Deg;
    }
}