using TMPro;
using UnityEngine;

public class TowerUpgrade : MonoBehaviour
{
    [System.Serializable]
    public struct UpgradeLevel
    {
        public int cost;
        public float rangeBonus;
        public float bpsBonus;
        public int damageBonus;
    }

    [SerializeField] private UpgradeLevel[] upgrades;
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private TextMeshProUGUI upgradeCostText;
    [SerializeField] private TextMeshProUGUI upgradeMaxText;

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

    private void OnMouseDown()
    {
        panelOpen = !panelOpen;
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(panelOpen);
            RefreshUI();
        }
    }

    public void Upgrade()
    {
        if (currentLevel >= upgrades.Length) return;

        UpgradeLevel lvl = upgrades[currentLevel];
        if (economy.getMoney() < lvl.cost)
        {
            Debug.Log("Not enough money to upgrade. Cost: " + lvl.cost);
            return;
        }

        economy.ChangeMoney(economy.getMoney() - lvl.cost);
        turret.ApplyUpgrade(lvl.rangeBonus, lvl.bpsBonus, lvl.damageBonus);
        currentLevel++;
        RefreshUI();
    }

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
