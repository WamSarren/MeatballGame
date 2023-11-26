using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float fallThreshold = -10.0f; // Adjust this threshold based on your game's requirements
    public PauseMenu pauseMenu;

    void Update()
    {
        // Check if the player has fallen below the fall threshold
        if (PlayerHasFallen())
        {
            // Reload the current scene
            ReloadCurrentScene();
        }
    }

    bool PlayerHasFallen()
    {
        // Check the player's Y position or any other relevant condition
        return (PlayerController.Instance.transform.position.y < fallThreshold);
    }

    void ReloadCurrentScene()
    {
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // Spawn the player at the spawn point
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        // Find the spawn point by tag
        GameObject spawnPoint = GameObject.FindGameObjectWithTag("Respawn");

        // Check if the spawn point is found
        if (spawnPoint != null)
        {
            // Find the player by tag
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            // Check if the player is found
            if (player != null)
            {
                // Move the player to the spawn point position
                player.transform.position = spawnPoint.transform.position;
                Debug.Log("Player spawned at the spawn point.");
            }
            else
            {
                Debug.LogError("Player not found!");
            }
        }
        else
        {
            Debug.LogWarning("Spawn point not found!");
        }
    }
}