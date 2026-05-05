using UnityEngine;

// Owns the player's gold balance. Lives as a child of the LevelManager GameObject
// so any script can reach it via GetComponentInChildren<Economy>().
public class Economy : MonoBehaviour
{
    [SerializeField] private int money = 0;

    void Update()
    {
        // Dev shortcut: M adds 100 gold for testing purchases without grinding waves.
        if (Input.GetKeyDown(KeyCode.M))
        {
            money += 100;
        }
    }

    // Called by Health when an enemy is killed (not when it leaks).
    public void AddMoney(int moneyReward)
    {
        money += moneyReward;
    }

    public int GetMoney()
    {
        return money;
    }

    // Used by TowerDragHandler and TowerUpgrade to deduct purchase costs.
    // Takes the new absolute value rather than a delta — caller computes getMoney() - cost.
    public void ChangeMoney(int money)
    {
        this.money = money;
    }
}
