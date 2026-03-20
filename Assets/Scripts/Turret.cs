using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using Unity.Mathematics;

public class Turret : MonoBehaviour
{
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPreFab;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private float rotationSpeed = 200.0f;
    [SerializeField] private float bps = 1.2f; // bullets per second

    private Transform target;
    private float TimeUntilFire;

    // Draws the targeting range as a circle in the Scene view when selected.
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }

    void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }

        RotateToTarget();

        if (!CheckIfTargetIsInRange())
        {
            target = null;
        }
        else
        {
            // Accumulate time and fire once enough has passed for the current bps rate.
            TimeUntilFire += Time.deltaTime;
            if (TimeUntilFire >= 1f / bps)
            {
                Shoot();
            }
        }
    }

    private void Shoot()
    {
        Debug.Log("Shoot");
        GameObject bulletObj = Instantiate(bulletPreFab, firingPoint.position, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        TimeUntilFire = 0f;
        bulletScript.SetTarget(target);
    }

    private void FindTarget()
    {
        // Cast an overlap circle using only the enemy layer to avoid hitting other colliders.
        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, targetingRange, enemyMask);
        if (hit.Length > 0)
        {
            target = hit[0].transform;
        }
    }

    private void RotateToTarget()
    {
        // Atan2 gives the angle (in radians) toward the target; -90 corrects for the
        // sprite's default "up" orientation so the barrel points at the enemy.
        float angle = Mathf.Atan2(target.position.y - transform.position.y,
                                   target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private bool CheckIfTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }
}
