using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  // Reference to the player's transform
    public Vector3 offset = new Vector3(0.0f, 2.0f, -5.0f);  // Offset from the player
    public float minDistance = 5.0f;  // Minimum distance from the player
    public float maxDistance = 10.0f; // Maximum distance from the player

    void Update()
    {
        // Check if the target (player) is assigned
        if (target != null)
        {
            // Calculate the total height of the cube stack
            float stackHeight = CubeStackUtility.CalculateStackHeight(target.gameObject); // Assuming the player carries the stack

            // Calculate the desired distance based on the stack height
            float desiredDistance = Mathf.Clamp(stackHeight * 0.5f, minDistance, maxDistance);

            // Set the camera's position to the player's position + offset and adjust the distance
            transform.position = target.position + offset.normalized * desiredDistance;

            // Look at the player's position
            transform.LookAt(target.position);
        }
    }
}