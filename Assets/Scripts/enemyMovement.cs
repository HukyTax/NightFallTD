using System.IO;
using UnityEngine;

public class enemyMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [Header("Referance")]
    [SerializeField] private Rigidbody2D rb;
    [Header("Atrubites")]
    [SerializeField] private float movementSpeed = 2f;

    public HealthManager healthManager;
    public Health health;

    private Transform target;
    private int pathIndex = 0;

    public void set()
    {
        GameObject LevelManager = GameObject.Find("LevelManager");
         healthManager = LevelManager.GetComponentInChildren<HealthManager>();
         health = GetComponentInChildren<Health>();
    }

    void Start()
    {
        target = LevelManager.main.path[pathIndex];
        set();
    }

    // Update is called once per frame
 
    private void FixedUpdate()
    {
        if(Vector2.Distance(target.position,transform.position) <= 0.1f)
        {
            pathIndex++;
            
            if(pathIndex == LevelManager.main.path.Length)
            {   
                enemySpawner.onEnemyDestroy.Invoke();
                healthManager.updateHealth(health.GethitPoints());
                Destroy(gameObject);
                return;
            }

            target = LevelManager.main.path[pathIndex];
        }

        Vector2 direction = ((Vector2)target.position - (Vector2)transform.position).normalized;
        rb.linearVelocity = direction * movementSpeed;
    }
}
