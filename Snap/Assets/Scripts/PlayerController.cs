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

    public Transform groundCheck; // Reference to the ground check object
    public float groundCheckDistance = 0.1f; // Distance to check for ground
    public float fallMultiplier = 2.5f; // Adjust this value to control the fall speed

    public float jumpForce = 5.0f; // Adjust this value to control the jump force
    private bool isJumping = false;

    public static RaycastHit hit;
    public CrosshairController crosshairController;
    private GameObject hitObject; // Declare hitObject as a class-level variable
    private Color originalCubeColor = Color.clear;
    private GameObject coloredCube;

    private float currentRotationX = 0.0f;
    private bool isCarryingCube = false;
    private GameObject carriedCube;
    private Rigidbody playerRigidbody;
    public static PlayerController Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


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

        // Handle jump input
        HandleJumpInput();

        // Raycast to check if there's a cube in front of the player
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2.0f))
        {
            // Get the GameObject hit by the raycast
            hitObject = hit.collider.gameObject;

            // Notify the CrosshairController that there's a hit
            crosshairController.SetHit(true);

            if (crosshairController.isHit == true)
            {
                // Change the color of the cube
                ChangeCubeColor(hitObject);
            }

            // Handle all cube interactions (pick up, drop, stack) when 'Cube' is pressed
            if (Input.GetButtonDown("Cube"))
            {
                HandleCubeInteraction();
            }
        }
        else
        {
            if (crosshairController.isHit == true)
            {
                // Reset the color of the cube if not hit
                ResetCubeColor(coloredCube);
            }

            // Handle cube interaction (drop) when 'Cube' is pressed
            if (Input.GetButtonDown("Cube"))
            {
                DropCube();
            }

            // Notify the CrosshairController that there's no hit
            crosshairController.SetHit(false);
        }

        // Ground check
        if (IsGrounded())
        {
            // Reset jumping state when grounded
            isJumping = false;
        }
        else
        {
            // Apply fall multiplier when not grounded
            ApplyFallMultiplier();
        }
    }

    void FixedUpdate()
    {
        // Additional physics-related updates can be placed here if needed
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

    void HandleJumpInput()
    {
        // Check for jump input
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            // Apply jump force
            playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = true;
        }
    }

    void HandleCubeInteraction()
    {
        // Check if the player is carrying a cube
        if (isCarryingCube)
        {
            // Raycast to check if there's a cube in front of the player
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 2.0f))
            {
                if (hit.collider.CompareTag("Cube"))
                {
                    // Check if the space to stack is clear before stacking
                    Vector3 stackPosition = hit.collider.transform.position + new Vector3(0.0f, hit.collider.transform.localScale.y, 0.0f);
                    if (IsSpaceClear(stackPosition))
                    {
                        // Stack the carried cube on top of the other cube
                        StackCubeOnTop(hit.collider.gameObject);
                    }
                    else
                    {
                        // Handle the case where the stacking space is not clear
                        Debug.Log("Cannot stack cube, space is not clear.");
                    }
                }
            }
            else
            {
                // Drop the cube if no additional cubes are in front of the player
                DropCube();
            }
        }
        else
        {
            // Try to pick up a nearby cube
            PickUpCube();
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

    void UpdatePlayerMass(Transform parent)
    {
        // Reset player mass to base mass
        playerRigidbody.mass = basePlayerMass;

        // Calculate the Mass of stacked cubes
        float stackMass = CubeStackUtility.CalculateTotalMass(carriedCube);

        // Add the mass of the originally carried cube
        if (carriedCube != null && carriedCube.TryGetComponent(out Rigidbody carriedRigidbody))
        {
            playerRigidbody.mass += carriedRigidbody.mass;
        }

        // Adjust the player's mass based on the stack mass
        playerRigidbody.mass += stackMass;
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

            // Check if the space to stack is clear
            if (IsSpaceClear(stackPosition))
            {
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

                // Reset player mass to base mass
                playerRigidbody.mass = basePlayerMass;
            }
            else
            {
                // Handle the case where the stacking space is not clear
                Debug.Log("Cannot stack cube, space is not clear.");
            }
        }
    }

    void ChangeCubeColor(GameObject cube)
    {
        // Check if the object has the tag "Cube"
        if (cube.CompareTag("Cube"))
        {
            if (coloredCube != null && coloredCube != cube)
            {
                // Reset the color of the previously colored cube
                ResetCubeColor(coloredCube);
            }

            Renderer cubeRenderer = cube.GetComponent<Renderer>();
            if (cubeRenderer != null)
            {
                // Store the original color if not already stored
                if (originalCubeColor == Color.clear)
                {
                    originalCubeColor = cubeRenderer.material.color;
                }

                // Change the color of the cube
                cubeRenderer.material.color = Color.red; // Change to your desired color

                // Set the currently colored cube
                coloredCube = cube;
            }
        }
    }

    void ResetCubeColor(GameObject cube)
    {
        if (cube == null)
        {
            // Cube is null, no need to reset the color
            return;
        }

        Renderer cubeRenderer = cube.GetComponent<Renderer>();
        if (cubeRenderer != null)
        {
            // Reset the color of the cube to the original color
            if (originalCubeColor != Color.clear)
            {
                cubeRenderer.material.color = originalCubeColor;

                // Clear the original color to indicate it has been reset
                originalCubeColor = Color.clear;
            }
        }
    }

    bool ShouldResetColor(GameObject cube)
    {
        // Check if the raycast is not hitting the cube anymore
        if (!Physics.Raycast(transform.position, transform.forward, out RaycastHit newHit, 2.0f))
        {
            return true; // Reset the color if the raycast is not hitting the cube
        }

        // Check if the new hit object is different from the previous hit object
        if (newHit.collider.gameObject != hitObject)
        {
            return true; // Reset the color if the raycast hits a different object
        }

        // Check if the hit object is not the currently colored cube
        if (cube != coloredCube)
        {
            return true; // Reset the color if the hit object is not the currently colored cube
        }

        // Add other conditions as needed

        return false; // Don't reset the color by default
    }

    bool IsGrounded()
    {
        // Check for collisions beneath the player using the groundCheck GameObject's layer
        Ray ray = new Ray(groundCheck.position, Vector3.down);

        Debug.DrawRay(ray.origin, ray.direction * groundCheckDistance, Color.red);

        if (Physics.Raycast(ray, groundCheckDistance))
        {
            return true;
        }

        return false;
    }

    void ApplyFallMultiplier()
    {
        if (playerRigidbody.velocity.y < 0)
        {
            // Applying extra force to make the player descend faster
            playerRigidbody.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }

    bool IsSpaceClear(Vector3 position)
    {
        // Offset the starting position slightly above the cube
        Vector3 rayStart = position + new Vector3(0.0f, -0.7f, 0.0f); // Adjust the Y offset as needed

        // Cast a ray downward to check if the space is clear
        Ray ray = new Ray(rayStart, Vector3.up);
        float rayDistance = 0.5f; // Adjust this value based on your needs

        // Debug the ray in the Scene view
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red, 0.5f);

        // Perform the raycast
        if (!Physics.Raycast(ray, rayDistance))
        {
            return true; // Space is clear
        }

        return false; // Space is not clear
    }
}