using UnityEngine;

// Tracks the base's health (the thing enemies are trying to destroy).
// Enemies that reach the end of the path deal damage equal to their remaining HP.
// When health hits zero, triggers the Game Over scene load.
public class HealthManager : MonoBehaviour
{
    [SerializeField] public int health = 100;

    private levelLoader levelloader;

    void Start()
    {
        levelloader = gameObject.GetComponentInChildren<levelLoader>();
    }

    void Update()
    {
        if (health <= 0)
        {
            Debug.Log("GameOver! :(");
            levelloader.gameOver();
        }

        // Dev shortcut: Tab adds 25 HP so you can test later waves without dying.
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            health += 25;
        }
    }

    // Called by enemyMovement when an enemy leaks through the end of the path.
    // `damage` is the enemy's remaining hitPoints — tougher enemies that aren't
    // killed deal more base damage.
    public void updateHealth(int damage)
    {
        health -= damage;
    }

    public int GetHealth()
    {
        return health;
    }
}
