using System.IO;
using UnityEngine;

public class enemyMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [Header("Referance")]
    [SerializeField] private Rigidbody2D rb;
    [Header("Atrubites")]
    [SerializeField] private float movementSpeed = 2f;

    private Transform target;
    private int pathIndex = 0;

    void Start()
    {
        target = LevelManager.main.path[pathIndex];
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
                Destroy(gameObject);
                return;
            }

            target = LevelManager.main.path[pathIndex];
        }

        Vector2 direction = ((Vector2)target.position - (Vector2)transform.position).normalized;
        rb.linearVelocity = direction * movementSpeed;
    }
}
