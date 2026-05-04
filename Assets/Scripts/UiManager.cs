using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

// Updates the HUD every frame (money, health, wave counter) and controls
// the shop panel slide animation — open moves it up 90 units, close moves it back.
public class UiManager : MonoBehaviour
{
    // Toggle buttons found at runtime by searching the panel's children by name.
    private Button close;
    private Button open;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI roundText;
    Economy economy;
    HealthManager healthManager;
    enemySpawner enemyspawner;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject panel;

    void Start()
    {
        GameObject LevelManager = GameObject.Find("LevelManager");
        economy = LevelManager.GetComponentInChildren<Economy>();
        healthManager = LevelManager.GetComponent<HealthManager>();
        enemyspawner = LevelManager.GetComponent<enemySpawner>();

        // true = include inactive children, needed because the buttons start hidden.
        Button[] buttons = panel.GetComponentsInChildren<Button>(true);
        foreach (Button bt in buttons)
        {
            if (bt.name == "ToggelOpen")
            {
                open = bt;
            }
            if(bt.name == "ToggelClose")
            {
                close = bt;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMoney();
        UpdateHealth();
        UpdateRound();
    }

    // Slides the shop panel into view by moving it up 90 units.
    // Also swaps the visible toggle button.
    public void openShop()
    {
        panel.transform.position = new Vector3(panel.transform.position.x, panel.transform.position.y + 90);
        open.gameObject.SetActive(false);
        close.gameObject.SetActive(true);
    }

    // Slides the shop panel out of view by moving it down 90 units.
    public void closeShop()
    {
        panel.transform.position = new Vector3(panel.transform.position.x, panel.transform.position.y - 90);
        open.gameObject.SetActive(true);
        close.gameObject.SetActive(false);
    }

    public void UpdateMoney(){
        moneyText.text = ("Money:" + economy.getMoney());
    }
    public void UpdateHealth()
    {
        healthText.text = ("Health:" +healthManager.getHealth());
    }
    public void UpdateRound()
    {
        roundText.text = ("Round: " + enemyspawner.getWave());
    }
}
