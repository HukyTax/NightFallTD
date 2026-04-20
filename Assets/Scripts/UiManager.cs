using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    private Button close;
    private Button open;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI healthText;
    Economy economy;
    HealthManager healthManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject panel;

    void Start()
    {
        GameObject LevelManager = GameObject.Find("LevelManager");
        economy = LevelManager.GetComponentInChildren<Economy>();
        healthManager = LevelManager.GetComponent<HealthManager>();

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
    }

    public void openShop()
    {
        panel.transform.position = new Vector3(panel.transform.position.x, panel.transform.position.y + 90);
        open.gameObject.SetActive(false);
        close.gameObject.SetActive(true);
    }
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
}
