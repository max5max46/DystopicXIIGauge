using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_HeadOfSecurity : Enemy
{
    enum BossState
    {
        Moving,
        SpinAttack,
        MeleeAttack
    }


    [Header("Additional Properties")]
    [SerializeField] private float attackRadius;
    [SerializeField] private float attackEndLag;
    [SerializeField] private float spinAttackMinCooldown;
    [SerializeField] private float spinAttackMaxCooldown;
    [SerializeField] private float timeBetweenVolleys;
    [SerializeField] private float amountOfVolleys;
    [SerializeField] private float projectileSpeed;

    [Header("Additional References")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject slamAttackVisualPrefab;

    [Header("Additional Sound References")]
    [SerializeField] private AudioClip fireProjectile;

    [Header("DEBUG")]
    [SerializeField] private bool isVisualAttackOn = false;

    private float attackEndLagTimer;
    private float spinAttackCooldownTimer;
    private float volleyTimer;
    private float volleyCount;
    private bool hasAttacked;
    private BossState bossState;

    private void Start()
    {
        hasAttacked = false;
        attackEndLagTimer = 0;
        volleyTimer = 0;
        spinAttackCooldownTimer = Random.Range(spinAttackMinCooldown, spinAttackMaxCooldown);

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

            switch (bossState)
            {
                case BossState.Moving:

                    Movement();

                    // Condition to switch to Spin Attack
                    if (spinAttackCooldownTimer < 0)
                    {
                        agent.isStopped = true;
                        bossState = BossState.SpinAttack;
                    }
                    else
                    {
                        spinAttackCooldownTimer -= Time.deltaTime;
                    }

                    // Condition to switch to Melee Attack
                    if (Vector2.Distance(player.transform.position, transform.position) < disFromPlayerToStartAttacking)
                    {
                        agent.isStopped = true;
                        attackWindupTimer = attackWindup;
                        bossState = BossState.MeleeAttack;
                    }

                    break;


                case BossState.MeleeAttack:

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
                                    bossState = BossState.Moving;
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
                            bossState = BossState.Moving;
                        }
                    }

                    break;


                case BossState.SpinAttack:

                    if (volleyTimer < 0)
                    {
                        if (volleyCount > amountOfVolleys)
                        {
                            bossState = BossState.Moving;
                            spinAttackCooldownTimer = Random.Range(spinAttackMinCooldown, spinAttackMaxCooldown);
                            volleyCount = 0;
                        }
                        else
                        {
                            for (int i = 0; i < 8; i++)
                            {
                                //soundHandler.PlaySound(fireProjectile, 0.2f, transform.position);

                                GameObject projectile = Instantiate(projectilePrefab);

                                projectile.GetComponent<EnemyProjectile>().damage = damage;
                                projectile.GetComponent<EnemyProjectile>().projectileSpeed = projectileSpeed;

                                projectile.transform.position = new Vector3(transform.position.x, transform.position.y, 0);

                                projectile.transform.rotation = Quaternion.Euler(0, 0, (45 * i) + (15 * volleyCount));
                            }

                            volleyCount++;
                            volleyTimer = timeBetweenVolleys;
                        }
                    }
                    else
                    {
                        volleyTimer -= Time.deltaTime;
                    }

                    break;

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
