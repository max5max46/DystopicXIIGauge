using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyShield : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private float projectileLifeSpan = 2;
    [SerializeField] private Color deathParticleColor;

    [Header("References")]
    [SerializeField] protected GameObject dealthParticlePrefab;


    private float lifeSpanTimer;

    [HideInInspector] public float projectileSpeed = 0;
    [HideInInspector] public int damage;

    // Start is called before the first frame update
    void Start()
    {
        lifeSpanTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (lifeSpanTimer > projectileLifeSpan)
            Kill();

        lifeSpanTimer += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        transform.position += transform.right * (projectileSpeed * (1 - (lifeSpanTimer / projectileLifeSpan)));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyProjectile>() || collision.GetComponent<Enemy>() || collision.GetComponent<EnemyShield>() || collision.GetComponent<Pellet>() || collision.GetComponent<ExplosiveBarrel>())
            return;

        if (collision.GetComponent<Player>())
            collision.GetComponent<Player>().TakeDamage(damage);

        Kill();
    }

    public void Kill()
    {
        GameObject deathParticle = Instantiate(dealthParticlePrefab, transform.position, transform.rotation);
        deathParticle.GetComponent<OneTimeParticle>().StartParticles(deathParticleColor);

        Destroy(gameObject);
    }
}
