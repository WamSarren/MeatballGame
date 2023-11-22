using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  // Reference to the player's transform
    public Vector3 offset = new Vector3(0.0f, 2.0f, -5.0f);  // Offset from the player

    void Update()
    {
        // Check if the target (player) is assigned
        if (target != null)
        {
            // Set the camera's position to the player's position + offset
            transform.position = target.position + offset;

            // Look at the player's position
            transform.LookAt(target.position);
        }
    }
}