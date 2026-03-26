using UnityEngine;

public class Health : MonoBehaviour
{

    [SerializeField] private int hitPoints = 2;
    private bool isDestoryed = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void TakeDamage(int dmg)
    {
       hitPoints -= dmg;
       if(hitPoints <= 0 && !isDestoryed)
        {
            enemySpawner.onEnemyDestroy.Invoke();
            Destroy(gameObject);
        } 
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
