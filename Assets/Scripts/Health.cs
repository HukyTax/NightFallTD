using UnityEngine;

public class Health : MonoBehaviour
{

    [SerializeField] private int hitPoints = 2;
    [SerializeField] private int moneyReward = 10;
    private bool isDestoryed = false;
    Economy economy;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void TakeDamage(int dmg)
    {
       hitPoints -= dmg;
       if(hitPoints <= 0 && !isDestoryed)
        {
            isDestoryed = true;
            enemySpawner.onEnemyDestroy.Invoke();
            economy.AddMoney(moneyReward);
            Destroy(gameObject);
        }
    }



    void Start()
    {
         GameObject LevelManager = GameObject.Find("LevelManager");
         economy = LevelManager.GetComponentInChildren<Economy>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public int GethitPoints()
    {
        return hitPoints;
    }
}
