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
    void Update()
    {
        if(Vector2.Distance(target.position,transform.position) <= 0.1f)
        {
            pathIndex++;
            
            if(pathIndex == LevelManager.main.path.Length)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                target = LevelManager.main.path[pathIndex];
            }
        }
    }
    private void FixedUpdate()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * movementSpeed;
    }
}
