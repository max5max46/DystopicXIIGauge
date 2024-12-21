using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_SecurityCylinder : Enemy
{
    [Header("Additional Properties")]
    [SerializeField] private float attackRadius;
    [SerializeField] private int damageDealtToEnemies;

    [Header("Additional References")]
    [SerializeField] private GameObject explosionParticlePrefab;
    [SerializeField] private GameObject explosionRadiousVisual;

    [Header("Additional Sound References")]
    [SerializeField] private AudioClip explosionSound;


    [HideInInspector] public bool isExploding;

    private void Start()
    {
        soundHandler = FindAnyObjectByType<SoundHandler>();
        waveManager = FindFirstObjectByType<WaveManager>();
        player = FindFirstObjectByType<Player>().gameObject;

        isExploding = false;

        explosionRadiousVisual.SetActive(false);
        explosionRadiousVisual.transform.localScale = new Vector3(attackRadius * 2, attackRadius * 2);
    }

    void Update()
    {
        if (attackCooldownTimer >= 0)
            attackCooldownTimer -= Time.deltaTime;

        if (attackCooldownTimer < 0)
            explosionRadiousVisual.SetActive(false);

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

            explosionRadiousVisual.SetActive(true);

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

        soundHandler.PlaySound(explosionSound, 0.2f, transform.position);

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

            if (collider.TryGetComponent<ExplosiveBarrel>(out ExplosiveBarrel explosiveBarrel))
            {
                if (!explosiveBarrel.isExploding)
                {
                    explosiveBarrel.Hit();
                }
            }

            if (collider.TryGetComponent<EnemyProjectile>(out EnemyProjectile projectile))
            {
                projectile.Kill();
            }
        }

        player.GetComponent<Player>().ReceivePartsInRun(amountOfParts);
        waveManager.EnemyDied(gameObject);

        GameObject explosionParticle = Instantiate(explosionParticlePrefab, transform.position, transform.rotation);
        explosionParticle.GetComponent<OneTimeParticle>().StartParticles(null, attackRadius);

        GameObject gsParticle = Instantiate(gsParticlePrefab, transform.position, transform.rotation);
        gsParticle.GetComponent<OneTimeParticle>().StartParticles();

        Destroy(gameObject);
    }

    public override void Die()
    {
        agent.isStopped = true;
        attackWindupTimer = attackWindup;
        state = EnemyState.Attacking;
    }
}
