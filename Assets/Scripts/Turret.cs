using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Turret : MonoBehaviour
{
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPreFab;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private float rotationSpeed = 200.0f;
    [SerializeField] private float bps = 1.2f;
    [SerializeField] private int bulletDamage = 1;

    private Transform target;
    private float TimeUntilFire;

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
#endif

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
            TimeUntilFire = 0f;
        }
        else
        {
            TimeUntilFire += Time.deltaTime;
            if (TimeUntilFire >= 1f / bps)
            {
                Shoot();
            }
        }
    }

    private void Shoot()
    {
        GameObject bulletObj = Instantiate(bulletPreFab, firingPoint.position, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.SetDamage(bulletDamage);
        bulletScript.SetTarget(target);
        TimeUntilFire = 0f;
    }

    private void FindTarget()
    {
        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, targetingRange, enemyMask);
        if (hit.Length > 0)
        {
            target = hit[0].transform;
        }
    }

    private void RotateToTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y,
                                   target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private bool CheckIfTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    public void ApplyUpgrade(float rangeBonus, float bpsBonus, int damageBonus)
    {
        targetingRange += rangeBonus;
        bps += bpsBonus;
        bulletDamage += damageBonus;
    }

    public float GetRange() => targetingRange;
    public float GetBps() => bps;
    public int GetDamage() => bulletDamage;
}
