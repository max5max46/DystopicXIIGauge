using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_SecurityCylinder : Enemy
{
    [Header("Additional Properties")]
    [SerializeField] private float attackRadius;
    [SerializeField] private int damageDealtToEnemies;

    [Header("Additional References")]
    [SerializeField] private GameObject debugCircle;

    [Header("DEBUG")]
    [SerializeField] private bool isVisualAttackOn = false;

    [HideInInspector] public bool isExploding;

    private void Start()
    {
        isExploding = false;

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

            if (isVisualAttackOn)
                debugCircle.SetActive(true);

            if (attackWindupTimer < 0 && attackCooldownTimer < 0)
            {
                Attack();
            }
        }
    }

    void Attack()
    {
        isExploding = true;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRadius);

        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<Player>(out Player player))
                player.TakeDamage(damage);

            if (collider != this && collider.TryGetComponent<Enemy>(out Enemy enemy))
            {
                if (collider.GetComponent<E_SecurityCylinder>())
                {
                    if (!collider.GetComponent<E_SecurityCylinder>().isExploding)
                    {
                        enemy.TakeDamage(damageDealtToEnemies);
                    }
                }
                else
                {
                    enemy.TakeDamage(damageDealtToEnemies);
                }
            }

            if (collider != this && collider.TryGetComponent<EnemyProjectile>(out EnemyProjectile projectile))
            {
                projectile.Kill();
            }
        }

        player.GetComponent<Player>().ReceivePartsInRun(amountOfParts);
        waveManager.EnemyDied(gameObject);
        Destroy(gameObject);
    }

    public override void Die()
    {
        state = EnemyState.Attacking;
    }
}
