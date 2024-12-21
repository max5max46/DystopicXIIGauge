using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_SecuritySphere : Enemy
{
    [Header("Additional Properties")]
    [SerializeField] private float attackRadius;
    [SerializeField] private float attackEndLag;

    [Header("Additional References")]
    [SerializeField] private GameObject slamAttackVisualPrefab;

    [Header("DEBUG")]
    [SerializeField] private bool isVisualAttackOn = false;

    private float attackEndLagTimer;
    private bool hasAttacked;

    private void Start()
    {
        hasAttacked = false;
        attackEndLagTimer = 0;

        soundHandler = FindAnyObjectByType<SoundHandler>();
        waveManager = FindFirstObjectByType<WaveManager>();
        player = FindFirstObjectByType<Player>().gameObject;

    }

    void Update()
    {
        if (stunTimer < 0)
        {
            spriteRenderer.color = Color.white;

            if (attackCooldownTimer >= 0)
                attackCooldownTimer -= Time.deltaTime;

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
                    if (attackEndLagTimer <= 0)
                    {
                        if (!hasAttacked)
                        {
                            Attack();
                            attackEndLagTimer = attackEndLag;
                            hasAttacked = true;

                        }
                        else
                        {
                            // Condition to switch to Moving
                            if (Vector2.Distance(player.transform.position, transform.position) > disFromPlayerToStartAttacking)
                            {
                                state = EnemyState.Moving;
                            }

                            attackCooldownTimer = attackCooldown;
                            attackWindupTimer = attackWindup;
                            hasAttacked = false;
                        }
                    }
                    else
                    {
                        attackEndLagTimer -= Time.deltaTime;
                    }
                }
                else
                {
                    // Condition to switch to Moving
                    if (Vector2.Distance(player.transform.position, transform.position) > disFromPlayerToStartAttacking)
                    {
                        state = EnemyState.Moving;
                    }
                }
            }
        }
        else
        {
            spriteRenderer.color = stunTint;
            agent.isStopped = true;
            stunTimer -= Time.deltaTime;
        }

    }

    void Attack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRadius);

        GameObject slamAttackVisaul = Instantiate(slamAttackVisualPrefab);
        slamAttackVisaul.transform.position = transform.position;
        slamAttackVisaul.transform.localScale = new Vector2(2 * attackRadius, 2 * attackRadius);

        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<Player>(out Player player))
            {
                player.TakeDamage(damage);
            }

            if (collider.TryGetComponent<ExplosiveBarrel>(out ExplosiveBarrel explosiveBarrel))
            {
                if (!explosiveBarrel.isExploding)
                {
                    explosiveBarrel.Hit();
                }
            }
        }
    }
}
