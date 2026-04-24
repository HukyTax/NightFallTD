using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private int bulletDamage = 1;
    [SerializeField] private float maxLifetime = 5f;
    [SerializeField] private float slowFactor = 0f;
    [SerializeField] private float slowDuration = 2f;
    [SerializeField] private float explosionRadius = 0f;
    [SerializeField] private LayerMask enemyMask;

    private Transform target;
    private float lifetime;

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    public void SetDamage(int dmg)
    {
        bulletDamage = dmg;
    }

    void Start()
    {
        lifetime = 0f;
    }

    void FixedUpdate()
    {
        if (!target) return;

        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * bulletSpeed;
    }

    void Update()
    {
        lifetime += Time.deltaTime;
        if (lifetime >= maxLifetime)
        {
            Destroy(gameObject);
            return;
        }

        if (Mathf.Abs(transform.position.x) > 25 || Mathf.Abs(transform.position.y) > 25)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("enemy")) return;

        if (explosionRadius > 0f)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyMask);
            foreach (Collider2D hit in hits)
            {
                Health h = hit.GetComponent<Health>();
                if (h != null) h.TakeDamage(bulletDamage);
            }
        }
        else
        {
            Health health = collision.gameObject.GetComponent<Health>();
            if (health != null) health.TakeDamage(bulletDamage);

            if (slowFactor > 0f)
            {
                enemyMovement movement = collision.gameObject.GetComponent<enemyMovement>();
                if (movement != null) movement.ApplySlow(slowFactor, slowDuration);
            }
        }

        Destroy(gameObject);
    }
}
