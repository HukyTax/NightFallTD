using UnityEngine;

// Attached to every enemy prefab.
// Tracks hit points, awards gold on kill, and fires the global onEnemyDestroy
// event so the wave spawner knows one fewer enemy is alive.
public class Health : MonoBehaviour
{

    [SerializeField] private int hitPoints = 2;
    // Gold given to the player when this enemy is killed (not when it leaks to the base).
    [SerializeField] private int moneyReward = 10;
    // Guard flag — prevents the death sequence from firing twice if two bullets
    // hit the same enemy on the exact same frame before Destroy processes.
    private bool isDestroyed = false;
    Economy economy;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void TakeDamage(int dmg)
    {
       hitPoints -= dmg;
       if(hitPoints <= 0 && !isDestroyed)
        {
            isDestroyed = true;
            // Notify the spawner that this enemy is gone — it decrements its alive counter.
            enemySpawner.onEnemyDestroy.Invoke();
            economy.AddMoney(moneyReward);
            Destroy(gameObject);
        }
    }



    void Start()
    {
         // Economy is a child component of LevelManager, not on the root GameObject itself.
         GameObject LevelManager = GameObject.Find("LevelManager");
         economy = LevelManager.GetComponentInChildren<Economy>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Returns remaining HP. Used by enemyMovement when the enemy leaks through the end
    // of the path — the leftover HP is treated as damage dealt to the player's base.
    public int GethitPoints()
    {
        return hitPoints;
    }
}
