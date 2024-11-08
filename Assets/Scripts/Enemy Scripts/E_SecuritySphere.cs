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

    // Update is called once per frame
    void Update()
    {
        attackCooldownTimer -= Time.deltaTime;

        if (isVisualAttackOn && attackCooldownTimer < 0)
            debugCircle.SetActive(false);

        if (state == EnemyState.Moving)
        {
            if (Vector2.Distance(player.transform.position, transform.position) < disFromPlayerToStartAttacking)
            {
                attackWindupTimer = attackWindup;
                state = EnemyState.Attacking;
            }
        }
        else if (state == EnemyState.Attacking)
        {
            attackWindupTimer -= Time.deltaTime;

            if (attackWindupTimer < 0 && attackCooldownTimer < 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRadius);

                foreach (var collider in colliders)
                {
                    if (collider.TryGetComponent<Player>(out Player player))
                    {
                        player.TakeDamage(damage);

                        if (isVisualAttackOn) 
                            debugCircle.SetActive(true);
                    }
                }

                if (Vector2.Distance(player.transform.position, transform.position) > disFromPlayerToStartAttacking)
                    state = EnemyState.Moving;

                attackCooldownTimer = attackCooldown;
                attackWindupTimer = attackWindup;
            }
        }
    }

    private void FixedUpdate()
    {
        if (state == EnemyState.Moving)
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            transform.position += (player.transform.position - transform.position).normalized * speed;
        }
        else if (state == EnemyState.Attacking)
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        }
    }
}
