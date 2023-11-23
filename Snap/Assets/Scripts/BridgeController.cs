using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeController : MonoBehaviour
{
    public float fallThreshold = 10.0f; // Adjust this threshold based on your game's requirements
    public GameObject triggerArea; // Reference to the trigger area GameObject

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
        // Add any additional actions here, such as playing a falling animation or destroying the bridge GameObject
        Destroy(gameObject);
    }
}