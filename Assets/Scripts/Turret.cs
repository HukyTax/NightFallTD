using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using Unity.Mathematics;
using System.Numerics;
using UnityEngine.InputSystem.Controls;

public class Turret : MonoBehaviour
{
    [SerializeField] private Transform turretRotationPoint;

    [SerializeField] private float targetingRange = 5f;

    [SerializeField] private LayerMask enemyMask;

    [SerializeField] private float rotationSpeed = 200.0f;


    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }

    private Transform target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null)
        {
            FindTarget();
            return;
        }
        RotateToTarget();

        if (!CheckIfTargetIsInRange())
        {
            target = null;
        }
    }

    private void FindTarget()
    {
        RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, targetingRange, (UnityEngine.Vector2) transform.position, 0f, enemyMask );
        if(hit.Length > 0){
            target = hit[0].transform;
        }
    }
    private void RotateToTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;

        UnityEngine.Quaternion targetRotation = UnityEngine.Quaternion.Euler(new UnityEngine.Vector3(0f,0f,angle));
        turretRotationPoint.rotation = UnityEngine.Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);

    }
    private bool CheckIfTargetIsInRange()
    {
        return UnityEngine.Vector2.Distance(target.position, transform.position) <= targetingRange;
    }
}
