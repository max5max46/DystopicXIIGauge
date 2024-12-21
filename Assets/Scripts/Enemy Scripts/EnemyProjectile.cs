using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private float projectileLifeSpan = 2;
    [SerializeField] private Color deathParticleColor;

    [Header("References")]
    [SerializeField] protected GameObject dealthParticlePrefab;


    private float lifeSpanTimer;
    private bool isDead;

    [HideInInspector] public float projectileSpeed = 0;
    [HideInInspector] public int damage;

    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
        lifeSpanTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (lifeSpanTimer > projectileLifeSpan)
            Destroy(gameObject);

        lifeSpanTimer += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (!isDead)
            transform.position += transform.right * projectileSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead)
            return;

        if (collision.GetComponent<EnemyProjectile>() || collision.GetComponent<Enemy>())
            return;

        if (collision.GetComponent<Player>())
            collision.GetComponent<Player>().TakeDamage(damage);

        if (collision.GetComponent<ExplosiveBarrel>())
            if (!collision.GetComponent<ExplosiveBarrel>().isExploding)
                collision.GetComponent<ExplosiveBarrel>().Hit();

        isDead = true;

        GameObject deathParticle = Instantiate(dealthParticlePrefab, transform.position, transform.rotation);
        deathParticle.GetComponent<OneTimeParticle>().StartParticles(deathParticleColor);

        gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
    }

    public void Kill()
    {
        if (isDead)
            return;

        isDead = true;

        GameObject deathParticle = Instantiate(dealthParticlePrefab, transform.position, transform.rotation);
        deathParticle.GetComponent<OneTimeParticle>().StartParticles(deathParticleColor);

        gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
    }
}
