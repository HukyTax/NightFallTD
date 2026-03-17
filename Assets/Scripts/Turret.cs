using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using Unity.Mathematics;
using System.Numerics;

public class Turret : MonoBehaviour
{
    [SerializeField] private Transform turretRotationPoint;

    [SerializeField] private float targetingRange = 5f;

    [SerializeField] private LayerMask enemyMask;


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
    }

    private void FindTarget()
    {
        RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)transform.position, 0f, enemyMask );
        if(hit.Length > 0){
            target = hit[0].transform;
        }
    }
    private void RotateToTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg;

        quaternion targetRotation = quaternion.Euler(new Vector3(0f,0f,angle));
        transform.rotation = targetRotation;

    }
}
