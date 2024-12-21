using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class E_SecurityPyramid : Enemy
{
    [Header("Additional Properties")]
    [SerializeField] private LayerMask raycastLayerMask;
    [SerializeField] private float projectileSpeed;

    [Header("Additional References")]
    [SerializeField] private GameObject projectilePrefab;

    [Header("Additional Sound References")]
    [SerializeField] private AudioClip fireProjectile;

    private void Start()
    {
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
                    state = EnemyState.Moving;
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
        soundHandler.PlaySound(fireProjectile, 0.2f, transform.position);

        GameObject projectile = Instantiate(projectilePrefab);

        projectile.GetComponent<EnemyProjectile>().damage = damage;
        projectile.GetComponent<EnemyProjectile>().projectileSpeed = projectileSpeed;
        
        projectile.transform.position = new Vector3 (transform.position.x , transform.position.y, 0);

        Vector3 vectorToTarget = Quaternion.Euler(0, 0, 90) * (player.transform.position - transform.position);
        Quaternion lookDirection = Quaternion.LookRotation(Vector3.forward, vectorToTarget);
        projectile.transform.rotation = Quaternion.Euler(0, 0, lookDirection.eulerAngles.z);

        
    }
}
