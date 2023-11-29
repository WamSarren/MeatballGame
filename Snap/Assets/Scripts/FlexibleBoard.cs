using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlexibleBoard : MonoBehaviour
{
    public float playerMassThreshold = 20f;

    public Rigidbody cube1Rigidbody;
    public Rigidbody cube2Rigidbody;

    public GameObject triggerArea; // Reference to the trigger area GameObject

    void Start()
    {
        // Find rigidbodies dynamically
        cube1Rigidbody = transform.Find("Board1").GetComponent<Rigidbody>();
        cube2Rigidbody = transform.Find("Board2").GetComponent<Rigidbody>();

        // Null checks for the components
        if (cube1Rigidbody == null || cube2Rigidbody == null)
        {
            Debug.LogError("Rigidbody is not found. Make sure to assign them in the Inspector.");
            return;
        }

        // Null check for PlayerController.Instance
        if (PlayerController.Instance == null)
        {
            Debug.LogError("PlayerController.Instance is null. Make sure it's assigned.");
        }
    }

    void Update()
    {
        // Check if the player is potentially inside the trigger area before checking the board condition
        if (IsPlayerPotentiallyInsideTrigger())
        {
            // Check player's mass every frame
            CheckPlayerMass();
        }
    }

    void CheckPlayerMass()
    {
        float playerMass = PlayerController.Instance.GetPlayerMass();

        // Break the board if the player's mass is greater than or equal to the threshold
        if (playerMass >= playerMassThreshold)
        {
            BreakBoard();
        }
    }

    bool IsPlayerPotentiallyInsideTrigger()
    {
        Collider[] colliders = Physics.OverlapSphere(triggerArea.transform.position, triggerArea.GetComponent<SphereCollider>().radius);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

    void BreakBoard()
    {
        // Destroy all HingeJoint components on "Board1" and its children
        DestroyHingeJoints(cube1Rigidbody);

        // Destroy all HingeJoint components on "Board2" and its children
        DestroyHingeJoints(cube2Rigidbody);

        // Enable gravity on rigidbodies
        cube1Rigidbody.useGravity = true;
        cube2Rigidbody.useGravity = true;

        // Apply force to simulate the bridge snapping in half
        ApplyForceToRigidbody(cube1Rigidbody);
        ApplyForceToRigidbody(cube2Rigidbody);

        // Apply additional forces at the ends of the boards
        ApplyForceAtEnd(cube1Rigidbody, cube1Rigidbody.transform.position + cube1Rigidbody.transform.forward * 2f);
        ApplyForceAtEnd(cube2Rigidbody, cube2Rigidbody.transform.position - cube2Rigidbody.transform.forward * 2f);

        // Increase the mass to make the boards fall faster
        IncreaseMass(cube1Rigidbody);
        IncreaseMass(cube2Rigidbody);
    }

    void DestroyHingeJoints(Rigidbody rb)
    {
        // Destroy all HingeJoint components on the given Rigidbody and its children
        HingeJoint[] allHingeJoints = rb.GetComponentsInChildren<HingeJoint>();
        foreach (HingeJoint hingeJoint in allHingeJoints)
        {
            Destroy(hingeJoint);
        }
    }

    void ApplyForceToRigidbody(Rigidbody rb)
    {
        // Adjust the force magnitude and direction based on your requirements
        Vector3 forceDirection = rb.transform.up; // Example: apply force in the upward direction
        float forceMagnitude = 100f; // Example: adjust force magnitude as needed

        // Apply the force to the rigidbody
        rb.AddForce(forceDirection * forceMagnitude);
    }

    void ApplyForceAtEnd(Rigidbody rb, Vector3 position)
    {
        // Adjust the force magnitude and direction based on your requirements
        Vector3 forceDirection = -rb.transform.up; // Example: apply force opposite to the upward direction
        float forceMagnitude = 100f; // Example: adjust force magnitude as needed

        // Apply the force to the rigidbody at a specific position
        rb.AddForceAtPosition(forceDirection * forceMagnitude, position);
    }

    void IncreaseMass(Rigidbody rb)
    {
        // Adjust the mass multiplier based on your requirements
        float massMultiplier = 2f; // Example: adjust multiplier as needed

        // Increase the mass of the rigidbody
        rb.mass *= massMultiplier;
    }
}