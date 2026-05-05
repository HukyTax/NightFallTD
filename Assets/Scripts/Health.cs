using UnityEngine;

// Attached to every enemy prefab. Tracks hit points and handles death:
// awards money, fires the global onEnemyDestroy event so the spawner can
// decrement its alive-count, then destroys the GameObject.
public class Health : MonoBehaviour
{
    [SerializeField] private int hitPoints = 2;
    // How much gold the player earns when this enemy is killed (not when it reaches the base).
    [SerializeField] private int moneyReward = 10;

    // Guard flag — prevents TakeDamage from firing the destroy sequence twice
    // if two bullets hit on the same frame.
    private bool isDestoryed = false;

    private Economy economy;

    void Start()
    {
        // Economy lives as a child of the LevelManager GameObject.
        GameObject LevelManager = GameObject.Find("LevelManager");
        economy = LevelManager.GetComponentInChildren<Economy>();
    }

    // Called by Bullet on collision. Reduces HP and triggers death if HP hits 0.
    public void TakeDamage(int dmg)
    {
        hitPoints -= dmg;
        if (hitPoints <= 0 && !isDestoryed)
        {
            isDestoryed = true;
            // Notify the spawner so it can decrement enemiesAlive and end the wave.
            enemySpawner.onEnemyDestroy.Invoke();
            economy.AddMoney(moneyReward);
            Destroy(gameObject);
        }
    }

    void Update() { }

    // Used by enemyMovement to pass remaining HP as "damage" to HealthManager
    // when the enemy reaches the end of the path (leaked HP = base damage dealt).
    public int GethitPoints()
    {
        return hitPoints;
    }
}
