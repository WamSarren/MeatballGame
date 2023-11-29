using UnityEngine;

public class BridgeUpgrader : MonoBehaviour
{
    public GameObject[] bridgePrefabs; // An array to store all bridge prefabs
    private int currentBridgeIndex = 0; // Index to track the current bridge in the upgrade sequence
    private bool reachedFinalBridge = false; // Added variable to track if the final bridge is reached
    private bool initialBridgeBuilt = false; // Added variable to track if the initial bridge is built

    public SphereCollider upgradeArea; // Reference to the upgrade trigger collider

    void Update()
    {
        // Your logic to trigger the upgrade
        if (!reachedFinalBridge && Input.GetKeyDown(KeyCode.U))
        {
            if (!initialBridgeBuilt)
            {
                BuildInitialBridge();
            }
            else
            { 
                    UpgradeBridge();
            }
        }
    }

    private void BuildInitialBridge()
    {
        // Instantiate the initial bridge at the upgrade area's position with an offset on the z-axis
        if (bridgePrefabs.Length > 0)
        {
            Vector3 initialBridgePosition = upgradeArea.transform.position + new Vector3(0f, 0f, 5.1f);
            GameObject initialBridge = Instantiate(bridgePrefabs[0], initialBridgePosition, Quaternion.identity);
            // Optionally, you might want to parent the initial bridge to the upgrade area or set its position relative to the upgrade area.

            initialBridgeBuilt = true;
        }
    }

    public void UpgradeBridge()
    {
        // Check if the player has enough materials to upgrade
        int requiredMaterials = GetRequiredMaterialsForUpgrade();
        PlayerController playerController = PlayerController.Instance;

        if (playerController != null && playerController.GetPickedUpCubeCount() >= requiredMaterials)
        {
            // Player has enough materials, and the final bridge is not reached
            if (!reachedFinalBridge)
            {
                // Destroy the existing bridge
                GameObject currentBridge = GameObject.FindGameObjectWithTag("Bridge");
                if (currentBridge != null)
                {
                    Destroy(currentBridge);
                }

                // Increment the current bridge index
                playerController.UpdateCurrentBridgeIndex();

                // Ensure the index is within bounds
                if (playerController.currentBridgeIndex < bridgePrefabs.Length)
                {
                    // Instantiate the next bridge at the saved position with an offset on the z-axis
                    GameObject newBridge = Instantiate(bridgePrefabs[playerController.currentBridgeIndex], GetBridgePosition(), Quaternion.identity);

                    // Subtract the required materials from the player's count
                    playerController.DecreasePickedUpCubeCount(requiredMaterials);

                    // Update the UI after the bridge is upgraded
                    playerController.UpdateCubeCountUI();

                    // Check if the final bridge is reached
                    if (playerController.currentBridgeIndex == bridgePrefabs.Length - 1)
                    {
                        reachedFinalBridge = true;
                        Debug.Log("You have reached the final bridge!");
                    }
                }
                else
                {
                    // Player has reached the final bridge, display a message or perform other actions
                    Debug.Log("You have reached the final bridge!");
                    reachedFinalBridge = true;
                }
            }
            else
            {
                // Player has reached the final bridge, display a message or perform other actions
                Debug.Log("You have reached the final bridge!");
            }
        }
        else
        {
            // Player does not have enough materials, display a message or perform other actions
            Debug.Log("Not enough materials to upgrade the bridge.");
        }
    }

    private int GetRequiredMaterialsForUpgrade()
    {
        // Define your upgrade costs based on the current bridge index
        int[] upgradeCosts = { 5, 10}; // Adjust the costs for each upgrade

        // Ensure the current bridge index is within bounds
        if (currentBridgeIndex < upgradeCosts.Length)
        {
            return upgradeCosts[currentBridgeIndex];
        }

        return 0; // Return 0 if the index is out of bounds (for safety)
    }

    private Vector3 GetBridgePosition()
    {
        // Check if the current bridge is LargeBridge and adjust the position accordingly
        int largeBridgeIndex = 2; // Assuming LargeBridge is at index 2 in the bridgePrefabs array

        if (PlayerController.Instance.currentBridgeIndex == largeBridgeIndex)
        {
            // Set the position for LargeBridge with an offset on the y-axis
            return new Vector3(
                GameObject.FindWithTag("Bridge").transform.position.x,
                GameObject.FindWithTag("Bridge").transform.position.y,
                GameObject.FindWithTag("Bridge").transform.position.z - 4.4f
            );
        }

        // For other bridges, use the initial position of the existing bridge
        return GameObject.FindWithTag("Bridge").transform.position;
    }
}