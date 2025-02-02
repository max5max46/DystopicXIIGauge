using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private float timeToExplode;
    [SerializeField] private float attackRadius;
    [SerializeField] private int damageDealtToEnemies;
    [SerializeField] private int damageDealtToPlayer;

    [Header("References")]
    [SerializeField] private GameObject explosionParticlePrefab;
    [SerializeField] private GameObject explosionRadiousVisual;
    [SerializeField] private GameObject sprite;

    [Header("Sound References")]
    [SerializeField] private AudioClip explosiveSound;

    private SoundHandler soundHandler;
    private float explosionTimer;
    private Vector3 originalScale;
    [HideInInspector] public bool isExploding;
    [HideInInspector] public bool isAboutToExplode;

    // Start is called before the first frame update
    void Start()
    {
        soundHandler = FindFirstObjectByType<SoundHandler>();

        explosionTimer = 0;
        isAboutToExplode = false;
        isExploding = false;

        originalScale = sprite.transform.localScale;

        explosionRadiousVisual.SetActive(false);
        explosionRadiousVisual.transform.localScale = new Vector3(attackRadius * 2, attackRadius * 2);
    }

    // Update is called once per frame
    void Update()
    {
        if (isAboutToExplode)
            explosionTimer -= Time.deltaTime;

        if (explosionTimer < 0 && !isExploding)
            Explode();
    }

    private void FixedUpdate()
    {
        if (isAboutToExplode)
        {
            float wiggleFactor = 0.1f * (1 - (explosionTimer / timeToExplode));
            sprite.transform.localPosition = new Vector3(Random.Range(-wiggleFactor, wiggleFactor), Random.Range(-wiggleFactor, wiggleFactor), 0);
            sprite.transform.localScale = originalScale * (1 + (0.1f * (1 - (explosionTimer / timeToExplode))));
        }
    }

    public void Hit()
    {
        if (isAboutToExplode)
            return;

        explosionRadiousVisual.SetActive(true);

        isAboutToExplode = true;
        explosionTimer = timeToExplode;
    }

    void Explode()
    {
        isExploding = true;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRadius);

        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<Player>(out Player player))
                player.TakeDamage(damageDealtToPlayer);

            if (collider.TryGetComponent<Enemy>(out Enemy enemy))
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

            if (collider != this && collider.TryGetComponent<ExplosiveBarrel>(out ExplosiveBarrel explosiveBarrel))
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

        soundHandler.PlaySound(explosiveSound, 0.6f, transform.position);

        GameObject explosionParticle = Instantiate(explosionParticlePrefab, transform.position, transform.rotation);
        explosionParticle.GetComponent<OneTimeParticle>().StartParticles(null, attackRadius);
        Destroy(gameObject);
    }
}
