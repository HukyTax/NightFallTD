using UnityEngine;

// Tracks the player's base health — the thing enemies are trying to destroy.
// Enemies that reach the end of the path call updateHealth() with their remaining HP.
public class HealthManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] public int health = 100;
    levelLoader levelloader;
    void Start()
    {
        levelloader = gameObject.GetComponentInChildren<levelLoader>();
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            Debug.Log("GameOver");
            levelloader.gameOver();

        }
        // dev tool — Tab adds 25 HP so you can survive longer when testing late waves.
        if (Input.GetKeyDown(KeyCode.Tab)){
            health += 25;
        }
    }

    // Called by enemyMovement when an enemy leaks to the base.
    // `damage` is the enemy's remaining hitPoints — tougher surviving enemies hurt more.
    public void updateHealth(int damage)
    {
        health -= damage;
    }
    public int getHealth()
    {
        return health;
    }

}
