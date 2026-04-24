using TMPro;
using UnityEngine;

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

    private const string SeenKey = "TutorialSeen";
    private int currentStep = 0;

    void Start()
    {
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

    public void Skip()
    {
        PlayerPrefs.SetInt(SeenKey, 1);
        PlayerPrefs.Save();
        tutorialPanel.SetActive(false);
    }

    public void ResetTutorial()
    {
        PlayerPrefs.DeleteKey(SeenKey);
    }

    private void ShowStep(int index)
    {
        messageText.text = steps[index].message;
    }
}
