using UnityEngine;
using UnityEngine.SceneManagement;

// Handles the main menu buttons. Attach to a GameObject in the Start scene.
public class StartMenuManager : MonoBehaviour
{
    // Name of the gameplay scene to load. Set in the Inspector so it can be
    // changed without touching code if the scene gets renamed.
    [SerializeField] private string gameSceneName = "Test";

    // Wired to the Play button in the Start scene UI.
    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    // Wired to the Quit button. Has no effect inside the Unity Editor — only works in a build.
    public void QuitGame()
    {
        Application.Quit();
    }
}
