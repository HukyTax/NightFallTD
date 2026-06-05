using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;

using UnityEditor;
using System;
using UnityEngine.Rendering.Universal;

// Core tower behaviour: scans for the nearest enemy each frame, rotates to track it,
// and fires a bullet at the configured rate. Stat upgrades are applied additively
// by TowerUpgrade when the player clicks the upgrade button.
public class Turret : MonoBehaviour
{
    // The child transform that physically rotates to face the target.
    [SerializeField] private Transform turretRotationPoint;

    [SerializeField] private float targetingRange = 5f;

    // Layer mask for enemy detection — keeps FindTarget() cheap by ignoring other physics layers.
    [SerializeField] private LayerMask enemyMask;

    [SerializeField] private GameObject bulletPreFab;

    // The point on the turret from which bullets are spawned.
    [SerializeField] private Transform firingPoint;

    [SerializeField] private float rotationSpeed = 200.0f;

    // Bullets per second. TimeUntilFire accumulates delta time and fires when it exceeds 1/bps.
    [SerializeField] private float bps = 1.2f;

    [SerializeField] private int bulletDamage = 1;
    private Boolean inLight = false;

    private Transform target;
    private float TimeUntilFire;
    private GameObject LightObj;
    private DayNightManager DayNightManagerScript;


    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, GetRange());
    }
    void Start()
    {
       LightObj = GameObject.Find("Light 2D");
       DayNightManagerScript = LightObj.GetComponent<DayNightManager>();

    }

    void Update()
    {
        if (DayNightManagerScript.GetIsNight())
        {
            
        }
        if (target == null)
        {
            FindTarget();
            return;
        }

        RotateToTarget();

        // Drop the target if it walked out of range between frames.
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
        // Push the current damage stat to the bullet so upgrades take effect immediately.
        bulletScript.SetDamage(bulletDamage);
        bulletScript.SetTarget(target);
        TimeUntilFire = 0f;
    }

    // gets the eninime furthest along the path. thats also within range
    private void FindTarget()
    {
        int furthest = -1;

        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, GetRange(), enemyMask);

        foreach (Collider2D c in hit)
        {
            enemyMovement e = c.GetComponent<enemyMovement>();
            if (e != null && e.GetPathIndex() > furthest)
            {
                furthest = e.GetPathIndex();
                target = c.transform;
            }
        }
    }

    //rotates the head of the turret to the unit
    private void RotateToTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y,
                                   target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private bool CheckIfTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= GetRange();
    }

    // Called by TowerUpgrade to additively apply a purchased upgrade tier.
    public void ApplyUpgrade(float rangeBonus, float bpsBonus, int damageBonus)
    {
        targetingRange += rangeBonus;
        bps += bpsBonus;
        bulletDamage += damageBonus;
    }



public float GetRange()
{
    if (!inLight && DayNightManagerScript.GetIsNight())
    {
        return targetingRange * .7f;
    }
    return targetingRange;
}
    public float GetBps() => bps;
    public int GetDamage() => bulletDamage;


    public void SetNightRangeBoost(Boolean inLight)
    {
        this.inLight = inLight;
    }

}
