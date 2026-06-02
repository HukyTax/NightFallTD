using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Refreshes the HUD every frame (money, health, wave number) and manages
// the shop panel slide — open moves it up 90 units, close moves it back down.
public class UiManager : MonoBehaviour
{
    // for upgrade
    [SerializeField] private GameObject Uipanel;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI upgradeMax;
    [SerializeField] private Button UpgradeButton;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private Button open;
    [SerializeField] private Button close;

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
    public GameObject getUpgradePanel()
    {
        return Uipanel;
    }
    public TextMeshProUGUI getUpgradeCostText()
    {
        return costText;
    }
    public TextMeshProUGUI getMaxUpgradeText()
    {
        return upgradeMax;
    }
    public Button getUpgradeButton()
    {
        return UpgradeButton;
    }
}
