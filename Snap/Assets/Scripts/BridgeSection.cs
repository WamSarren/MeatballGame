using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeSection : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 initialPosition;
    public bool isAnchor = false; // Set this to true for the first and last segments
    public Transform player; // Reference to the player's transform

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody not found on BridgeSection.");
        }

        // Store the initial position for reference
        initialPosition = transform.position;

        // Find the player if not assigned in the Inspector
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void FixedUpdate()
    {
        // Only adjust position for non-anchor segments
        if (!isAnchor)
        {
            ApplySagEffect();
        }
    }

    void ApplySagEffect()
    {
        // Calculate the distance between the player and the bridge section
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        // Use the distance as a factor to determine sagging
        float sagFactor = rb.mass * 0.1f * (1.0f / (distanceToPlayer + 1.0f)); // Adding 1 to avoid division by zero

        // Debugging information
        Debug.Log("Distance to Player: " + distanceToPlayer);
        Debug.Log("Sag Factor: " + sagFactor);

        // Apply sag effect by adjusting position along the Y-axis
        Vector3 sagPosition = initialPosition - new Vector3(0.0f, sagFactor, 0.0f);
        rb.MovePosition(sagPosition);
    }
}