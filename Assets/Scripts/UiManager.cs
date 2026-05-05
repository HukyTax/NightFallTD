using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Refreshes the HUD every frame (money, health, wave number) and manages
// the shop panel slide — open moves it up 90 units, close moves it back down.
public class UiManager : MonoBehaviour
{
    // References found at runtime from the LevelManager hierarchy.
    private Button close;
    private Button open;

    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI roundText;

    private Economy economy;
    private HealthManager healthManager;
    private enemySpawner enemyspawner;

    // The shop panel GameObject. Its children are searched for the toggle buttons by name.
    [SerializeField] GameObject panel;

    void Start()
    {
        GameObject levelManager = GameObject.Find("LevelManager");
        economy = levelManager.GetComponentInChildren<Economy>();
        healthManager = levelManager.GetComponent<HealthManager>();
        enemyspawner = levelManager.GetComponent<enemySpawner>();

        // Buttons are inactive by default, so `true` is required to find them.
        Button[] buttons = panel.GetComponentsInChildren<Button>(true);
        foreach (Button bt in buttons)
        {
            if (bt.name == "ToggelOpen")  open  = bt;
            if (bt.name == "ToggelClose") close = bt;
        }
    }

    void Update()
    {
        UpdateMoney();
        UpdateHealth();
        UpdateRound();
    }

    // Called by the shop open button. Slides the panel into view and swaps toggle buttons.
    public void OpenShop()
    {
        panel.transform.position = new Vector3(panel.transform.position.x, panel.transform.position.y + 90);
        open.gameObject.SetActive(false);
        close.gameObject.SetActive(true);
    }

    // Called by the shop close button. Slides the panel back down.
    public void CloseShop()
    {
        panel.transform.position = new Vector3(panel.transform.position.x, panel.transform.position.y - 90);
        open.gameObject.SetActive(true);
        close.gameObject.SetActive(false);
    }

    public void UpdateMoney()
    {
        moneyText.text = "Money:" + economy.GetMoney();
    }

    public void UpdateHealth()
    {
        healthText.text = "Health:" + healthManager.GetHealth();
    }

    public void UpdateRound()
    {
        roundText.text = "Round: " + enemyspawner.getWave();
    }
}
