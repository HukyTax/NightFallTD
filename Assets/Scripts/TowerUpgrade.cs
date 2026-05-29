using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Attached to placed tower GameObjects. Clicking the tower opens an upgrade panel;
// clicking Upgrade applies stat bonuses and deducts gold, up to the max level.
[RequireComponent(typeof(Turret))]
public class TowerUpgrade : MonoBehaviour
{
    // Each entry defines the cost and stat bonuses for one upgrade tier.
    [System.Serializable]
    public struct UpgradeLevel
    {
        public int cost;
        public float rangeBonus;
        public float bpsBonus;      // bullets-per-second increase
        public int damageBonus;
    }

    [SerializeField] private UpgradeLevel[] upgrades;
    [SerializeField] private GameObject uiPanelContainer;
    [SerializeField] private TextMeshProUGUI upgradeCostText;
    [SerializeField] private TextMeshProUGUI upgradeMaxText;  // shown when fully upgraded
    [SerializeField] private Button UpgradeButton;
    static Boolean open = false;

    // Index into `upgrades` — also equals "number of upgrades applied so far".
    private int currentLevel = 0;
    private Turret turret;
    private Economy economy;
    private bool panelOpen = false;

    void Start()
    {
        uiPanelContainer = GameObject.FindWithTag("Upgrade");

        if(uiPanelContainer != null)
        {
            UpgradeButton = uiPanelContainer.GetComponentInChildren<Button>(true);
            // Finds all TextMeshProUGUI components anywhere inside the panel hierarchy
            TextMeshProUGUI[] textFields = uiPanelContainer.GetComponentsInChildren<TextMeshProUGUI>(true);
            // Assign them based on their hierarchy order, or use a loop to check names
            foreach (var text in textFields)
            {
                if (text.gameObject.name.Contains("Cost")) upgradeCostText = text;
                if (text.gameObject.name.Contains("Max")) upgradeMaxText = text;
            }
            turret = GetComponent<Turret>();
            economy = GameObject.Find("LevelManager").GetComponentInChildren<Economy>();
            // if (uiPanelContainer != null)
            //uiPanelContainer.SetActive(false);
            }
        else
        {
            Debug.LogError("Could not find upgrade panel");
        }

    }

    // Clicking the tower sprite toggles the upgrade panel open/closed.
    private void OnMouseDown()
    {
        panelOpen = !panelOpen;
        if(UpgradeButton != null)
        {
            UpgradeButton.onClick.RemoveAllListeners();
            UpgradeButton.onClick.AddListener(Upgrade);
        }
        RefreshUI();
        if (uiPanelContainer != null && !open )
        {
            uiPanelContainer.transform.position = new Vector3(uiPanelContainer.transform.position.x - 250, uiPanelContainer.transform.position.y);
            open = true;
        }
        else
        {
            uiPanelContainer.transform.position = new Vector3(uiPanelContainer.transform.position.x + 250, uiPanelContainer.transform.position.y);
            open = false;
        }
    }

    // Called by the Upgrade button inside the panel.
    // Deducts cost, applies the stat bonuses to the Turret, then advances the level.
    public void Upgrade()
    {
        if (currentLevel >= upgrades.Length) return;

        UpgradeLevel lvl = upgrades[currentLevel];
        if (economy.GetMoney() < lvl.cost)
        {
            Debug.Log("Not enough money to upgrade. Cost: " + lvl.cost);
            return;
        }

        economy.ChangeMoney(economy.GetMoney() - lvl.cost);
        turret.ApplyUpgrade(lvl.rangeBonus, lvl.bpsBonus, lvl.damageBonus);
        currentLevel++;
        RefreshUI();
    }

    // Shows the cost of the next upgrade, or swaps to the "MAX" label when fully upgraded.
    private void RefreshUI()
    {
        if (currentLevel >= upgrades.Length)
        {
            if (upgradeCostText != null) upgradeCostText.gameObject.SetActive(false);
            if (upgradeMaxText != null) upgradeMaxText.gameObject.SetActive(true);
        }
        else
        {
            if (upgradeCostText != null)
            {
                upgradeCostText.gameObject.SetActive(true);
                upgradeCostText.text = "Upgrade: $" + upgrades[currentLevel].cost;
            }
            if (upgradeMaxText != null) upgradeMaxText.gameObject.SetActive(false);
        }

    }

    public bool CanUpgrade() => currentLevel < upgrades.Length;
    public int CurrentLevel() => currentLevel;
}
