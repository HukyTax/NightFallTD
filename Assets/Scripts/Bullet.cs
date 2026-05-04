using UnityEngine;

// Fired by Turret. Homes in on its assigned target every FixedUpdate.
// Supports three modes controlled by Inspector values on the bullet prefab:
//   Standard   — hits one enemy, optional slow effect
//   AOE/Bomb   — explodes in a radius on first contact, damages all enemies in range
//   Slow-only  — non-zero slowFactor with explosionRadius == 0
public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private int bulletDamage = 1;
    // Bullet self-destructs after this many seconds — catches cases where the target
    // dies before the bullet arrives and it would fly forever.
    [SerializeField] private float maxLifetime = 5f;
    // 0 = no slow applied. 0–1 scale: 0.5 = 50% speed reduction.
    [SerializeField] private float slowFactor = 0f;
    [SerializeField] private float slowDuration = 2f;
    // 0 = single target mode. Any value > 0 switches to AOE explosion mode.
    [SerializeField] private float explosionRadius = 0f;

    private Transform target;
    private float lifetime;

    // Called by Turret immediately after Instantiate to assign the homing target.
    public void SetTarget(Transform _target) => target = _target;
    // Called by Turret to override the prefab's default damage with the turret's current stat.
    public void SetDamage(int dmg) => bulletDamage = dmg;

    void Start() => lifetime = 0f;

    void FixedUpdate()
    {
        // If the target was destroyed (killed by another bullet), stop moving.
        if (!target) return;
        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * bulletSpeed;
    }

    void Update()
    {
        lifetime += Time.deltaTime;
        // Self-destruct if the bullet outlives its max lifetime or flies off the map.
        // The ±25 bounds are hardcoded to the expected playfield size.
        if (lifetime >= maxLifetime || Mathf.Abs(transform.position.x) > 25 || Mathf.Abs(transform.position.y) > 25)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Ignore collisions with anything that isn't tagged "enemy".
        if (!collision.gameObject.CompareTag("enemy")) return;

        if (explosionRadius > 0f)
        {
            // Use tag-based detection so enemyMask doesn't need to be set on every bomb bullet
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
            foreach (Collider2D hit in hits)
            {
                if (!hit.CompareTag("enemy")) continue;

                Health h = hit.GetComponentInParent<Health>();
                if (h != null) h.TakeDamage(bulletDamage);

                // Flash red so the player can see the explosion area
                enemyMovement em = hit.GetComponentInParent<enemyMovement>();
                if (em != null) em.FlashColor(new Color(1f, 0.3f, 0f), 0.3f);
            }
        }
        else
        {
            // GetComponentInParent works whether the collider is on the root or a child
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
