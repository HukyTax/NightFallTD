using UnityEngine;

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
        // dev tool
        if (Input.GetKeyDown(KeyCode.Tab)){
            health += 25;
        }
    }
    public void updateHealth(int damage)
    {
        health -= damage;
    }
    public int getHealth()
    {
        return health;
    }

}
