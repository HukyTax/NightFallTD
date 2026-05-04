using UnityEngine;
using UnityEngine.SceneManagement;

// Handles all scene transitions in the game.
// Lives as a child of LevelManager so HealthManager can reach it
// via GetComponentInChildren<levelLoader>().
public class levelLoader : MonoBehaviour
{
    void Update()
    {
        // Dev shortcut — R restarts the current level without going through the menu.
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadLevel();
        }
    }

    // Reloads the main gameplay scene. Also called by any Restart button in UI.
    public void ReloadLevel()
    {
        SceneManager.LoadScene("Test");
    }

    // Called by HealthManager when base health hits 0.
    public void gameOver()
    {
        SceneManager.LoadScene("Game Over");
    }

    // Called by the Main Menu button on the Game Over screen (or any pause menu).
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Start");
    }
}
