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

    private SpriteRenderer GetSR()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) sr = GetComponentInChildren<SpriteRenderer>();
        return sr;
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
            StartCoroutine(SlowCoroutine(factor, duration));
    }

    private IEnumerator SlowCoroutine(float factor, float duration)
    {
        isSlowed = true;
        float originalSpeed = movementSpeed;
        movementSpeed *= 1f - factor;

        SpriteRenderer sr = GetSR();
        if (sr != null) sr.color = new Color(0.4f, 0.7f, 1f);

        yield return new WaitForSeconds(duration);

        sr = GetSR();
        if (sr != null) sr.color = Color.white;
        movementSpeed = originalSpeed;
        isSlowed = false;
    }

    public void FlashColor(Color color, float duration)
    {
        StartCoroutine(FlashCoroutine(color, duration));
    }

    private IEnumerator FlashCoroutine(Color color, float duration)
    {
        SpriteRenderer sr = GetSR();
        if (sr == null) yield break;
        Color original = sr.color;
        sr.color = color;
        yield return new WaitForSeconds(duration);
        sr = GetSR();
        if (sr != null) sr.color = original;
    }
}
