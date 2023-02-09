using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private void Update()
    {
        if(BallBehaviour.Instance)
        {
            float wantedXpos = BallBehaviour.Instance.transform.position.x / 8f;
            Vector3 currentPos = transform.position;
            // JON23: introduce variable for the float number
            currentPos.x = Mathf.Lerp(currentPos.x, wantedXpos, 0.05f);
            transform.position = currentPos;

        }
    }
}
