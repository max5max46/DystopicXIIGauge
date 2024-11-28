using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_SecurityPyramid : Enemy
{
    [Header("Additional Properties")]
    [SerializeField] private LayerMask raycastLayerMask;

    [Header("Additional References")]
    [SerializeField] private GameObject projectile;


    private void Start()
    {

    }

    void Update()
    {
        attackCooldownTimer -= Time.deltaTime;

        if (state == EnemyState.Moving)
        {
            Movement();

            // Condition to switch to Attacking
            if (Vector2.Distance(player.transform.position, transform.position) < disFromPlayerToStartAttacking && 
                Physics2D.Raycast(transform.position, player.transform.position - transform.position, raycastLayerMask))
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

                // Condition to switch to Moving
                if (Vector2.Distance(player.transform.position, transform.position) > disFromPlayerToStartAttacking)
                {
                    agent.isStopped = false;
                    state = EnemyState.Moving;
                }

                attackCooldownTimer = attackCooldown;
                attackWindupTimer = attackWindup;
            }
        }
    }

    void Attack()
    {
        
    }
}
