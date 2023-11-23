using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 5.0f;
    public float rotationSpeed = 120.0f;
    public float maxLookUp = 80.0f;
    public float maxLookDown = 80.0f;
    public float basePlayerMass = 10.0f; // Initial mass of the player without the cube

    private float currentRotationX = 0.0f;
    private bool isCarryingCube = false;
    private GameObject carriedCube;
    private Rigidbody playerRigidbody;

    void Start()
    {
        // Get the player's rigidbody component
        playerRigidbody = GetComponent<Rigidbody>();
        if (playerRigidbody == null)
        {
            Debug.LogError("Player does not have a Rigidbody component.");
        }
    }

    void Update()
    {
        // Handle player movement
        HandleMovement();

        // Handle player rotation based on mouse movement
        HandleMouseLook();

        // Handle cube interaction
        HandleCubeInteraction();

        HandleCubeStacking();

        // Calculate the Height of stacked cubes
        float stackHeight = CubeStackUtility.CalculateStackHeight(carriedCube);

        Debug.Log("Total stack height: " + stackHeight);
    }

    void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, 0.0f, verticalInput);
        movement.Normalize();

        transform.Translate(movement * movementSpeed * Time.deltaTime);
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * 3;
        float mouseY = Input.GetAxis("Mouse Y") * 3;

        currentRotationX -= mouseY * rotationSpeed * Time.deltaTime;
        currentRotationX = Mathf.Clamp(currentRotationX, -maxLookUp, maxLookDown);

        transform.rotation = Quaternion.Euler(currentRotationX, transform.eulerAngles.y + mouseX * rotationSpeed * Time.deltaTime, 0.0f);
    }

    void HandleCubeInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isCarryingCube)
            {
                // Drop the cube
                DropCube();
            }
            else
            {
                // Try to pick up a nearby cube
                PickUpCube();
            }
        }
    }

    void PickUpCube()
    {
        // Raycast to check if there's a cube in front of the player
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2.0f))
        {
            if (hit.collider.CompareTag("Cube"))
            {
                // Pick up the cube
                isCarryingCube = true;
                carriedCube = hit.collider.gameObject;

                // Adjust the player's mass when picking up the cube
                UpdatePlayerMass(transform);

                // Set the cube as a child of the player
                carriedCube.transform.parent = transform;

                // Disable the cube's rigidbody physics
                Rigidbody cubeRigidbody = carriedCube.GetComponent<Rigidbody>();
                if (cubeRigidbody != null)
                {
                    cubeRigidbody.isKinematic = true;
                }

                // Offset the cube's position to place it on top of the player's head
                Vector3 offset = new Vector3(0.0f, 1.8f, 0.0f); // Adjust the Y value based on your preference
                carriedCube.transform.localPosition = offset;
            }
        }
    }

    void DropCube()
    {
        // Drop the cube
        isCarryingCube = false;

        if (carriedCube != null)
        {
            // Enable the cube's rigidbody physics
            Rigidbody cubeRigidbody = carriedCube.GetComponent<Rigidbody>();
            if (cubeRigidbody != null)
            {
                cubeRigidbody.isKinematic = false;
            }

            // Calculate the drop position in front of the player
            Vector3 dropPosition = transform.position + transform.forward * 2.0f; // Adjust the distance as needed

            // Set the drop position for the cube
            carriedCube.transform.position = dropPosition;

            // Detach the cube from the player
            carriedCube.transform.parent = null;
            carriedCube = null;

            // Adjust the player's mass when dropping the cube
            if (playerRigidbody != null)
            {
                playerRigidbody.mass = basePlayerMass;
            }
        }
    }

    void HandleCubeStacking()
    {
        // Check if the player is carrying a cube
        if (isCarryingCube)
        {
            // Check if the player presses a key to stack the cube
            if (Input.GetKeyDown(KeyCode.F))
            {
                // Raycast to check if there's a cube in front of the player
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, 2.0f))
                {
                    if (hit.collider.CompareTag("Cube"))
                    {
                        // Stack the carried cube on top of the other cube
                        StackCubeOnTop(hit.collider.gameObject);
                    }
                }
            }
        }
    }

    void UpdatePlayerMass(Transform parent)
    {
        // Reset player mass to base mass
        playerRigidbody.mass = basePlayerMass;

        // Calculate the stack height
        float stackHeight = CubeStackUtility.CalculateStackHeight(carriedCube);

        // Adjust the player's mass based on the stack height
        playerRigidbody.mass += stackHeight * 5.0f;
    }

    void StackCubeOnTop(GameObject baseCube)
    {
        // Check if the base cube is a valid target for stacking
        if (baseCube != null && baseCube.CompareTag("Cube"))
        {
            // Define the offset and rotation for stacking
            Vector3 stackOffset = new Vector3(0.0f, baseCube.transform.localScale.y, 0.0f);
            Quaternion stackRotation = Quaternion.identity; // No rotation

            // Calculate the position to stack the carried cube on top
            Vector3 stackPosition = baseCube.transform.position + stackOffset;

            // Set the position and rotation of the carried cube on top of the base cube
            carriedCube.transform.position = stackPosition;
            carriedCube.transform.rotation = stackRotation;

            // Set the base cube as the parent of the carried cube
            carriedCube.transform.parent = baseCube.transform;

            // Disable the carried cube's rigidbody physics
            Rigidbody cubeRigidbody = carriedCube.GetComponent<Rigidbody>();
            if (cubeRigidbody != null)
            {
                cubeRigidbody.isKinematic = true;
            }

            // Reset isCarryingCube and carriedCube since the cube is now stacked
            isCarryingCube = false;
            carriedCube = null;
        }
    }
}