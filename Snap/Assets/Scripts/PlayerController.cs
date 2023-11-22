using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 5.0f;
    public float rotationSpeed = 120.0f;
    public float maxLookUp = 80.0f;
    public float maxLookDown = 80.0f;
    public float basePlayerMass = 60.0f; // Initial mass of the player without the cube

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
                if (playerRigidbody != null)
                {
                    playerRigidbody.mass = basePlayerMass + carriedCube.GetComponent<Rigidbody>().mass;
                }

                // Set the cube as a child of the player
                carriedCube.transform.parent = transform;

                // Disable the cube's rigidbody physics
                Rigidbody cubeRigidbody = carriedCube.GetComponent<Rigidbody>();
                if (cubeRigidbody != null)
                {
                    cubeRigidbody.isKinematic = true;
                }
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

            // Adjust the player's mass when dropping the cube
            if (playerRigidbody != null)
            {
                playerRigidbody.mass = basePlayerMass;
            }

            // Detach the cube from the player
            carriedCube.transform.parent = null;
            carriedCube = null;
        }
    }
}