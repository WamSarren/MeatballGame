using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CubeStackUtility
{
    public static float CalculateStackHeight(GameObject stackRoot)
    {
        float totalHeight = 0.0f;

        // Check if the stackRoot is a valid GameObject with a Transform component
        if (stackRoot != null && stackRoot.transform != null)
        {
            // Iterate through each child in the hierarchy
            foreach (Transform childTransform in stackRoot.transform)
            {
                // Check if the child has a valid GameObject with a Collider component
                if (childTransform != null && childTransform.gameObject != null && childTransform.gameObject.TryGetComponent(out Collider childCollider))
                {
                    // Add the height of the child to the total height
                    totalHeight += childCollider.bounds.size.y;

                    // Recursively calculate height for potential grandchildren
                    totalHeight += CalculateStackHeight(childTransform.gameObject);
                }
            }
        }

        return totalHeight;
    }

    public static float CalculateTotalMass(GameObject stackRoot)
    {
        float totalMass = 0.0f;

        // Check if the stackRoot is a valid GameObject with a Rigidbody component
        if (stackRoot != null && stackRoot.TryGetComponent(out Rigidbody rootRigidbody))
        {
            // Add the mass of the root Rigidbody to the total mass
            totalMass += rootRigidbody.mass;
        }

        // Iterate through each child in the hierarchy
        foreach (Transform childTransform in stackRoot.transform)
        {
            // Check if the child has a valid GameObject with a Rigidbody component
            if (childTransform != null && childTransform.gameObject != null && childTransform.gameObject.TryGetComponent(out Rigidbody childRigidbody))
            {
                // Add the mass of the child Rigidbody to the total mass
                totalMass += childRigidbody.mass;
            }
        }

        return totalMass;
    }
}