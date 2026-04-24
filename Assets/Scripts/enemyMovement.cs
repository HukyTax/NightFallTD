using System.Collections;
using UnityEngine;

public class enemyMovement : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Rigidbody2D rb;
    [Header("Attributes")]
    [SerializeField] private float movementSpeed = 2f;

    public HealthManager healthManager;
    public Health health;

    private Transform target;
    private int pathIndex = 0;
    private bool isSlowed = false;

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

    private void FixedUpdate()
    {
        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++;

            if (pathIndex == LevelManager.main.path.Length)
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

    public void ApplySlow(float factor, float duration)
    {
        if (!isSlowed)
        {
            StartCoroutine(SlowCoroutine(factor, duration));
        }
    }

    private IEnumerator SlowCoroutine(float factor, float duration)
    {
        isSlowed = true;
        float originalSpeed = movementSpeed;
        movementSpeed *= 1f - factor;
        yield return new WaitForSeconds(duration);
        movementSpeed = originalSpeed;
        isSlowed = false;
    }
}
