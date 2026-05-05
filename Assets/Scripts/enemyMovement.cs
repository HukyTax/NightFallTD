using System.Collections;
using UnityEngine;

// Moves an enemy along the waypoint path defined in LevelManager.
// Also owns the slow and flash-colour effects that turrets apply.
public class enemyMovement : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float movementSpeed = 2f;

    public HealthManager healthManager;
    public Health health;

    private Transform target;  // current waypoint the enemy is heading toward
    private int pathIndex = 0;
    private bool isSlowed = false;  // guard so multiple bullets can't stack slows

    // Called by enemySpawner after instantiation so the enemy can find scene references.
    // Can't use Start() for this because we need it set before the first FixedUpdate.
    public void set()
    {
        GameObject LevelManager = GameObject.Find("LevelManager");
        healthManager = LevelManager.GetComponentInChildren<HealthManager>();
        health = GetComponentInChildren<Health>();
    }

    // Helper that finds the SpriteRenderer whether it's on the root or a child object.
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
        // Snap to the next waypoint once close enough to avoid overshooting.
        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++;

            // Reached the end of the path — deal damage to the base equal to remaining HP,
            // then fire the destroy event so the spawner decrements its alive count.
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

    // Entry point for slow bullets. Ignored if already slowed so stacking doesn't break movement.
    public void ApplySlow(float factor, float duration)
    {
        if (!isSlowed)
            StartCoroutine(SlowCoroutine(factor, duration));
    }

    // Temporarily reduces speed by `factor` (0–1) for `duration` seconds,
    // then restores original speed. Blue tint shows the slow is active.
    private IEnumerator SlowCoroutine(float factor, float duration)
    {
        isSlowed = true;
        float originalSpeed = movementSpeed;
        movementSpeed *= 1f - factor;

        SpriteRenderer sr = GetSR();
        if (sr != null) sr.color = new Color(0.4f, 0.7f, 1f);

        yield return new WaitForSeconds(duration);

        // Re-fetch in case the sprite reference was rebuilt during the wait.
        sr = GetSR();
        if (sr != null) sr.color = Color.white;
        movementSpeed = originalSpeed;
        isSlowed = false;
    }

    // Brief colour flash used by AOE/bomb bullets to show explosion impact visually.
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
