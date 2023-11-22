using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeController : MonoBehaviour
{
    public float fallThreshold = 10.0f; // Adjust this threshold based on your game's requirements
    public GameObject triggerArea; // Reference to the trigger area GameObject

    public Transform player;
    public float maxSagDistance = 5.0f;
    public float driveForce = 100.0f;

    private ConfigurableJoint joint;

    void Start()
    {
        joint = GetComponent<ConfigurableJoint>();
        if (joint == null)
        {
            Debug.LogError("ConfigurableJoint not found on BridgeController.");
            return;
        }

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        // Check if the total mass in the trigger area exceeds the threshold
        if (GetTotalMassInTriggerArea() > fallThreshold)
        {
            // Trigger the bridge to fall
            FallBridge();
        }
    }

    void FixedUpdate()
    {
        if (joint == null || player == null)
        {
            return;
        }

        // Calculate distance to player
        float distanceToPlayer = Mathf.Abs(player.position.y - transform.position.y);

        // Calculate normalized sag distance
        float normalizedSagDistance = Mathf.Clamp01(distanceToPlayer / maxSagDistance);

        // Adjust drive force based on sag distance
        joint.yDrive = new JointDrive
        {
            positionSpring = driveForce * (1.0f - normalizedSagDistance),
            maximumForce = Mathf.Infinity
        };
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