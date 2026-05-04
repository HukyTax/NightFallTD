using TMPro;
using UnityEngine;

// Shows a one-time step-by-step tutorial the first time the player loads the game.
// Uses PlayerPrefs to remember whether it has already been seen across sessions.
public class TutorialManager : MonoBehaviour
{
    // Each step holds one message displayed in the tutorial panel.
    [System.Serializable]
    public struct TutorialStep
    {
        [TextArea] public string message;
    }

    [SerializeField] private TutorialStep[] steps;
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private TextMeshProUGUI messageText;

    // Stored as an int in PlayerPrefs (0 = not seen, 1 = seen).
    // Using a const avoids typos if this key is ever referenced elsewhere.
    private const string SeenKey = "TutorialSeen";
    private int currentStep = 0;

    void Start()
    {
        // Skip the entire tutorial if the player has already completed or skipped it.
        if (PlayerPrefs.GetInt(SeenKey, 0) == 1)
        {
            tutorialPanel.SetActive(false);
            return;
        }

        if (steps.Length > 0)
        {
            tutorialPanel.SetActive(true);
            ShowStep(0);
        }
    }

    // Called by the "Next" button in the tutorial panel.
    // Advances to the next step, or marks the tutorial complete and closes the panel
    // if there are no more steps.
    public void Next()
    {
        currentStep++;
        if (currentStep >= steps.Length)
        {
            PlayerPrefs.SetInt(SeenKey, 1);
            PlayerPrefs.Save();
            tutorialPanel.SetActive(false);
        }
        else
        {
            ShowStep(currentStep);
        }
    }

    // Called by the "Skip" button. Marks the tutorial done immediately.
    public void Skip()
    {
        PlayerPrefs.SetInt(SeenKey, 1);
        PlayerPrefs.Save();
        tutorialPanel.SetActive(false);
    }

    // Dev/debug helper — clears the seen flag so the tutorial shows on the next launch.
    public void ResetTutorial()
    {
        PlayerPrefs.DeleteKey(SeenKey);
    }

    private void ShowStep(int index)
    {
        messageText.text = steps[index].message;
    }
}
