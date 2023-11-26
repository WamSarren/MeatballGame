using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Slider soundVolumeSlider;
    public Slider sensitivitySlider;
    public Text sensitivityText;
    public Text volumeText;

    public GameObject optionsMenuUI;
    public GameObject pauseMenuUI;

    void Start()
    {
        // Set initial slider values based on the current settings
        AudioManager.soundVolume = soundVolumeSlider.value;
        volumeText.text = "Volume: " + soundVolumeSlider.value;

        // Find the player GameObject by tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // Check if the player GameObject is found
        if (player != null)
        {
            // Assuming you have a script attached to the player with mouse sensitivity control
            PlayerController playerController = player.GetComponent<PlayerController>();

            if (playerController != null)
            {
                playerController.rotationSpeed = sensitivitySlider.value;
                sensitivityText.text = "Sensitivity: " + sensitivitySlider.value.ToString();
            }
            else
            {
                Debug.LogWarning("PlayerController script not found on the player GameObject.");
            }
        }
        else
        {
            Debug.LogWarning("Player GameObject not found by tag.");
        }
    }

    // Linked to the Sound Volume slider On Value Changed event
    public void OnSoundVolumeChanged(float value)
    {
        // Multiply the value by 100 for a more accurate setting
        float multipliedValue = soundVolumeSlider.value * 100;

        // Round the multiplied value to the nearest whole number
        float roundedValue = Mathf.Round(multipliedValue);

        AudioManager.soundVolume = soundVolumeSlider.value;
        volumeText.text = "Volume: " + roundedValue;
    }

    // Linked to the Sensitivity slider On Value Changed event
    public void OnSensitivityChanged(float value)
    {
        // Find the player GameObject by tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // Check if the player GameObject is found
        if (player != null)
        {
            // Assuming you have a script attached to the player with mouse sensitivity control
            PlayerController playerController = player.GetComponent<PlayerController>();

            if (playerController != null)
            {
                float dividedValue = sensitivitySlider.value / 100;
                float roundedValue = Mathf.Round(dividedValue);
                playerController.rotationSpeed = sensitivitySlider.value;
                sensitivityText.text = "Sensitivity: " + roundedValue;
                Debug.Log("Sensitivity changed: " + value);
            }
            else
            {
                Debug.LogWarning("PlayerController script not found on the player GameObject.");
            }
        }
        else
        {
            Debug.LogWarning("Player GameObject not found by tag.");
        }
    }

    // Linked to the Back button
    public void OnBackButtonClicked()
    {
        // Toggle the visibility of the options menu
        optionsMenuUI.SetActive(!optionsMenuUI.activeSelf);

        // Toggle the visibility of the pause menu
        pauseMenuUI.SetActive(!pauseMenuUI.activeSelf);
    }
}