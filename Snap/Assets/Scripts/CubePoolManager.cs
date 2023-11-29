using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePoolManager : MonoBehaviour
{
    public GameObject smallerCubePrefab;
    public int poolSize = 10;

    private List<GameObject> smallerCubePool;

    void Start()
    {
        InitializePool();
    }

    void InitializePool()
    {
        smallerCubePool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject smallerCube = Instantiate(smallerCubePrefab);
            smallerCube.SetActive(false);
            smallerCubePool.Add(smallerCube);
        }
    }

    public List<GameObject> GetSmallerCubesFromPool(int count)
    {
        List<GameObject> result = new List<GameObject>();

        // Find inactive cubes in the pool
        int inactiveCubeCount = 0;
        foreach (GameObject smallerCube in smallerCubePool)
        {
            if (!smallerCube.activeInHierarchy)
            {
                result.Add(smallerCube);
                inactiveCubeCount++;

                if (inactiveCubeCount >= count)
                {
                    // If enough inactive cubes are found, break the loop
                    break;
                }
            }
        }

        // If there are not enough inactive cubes, expand the pool
        for (int i = inactiveCubeCount; i < count; i++)
        {
            GameObject newSmallerCube = Instantiate(smallerCubePrefab);
            newSmallerCube.SetActive(false);
            smallerCubePool.Add(newSmallerCube);
            result.Add(newSmallerCube);

            // Break the loop if we've reached the requested count
            if (result.Count >= count)
            {
                break;
            }
        }

        return result;
    }
}