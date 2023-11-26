using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;

    void Start()
    {

    }

    void Update()
    {
        // Check for pause input (e.g., Escape key)
        if (Input.GetKeyDown(KeyCode.Escape) && optionsMenuUI.activeSelf)
        {
            TogglePauseMenu();
            OpenOptions();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        // Toggle the pause menu visibility
        pauseMenuUI.SetActive(!pauseMenuUI.activeSelf);

        // Pause or resume the game
        if (pauseMenuUI.activeSelf)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0f; // Stop time to pause the game
    }

    void ResumeGame()
    {
        Time.timeScale = 1f; // Resume time to continue the game
    }

    public void RestartGame()
    {
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        ResumeGame(); // Make sure to resume time after restarting
    }

    public void OpenOptions()
    {
        // Hide the pause menu
        pauseMenuUI.SetActive(false);

        // Toggle the options menu visibility
        optionsMenuUI.SetActive(!optionsMenuUI.activeSelf);

        // Pause or resume the game based on the options menu visibility
        if (optionsMenuUI.activeSelf)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}