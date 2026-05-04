using System.Collections;
using UnityEngine;

// Moves an enemy along the waypoint path stored in LevelManager.
// Also owns the slow effect (called by slow bullets) and the colour flash
// effect (called by AOE/bomb bullets to show explosion area).
public class enemyMovement : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Rigidbody2D rb;
    [Header("Attributes")]
    [SerializeField] private float movementSpeed = 2f;

    // Needed to deal damage to the player's base if this enemy reaches the end.
    public HealthManager healthManager;
    // Needed to read remaining HP when the enemy leaks — leftover HP = base damage.
    public Health health;

    private Transform target;   // the waypoint this enemy is currently heading toward
    private int pathIndex = 0;  // which waypoint in LevelManager.path we're aiming at
    private bool isSlowed = false; // guard so multiple slow bullets can't stack slows

    // Called by enemySpawner right after Instantiate to wire up scene references.
    // Done here instead of Start() because set() must run before the first FixedUpdate.
    public void set()
    {
        GameObject LevelManager = GameObject.Find("LevelManager");
        healthManager = LevelManager.GetComponentInChildren<HealthManager>();
        health = GetComponentInChildren<Health>();
    }

    // Looks for a SpriteRenderer on the root first, then on children.
    // Needed because enemy prefabs vary in whether the SR is on the root or a child.
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
        // When close enough to the current waypoint, advance to the next one.
        // The 0.1f threshold prevents the enemy from endlessly chasing an exact position.
        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++;

            // Reached the last waypoint — enemy has leaked through to the base.
            // Fire destroy event (so spawner decrements alive count), deal damage, then die.
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

    // Entry point for slow bullets. The isSlowed guard prevents multiple bullets
    // from stacking multiple SlowCoroutines on the same enemy.
    public void ApplySlow(float factor, float duration)
    {
        if (!isSlowed)
            StartCoroutine(SlowCoroutine(factor, duration));
    }

    // Temporarily reduces speed by `factor` (0 to 1 scale, e.g. 0.5 = 50% reduction)
    // for `duration` seconds, then restores original speed.
    private IEnumerator SlowCoroutine(float factor, float duration)
    {
        isSlowed = true;
        float originalSpeed = movementSpeed;
        movementSpeed *= 1f - factor;

        // Blue tint signals to the player that this enemy is slowed.
        SpriteRenderer sr = GetSR();
        if (sr != null) sr.color = new Color(0.4f, 0.7f, 1f);

        yield return new WaitForSeconds(duration);

        // Re-fetch SR in case it changed during the wait (e.g. pooling or child rebuild).
        sr = GetSR();
        if (sr != null) sr.color = Color.white;
        movementSpeed = originalSpeed;
        isSlowed = false;
    }

    // Brief colour flash used by AOE bullets to show which enemies were hit.
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
