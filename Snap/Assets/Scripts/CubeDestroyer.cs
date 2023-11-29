using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDestroyer : MonoBehaviour
{
    public CubePoolManager cubePoolManager;
    public int numberOfSmallCubesToSpawn = 5;
    public GameObject stackingReference; // Reference to the object for stacking
    public GameObject visibilityObject; // The GameObject you want to make visible
    private Vector3 stackOffset = new Vector3(0.0f, 0.5f, 0.0f); // Offset for stacking

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cube"))
        {
            // Destroy the cube
            Destroy(other.gameObject);

            // Get smaller cubes from the pool and enable them
            List<GameObject> smallerCubes = cubePoolManager.GetSmallerCubesFromPool(numberOfSmallCubesToSpawn);

            // Track the position for stacking based on the reference object
            Vector3 stackPosition = stackingReference.transform.position;

            foreach (GameObject smallerCube in smallerCubes)
            {
                smallerCube.transform.position = stackPosition; // Set the position based on the stacking position
                smallerCube.SetActive(true);

                // Update the stacking position for the next cube
                stackPosition += stackOffset;
            }
        }
    }

    public void OnCubePlaced()
    {
        // Make the visibilityObject active
        visibilityObject.SetActive(true);

        // Start a coroutine to make it visible for 2 seconds
        StartCoroutine(MakeVisibleForDuration(1.5f));
    }

    IEnumerator MakeVisibleForDuration(float duration)
    {
        // Wait for the specified duration
        yield return new WaitForSeconds(duration);

        // After waiting, make the visibilityObject inactive
        visibilityObject.SetActive(false);
    }
}