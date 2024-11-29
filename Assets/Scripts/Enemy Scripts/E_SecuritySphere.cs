using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_SecuritySphere : Enemy
{
    [Header("Additional Properties")]
    [SerializeField] private float attackRadius;

    [Header("Additional References")]
    [SerializeField] private GameObject debugCircle;

    [Header("DEBUG")]
    [SerializeField] private bool isVisualAttackOn = false;

    private void Start()
    {
        if (isVisualAttackOn)
        {
            debugCircle.SetActive(false);
            debugCircle.transform.localScale = new Vector3(attackRadius * 2, attackRadius * 2);
        }
    }

    void Update()
    {
        attackCooldownTimer -= Time.deltaTime;

        if (isVisualAttackOn && attackCooldownTimer < 0)
            debugCircle.SetActive(false);

        if (state == EnemyState.Moving)
        {
            Movement();

            // Condition to switch to Attacking
            if (Vector2.Distance(player.transform.position, transform.position) < disFromPlayerToStartAttacking)
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
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRadius);

        if (isVisualAttackOn)
            debugCircle.SetActive(true);

        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<Player>(out Player player))
            {
                player.TakeDamage(damage);
            }
        }
    }
}
