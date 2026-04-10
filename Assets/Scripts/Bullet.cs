using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private int bulletDamage = 1;

    private Transform target;

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    void Start() { }

    void FixedUpdate()
    {
        // If the enemy was destroyed before impact, stop the bullet.
        if (!target) return;

        // Recalculate direction every tick so the bullet curves toward a moving enemy.
        // Normalizing gives a unit vector so bulletSpeed fully controls the magnitude.
        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * bulletSpeed;


    }

    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
        {
           collision.gameObject.GetComponent<Health>().TakeDamage(bulletDamage);
            Destroy(gameObject); 
        }
    }
}
