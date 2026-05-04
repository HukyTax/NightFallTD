using UnityEngine;

public class Economy : MonoBehaviour
{
   [SerializeField] private int money = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // dev tool
        if (Input.GetKeyDown(KeyCode.M))
        {
            money += 100;
        }
        
    }
    public void AddMoney(int moneyReward){
        money += moneyReward;
    }
    public int getMoney(){
        return money;
    }
    public void ChangeMoney(int money)
    {
        this.money = money;
    }


}
