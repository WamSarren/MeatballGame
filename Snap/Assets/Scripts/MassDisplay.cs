using UnityEngine;
using UnityEngine.UI;

public class MassDisplay : MonoBehaviour
{
    public Rigidbody playerRigidbody;
    public Text massText;

    void Start()
    {
        // Check if the Rigidbody and Text are assigned in the Inspector
        if (playerRigidbody == null || massText == null)
        {
            Debug.LogError("Player Rigidbody or Mass Text not assigned in the Inspector.");
            enabled = false; // Disable the script if components are missing
            return;
        }

        // Set the initial mass text
        UpdateMassText();
    }

    void Update()
    {
        // Check for changes in the player's mass and update the UI text
        UpdateMassText();
    }

    void UpdateMassText()
    {
        // Update the UI text with the current mass of the player's Rigidbody
        massText.text = "Player Mass: " + playerRigidbody.mass.ToString("F2"); // Display mass with 2 decimal places
    }
}