using UnityEngine;
using UnityEngine.SceneManagement;

// Handles the main menu buttons. Lives on a GameObject in the Start scene.
public class StartMenuManager : MonoBehaviour
{
    // Name of the scene to load when the player hits Play.
    // Set in the Inspector so it can be changed without touching code.
    [SerializeField] private string gameSceneName = "Test";

    // Wired to the Play button in the Start scene UI.
    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    // Wired to the Quit button. No-ops in the Editor; closes the build.
    public void QuitGame()
    {
        Application.Quit();
    }
}
