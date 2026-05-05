using UnityEngine;

// Fired by Turret. Homes in on a target transform each FixedUpdate.
// Supports three modes set via Inspector on the bullet prefab:
//   - Standard: hits one enemy, optionally applies a slow
//   - AOE/Bomb: explodes in a radius on first enemy contact, flashes all hit enemies
//   - Slow-only: non-zero slowFactor with explosionRadius == 0
public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private int bulletDamage = 1;

    // Self-destructs if the target dies before the bullet arrives.
    [SerializeField] private float maxLifetime = 5f;

    // 0 = no slow. Factor is 0–1: 0.5 = 50% speed reduction.
    [SerializeField] private float slowFactor = 0f;
    [SerializeField] private float slowDuration = 2f;

    // 0 = single target. Any value > 0 enables the AOE explosion mode.
    [SerializeField] private float explosionRadius = 0f;

    private Transform target;
    private float lifetime;

    // Called by Turret immediately after Instantiate so the bullet knows what to chase.
    public void SetTarget(Transform _target) => target = _target;

    // Called by Turret to override the prefab's base damage (used for upgraded turrets).
    public void SetDamage(int dmg) => bulletDamage = dmg;

    void Start() => lifetime = 0f;

    void FixedUpdate()
    {
        // Stop moving if the target was destroyed (e.g. killed by another bullet).
        if (!target) return;
        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * bulletSpeed;
    }

    void Update()
    {
        lifetime += Time.deltaTime;

        // Destroy bullets that outlive their target or fly off-screen.
        // The ±25 bounds match the expected camera/map extents.
        if (lifetime >= maxLifetime || Mathf.Abs(transform.position.x) > 25 || Mathf.Abs(transform.position.y) > 25)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("enemy")) return;

        if (explosionRadius > 0f)
        {
            // AOE mode: scan all colliders in range regardless of layer — tag check
            // handles filtering so the bullet prefab doesn't need enemyMask wired up.
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
            foreach (Collider2D hit in hits)
            {
                if (!hit.CompareTag("enemy")) continue;

                Health h = hit.GetComponentInParent<Health>();
                if (h != null) h.TakeDamage(bulletDamage);

                // Orange flash lets the player see the explosion's area of effect.
                enemyMovement em = hit.GetComponentInParent<enemyMovement>();
                if (em != null) em.FlashColor(new Color(1f, 0.3f, 0f), 0.3f);
            }
        }
        else
        {
            // Single-target mode. GetComponentInParent works whether the enemy's
            // collider is on the root or a child object.
            Health health = collision.gameObject.GetComponentInParent<Health>();
            if (health != null) health.TakeDamage(bulletDamage);

            if (slowFactor > 0f)
            {
                enemyMovement movement = collision.gameObject.GetComponentInParent<enemyMovement>();
                if (movement != null) movement.ApplySlow(slowFactor, slowDuration);
            }
        }

        Destroy(gameObject);
    }
}
