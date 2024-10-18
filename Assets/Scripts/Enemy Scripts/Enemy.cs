using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] protected float speed;
    [SerializeField] protected int amountOfParts;

    [HideInInspector] public GameObject player;
    [HideInInspector] public  WaveManager waveManager;

    [HideInInspector] public int health;
    protected EnemyState state;
    protected float attackCooldownTimer;
    protected float attackWindupTimer;
    [HideInInspector] public bool isDead = false;

    void Awake()
    {
        attackCooldownTimer = 0;
        attackWindupTimer = 0;
        state = EnemyState.Moving;
        health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (health < 1)
            return;

        health -= damage;

        if (health < 1)
            Die();
    }

    public void Die()
    {
        
        player.GetComponent<Player>().ReceiveParts(amountOfParts);
        waveManager.EnemyDied(gameObject);
        Destroy(gameObject);
    }
}
