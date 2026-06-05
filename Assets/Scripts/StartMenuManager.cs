using UnityEngine;
using UnityEngine.SceneManagement;

// Handles the main menu buttons. Lives on a GameObject in the Start scene.
public class StartMenuManager : MonoBehaviour
{
    // Name of the scene to load when the player hits Play.
    // Set in the Inspector so it can be changed without touching code.
    [SerializeField] private string gameSceneName = "Test";

    // set to the Play button in the Start scene UI.
    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
