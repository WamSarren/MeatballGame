using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  // Reference to the player's transform
    public float minFOV = 60.0f;  // Minimum field of view
    public float maxFOV = 90.0f; // Maximum field of view
    public float heightMultiplier = 0.5f;  // Multiplier to control the effect of stack height on FOV

    private Camera playerCamera;
    private float originalFOV;

    void Start()
    {
        // Get the Camera component attached to the player
        playerCamera = target.GetComponentInChildren<Camera>();

        // Check if the Camera component exists on the player or its children
        if (playerCamera == null)
        {
            Debug.LogError("Camera component not found on the player or its children.");
        }
        else
        {
            // Store the original field of view
            originalFOV = playerCamera.fieldOfView;
        }
    }

    void Update()
    {
        // Check if the target (player) is assigned
        if (target != null)
        {
            // Calculate the total height of the cube stack
            float stackHeight = CubeStackUtility.CalculateStackHeight(target.gameObject); // Assuming the player carries the stack

            // Calculate the desired field of view based on the stack height
            float desiredFOV = Mathf.Clamp(stackHeight * heightMultiplier, minFOV, maxFOV);

            // Lerp between the current FOV and the desired FOV for smooth transitions
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, desiredFOV, Time.deltaTime);
        }
    }
}