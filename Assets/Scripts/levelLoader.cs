using UnityEngine;
using UnityEngine.SceneManagement;

public class levelLoader : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadLevel();
        }
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene("Test");
    }

    public void gameOver()
    {
        SceneManager.LoadScene("Game Over");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Start");
    }
}
