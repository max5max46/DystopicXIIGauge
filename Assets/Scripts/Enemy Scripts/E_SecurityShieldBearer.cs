using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_SecurityShieldBearer : Enemy
{
    [Header("Additional Properties")]
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float aimRotationSpeed;

    [Header("Additional References")]
    [SerializeField] private GameObject projectilePrefab;

    [Header("Additional Sound References")]
    [SerializeField] private AudioClip fireProjectile;

    private float currentAimAngle;
    private float goalAimAngle;


    private void Start()
    {
        soundHandler = FindAnyObjectByType<SoundHandler>();
        waveManager = FindFirstObjectByType<WaveManager>();
        player = FindFirstObjectByType<Player>().gameObject;


        Vector3 vectorToTarget = Quaternion.Euler(0, 0, 90) * (player.transform.position - transform.position);
        Quaternion lookDirection = Quaternion.LookRotation(Vector3.forward, vectorToTarget);
        goalAimAngle = lookDirection.eulerAngles.z;
        currentAimAngle = goalAimAngle;

    }

    void Update()
    {
        if (stunTimer < 0)
        {
            spriteRenderer.color = Color.white;

            if (attackCooldownTimer >= 0)
                attackCooldownTimer -= Time.deltaTime;
            else
            {
                Attack();
                attackCooldownTimer = attackCooldown;
            }

            Movement();
        }
        else
        {
            spriteRenderer.color = stunTint;
            agent.isStopped = true;
            stunTimer -= Time.deltaTime;
        }

    }

    private void FixedUpdate()
    {
        Vector3 vectorToTarget = Quaternion.Euler(0, 0, 90) * (player.transform.position - transform.position);
        Quaternion lookDirection = Quaternion.LookRotation(Vector3.forward, vectorToTarget);
        goalAimAngle = lookDirection.eulerAngles.z;


        float currentOppositeAimAngle = currentAimAngle + 180;
        if (currentOppositeAimAngle > 360)
            currentOppositeAimAngle -= 360;

        Debug.Log(currentOppositeAimAngle);

        if (currentAimAngle > 180)
        {
            if (goalAimAngle > currentAimAngle || goalAimAngle < currentOppositeAimAngle)
                currentAimAngle += aimRotationSpeed;

            if (goalAimAngle < currentAimAngle && goalAimAngle > currentOppositeAimAngle)
                currentAimAngle -= aimRotationSpeed;
        }
        else
        {
            if (goalAimAngle > currentAimAngle && goalAimAngle < currentOppositeAimAngle)
                currentAimAngle += aimRotationSpeed;

            if (goalAimAngle < currentAimAngle || goalAimAngle > currentOppositeAimAngle)
                currentAimAngle -= aimRotationSpeed;
        }

        if (currentAimAngle < 0)
            currentAimAngle += 360;

        if (currentAimAngle > 360)
            currentAimAngle -= 360; 
    }

    void Attack()
    {
        soundHandler.PlaySound(fireProjectile, 0.05f, transform.position);

        for (int i = 0; i < 5; i++)
        {
            GameObject projectile = Instantiate(projectilePrefab);

            projectile.GetComponent<EnemyShield>().damage = damage;
            projectile.GetComponent<EnemyShield>().projectileSpeed = projectileSpeed;

            projectile.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            projectile.transform.rotation = Quaternion.Euler(0, 0, (currentAimAngle - 30) + (15 * i));
        }
    }
}
