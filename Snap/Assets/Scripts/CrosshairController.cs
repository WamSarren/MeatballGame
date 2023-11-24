using UnityEngine;
using UnityEngine.UI;

public class CrosshairController : MonoBehaviour
{
    private Image crosshairImage;
    public bool isHit = false; // Added variable to track hit state

    void Start()
    {
        // Get the Image component
        crosshairImage = GetComponent<Image>();

        // Hide the crosshair initially (optional)
        crosshairImage.enabled = false;
    }

    void Update()
    {
        // Perform raycast from the player's position
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity))
        {
            // Set the hit state to true
            isHit = true;
        }
        else
        {
            // Set the hit state to false
            isHit = false;
        }
    }

    // Method to set the hit state
    public void SetHit(bool hitState)
    {
        isHit = hitState;
    }
}