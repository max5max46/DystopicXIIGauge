using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class E_SecurityPyramid : Enemy
{
    [Header("Additional Properties")]
    [SerializeField] private LayerMask raycastLayerMask;
    [SerializeField] private Color projectileColor;

    [Header("Additional References")]
    [SerializeField] private GameObject projectilePrefab;


    void Update()
    {
        attackCooldownTimer -= Time.deltaTime;

        if (state == EnemyState.Moving)
        {
            Movement();

            // Condition to switch to Attacking
            if (Vector2.Distance(player.transform.position, transform.position) < disFromPlayerToStartAttacking && 
                Physics2D.Raycast(transform.position, player.transform.position - transform.position, 50, raycastLayerMask).transform.name == "Player")
            {
                agent.isStopped = true;
                attackWindupTimer = attackWindup;
                state = EnemyState.Attacking;
            }
        }
        else if (state == EnemyState.Attacking)
        {
            attackWindupTimer -= Time.deltaTime;

            if (attackWindupTimer < 0 && attackCooldownTimer < 0)
            {
                Attack();

                attackCooldownTimer = attackCooldown;
                attackWindupTimer = attackWindup;
            }

            // Condition to switch to Moving
            if (Vector2.Distance(player.transform.position, transform.position) > disFromPlayerToStartAttacking ||
                Physics2D.Raycast(transform.position, player.transform.position - transform.position, 50, raycastLayerMask).transform.name != "Player")
            {
                agent.isStopped = false;
                state = EnemyState.Moving;
            }
        }
    }

    void Attack()
    {
        GameObject projectile = Instantiate(projectilePrefab);
        projectile.GetComponent<EnemyProjectile>().damage = damage;
        projectile.GetComponent<SpriteRenderer>().color = projectileColor;
        projectile.transform.position = new Vector3 (transform.position.x , transform.position.y, 0);

        Vector3 vectorToTarget = Quaternion.Euler(0, 0, 90) * (player.transform.position - transform.position);
        Quaternion lookDirection = Quaternion.LookRotation(Vector3.forward, vectorToTarget);

        projectile.transform.rotation = Quaternion.Euler(0, 0, lookDirection.eulerAngles.z);
        
    }
}
