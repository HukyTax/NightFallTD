using TMPro;
using UnityEngine;

// Shows a one-time tutorial sequence the first time the player enters the game.
// Uses PlayerPrefs to persist the "already seen" flag across sessions.
public class TutorialManager : MonoBehaviour
{
    [System.Serializable]
    public struct TutorialStep
    {
        [TextArea] public string message;
    }

    [SerializeField] private TutorialStep[] steps;
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private TextMeshProUGUI messageText;

    // PlayerPrefs key — stored as int (0 = not seen, 1 = seen).
    private const string SeenKey = "TutorialSeen";
    private int currentStep = 0;

    void Start()
    {
        // Skip the tutorial entirely if the player has completed it before.
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

    // Advances to the next step; marks the tutorial complete and hides the panel
    // when the last step is passed. Wired to the "Next" button in the panel.
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

    // Immediately marks the tutorial as seen and closes the panel.
    // Wired to the "Skip" button in the panel.
    public void Skip()
    {
        PlayerPrefs.SetInt(SeenKey, 1);
        PlayerPrefs.Save();
        tutorialPanel.SetActive(false);
    }

    // Dev/debug helper — clears the seen flag so the tutorial shows again next launch.
    public void ResetTutorial()
    {
        PlayerPrefs.DeleteKey(SeenKey);
    }

    private void ShowStep(int index)
    {
        messageText.text = steps[index].message;
    }
}
