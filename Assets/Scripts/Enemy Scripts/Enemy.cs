using NavMeshPlus.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    protected enum EnemyState
    {
        Moving,
        Attacking
    }

    [Header("Properties")]
    [SerializeField] protected int maxHealth;
    [SerializeField] protected int damage;
    [SerializeField] protected float attackCooldown;
    [SerializeField] protected float attackWindup;
    [SerializeField] protected float disFromPlayerToStartAttacking;
    [SerializeField] protected float stunTime;
    [SerializeField] protected int amountOfParts;
    public Color deathParticleColor;

    [Header("References")]
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected GameObject dealthParticlePrefab;
    [SerializeField] protected GameObject gsParticlePrefab;

    [Header("Sound References")]
    [SerializeField] private AudioClip enemyDeathSound;

    [HideInInspector] public SoundHandler soundHandler;
    [HideInInspector] public GameObject player;
    [HideInInspector] public  WaveManager waveManager;
    [HideInInspector] public int health;
    [HideInInspector] public bool isDead = false;

    protected NavMeshAgent agent;
    protected EnemyState state;
    protected float attackCooldownTimer;
    protected float attackWindupTimer;
    protected float stunTimer;

    protected Color stunTint = new Color(0.7f, 0.7f, 0.7f);

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;


        attackCooldownTimer = 0;
        attackWindupTimer = 0;
        state = EnemyState.Moving;
        health = maxHealth;
    }

    protected virtual void Movement()
    {
        agent.isStopped = false;
        agent.SetDestination(player.transform.position);
    }

    public void TakeDamage(int damage)
    {
        if (health < 1)
            return;

        health -= damage;
        stunTimer = stunTime;

        if (health < 1)
            Die();
    }

    public virtual void Die()
    {
        player.GetComponent<Player>().ReceivePartsInRun(amountOfParts);
        waveManager.EnemyDied(gameObject);

        soundHandler.PlaySound(enemyDeathSound, 0.2f, transform.position);

        GameObject deathParticle = Instantiate(dealthParticlePrefab, transform.position, transform.rotation);
        deathParticle.GetComponent<OneTimeParticle>().StartParticles(deathParticleColor);

        GameObject gsParticle = Instantiate(gsParticlePrefab, transform.position, transform.rotation);
        gsParticle.GetComponent<OneTimeParticle>().StartParticles();

        Destroy(gameObject);

    }
}
