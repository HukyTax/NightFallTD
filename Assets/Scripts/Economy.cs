using UnityEngine;

// Manages the player's gold balance.
// Sits as a child of the LevelManager GameObject so other scripts
// can find it via GetComponentInChildren<Economy>().
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
        // dev tool — M adds 100 gold to test tower purchases without grinding waves.
        if (Input.GetKeyDown(KeyCode.M))
        {
            money += 100;
        }

    }

    // Called by Health when an enemy is killed (not when it leaks to the base).
    public void AddMoney(int moneyReward){
        money += moneyReward;
    }
    public int getMoney(){
        return money;
    }

    // Sets gold to a new absolute value — NOT a delta.
    // Callers must compute the new total themselves, e.g. ChangeMoney(getMoney() - price).
    public void ChangeMoney(int money)
    {
        this.money = money;
    }


}
