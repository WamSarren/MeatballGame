using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeController2 : MonoBehaviour
{
    public float fallThreshold = 10.0f; // Adjust this threshold based on your game's requirements
    public GameObject triggerArea; // Reference to the trigger area GameObject
    public float forceMagnitude = 100f; // Adjust force magnitude as needed

    void Update()
    {
        // Check if the total mass in the trigger area exceeds the threshold
        if (GetTotalMassInTriggerArea() > fallThreshold)
        {
            // Trigger the bridge to fall
            FallBridge();
        }
    }

    float GetTotalMassInTriggerArea()
    {
        float totalMass = 0.0f;

        // Use the trigger area's position and the radius of the sphere collider to define the bounds
        Collider[] colliders = Physics.OverlapSphere(triggerArea.transform.position, triggerArea.GetComponent<SphereCollider>().radius);

        foreach (Collider collider in colliders)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                totalMass += rb.mass;
            }
        }

        return totalMass;
    }

    void FallBridge()
    {
        // Find all HingeJoint components in the hierarchy
        HingeJoint[] allHingeJoints = GetComponentsInChildren<HingeJoint>();

        // Iterate through each HingeJoint and destroy it
        foreach (HingeJoint hingeJoint in allHingeJoints)
        {
            if (hingeJoint != null)
            {
                // Get the associated Rigidbody
                Rigidbody rb = hingeJoint.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    // Enable gravity for the Rigidbody
                    rb.useGravity = true;

                    // Apply force to simulate the bridge snapping in half
                    ApplyForceToRigidbody(rb);
                }

                // Destroy the HingeJoint component
                Destroy(hingeJoint);
            }
        }

        // Add any additional actions here, such as playing a falling animation or applying force to individual segments
    }

    void ApplyForceToRigidbody(Rigidbody rb)
    {
        // Adjust the force direction based on your requirements
        Vector3 forceDirection = rb.transform.up; // Example: apply force in the upward direction

        // Apply the force to the rigidbody
        rb.AddForce(forceDirection * forceMagnitude);
    }
}