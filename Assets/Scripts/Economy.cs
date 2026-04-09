using UnityEngine;

public class Economy : MonoBehaviour
{
   int money = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddMoney(int moneyReward){
        money += moneyReward;
    }
    public int getMoney(){
        return money;
    }


}
