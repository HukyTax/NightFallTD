using TMPro;
using UnityEngine;

// Attached to placed tower GameObjects alongside Turret.
// Clicking the tower opens an upgrade panel; the Upgrade button applies stat
// bonuses and deducts gold. Upgrades go up to the number of tiers in the `upgrades` array.
public class TowerUpgrade : MonoBehaviour
{
    // Defines the cost and stat increases for one upgrade tier.
    // Add more entries in the Inspector to add more tiers.
    [System.Serializable]
    public struct UpgradeLevel
    {
        public int cost;
        public float rangeBonus;
        public float bpsBonus;      // bullets-per-second increase
        public int damageBonus;
    }

    [SerializeField] private UpgradeLevel[] upgrades;
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private TextMeshProUGUI upgradeCostText;
    // Shown in place of the cost text when the tower is fully upgraded.
    [SerializeField] private TextMeshProUGUI upgradeMaxText;

    // Tracks how many upgrades have been applied. Also used as the index into `upgrades`.
    private int currentLevel = 0;
    private Turret turret;
    private Economy economy;
    private bool panelOpen = false;

    void Start()
    {
        turret = GetComponent<Turret>();
        economy = GameObject.Find("LevelManager").GetComponentInChildren<Economy>();
        if (upgradePanel != null) upgradePanel.SetActive(false);
    }

    // Clicking the tower sprite toggles the upgrade panel open or closed.
    private void OnMouseDown()
    {
        panelOpen = !panelOpen;
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(panelOpen);
            RefreshUI();
        }
    }

    // Called by the Upgrade button inside the panel.
    // Checks affordability, deducts cost, applies stat bonuses, then advances the level.
    public void Upgrade()
    {
        // Already at max level — nothing to do.
        if (currentLevel >= upgrades.Length) return;

        UpgradeLevel lvl = upgrades[currentLevel];
        if (economy.getMoney() < lvl.cost)
        {
            Debug.Log("Not enough money to upgrade. Cost: " + lvl.cost);
            return;
        }

        // ChangeMoney takes an absolute value — compute new total manually.
        economy.ChangeMoney(economy.getMoney() - lvl.cost);
        turret.ApplyUpgrade(lvl.rangeBonus, lvl.bpsBonus, lvl.damageBonus);
        currentLevel++;
        RefreshUI();
    }

    // Updates the panel to show the next upgrade cost, or swaps to the MAX label
    // when the tower has been fully upgraded.
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
