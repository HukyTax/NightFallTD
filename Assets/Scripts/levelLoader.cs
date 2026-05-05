using UnityEngine;
using UnityEngine.SceneManagement;

// Handles all scene transitions in the game.
// Lives as a child of the LevelManager so HealthManager can reach it
// via GetComponentInChildren<levelLoader>().
public class levelLoader : MonoBehaviour
{
    void Update()
    {
        // Dev shortcut: R restarts the current level without going through the menu.
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadLevel();
        }
    }

    // Reloads the main gameplay scene. Also called by the Restart button on the Game Over screen.
    public void ReloadLevel()
    {
        SceneManager.LoadScene("Test");
    }

    // Called by HealthManager when base health reaches 0. Sends the player to Game Over.
    public void gameOver()
    {
        SceneManager.LoadScene("Game Over");
    }

    // Called by the Game Over or pause screen's Main Menu button.
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Start");
    }
}
