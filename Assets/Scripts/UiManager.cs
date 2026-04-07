using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    private Button close;
    private Button open;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject panel;

    void Start()
    {
        Button[] buttons = panel.GetComponentsInChildren<Button>();
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
        
    }

    public void openShop()
    {
        panel.transform.position = new Vector3(panel.transform.position.x, panel.transform.position.y + 5);
        open.gameObject.setActive(false);
        close.gameObject.setActive(true);
    }
    public void closeShop()
    {
        panel.transform.position = new Vector3(panel.transform.position.x, panel.transform.position.y - 5);
        open.gameObject.setActive(true);
        close.gameObject.setActive(false);
    }
}
