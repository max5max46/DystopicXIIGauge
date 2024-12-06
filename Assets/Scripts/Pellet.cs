using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Pellet : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private float projectileSpeed = 1;
    [SerializeField] private float projectileLifeSpan = 2;
    [SerializeField] private LayerMask raycastLayerMask;

    private float randomSpeedMultiplier;
    private float lifeSpanTimer;
    private bool isDead;
    private Vector3 previousPosition;

    [HideInInspector] public int damage;

    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
        lifeSpanTimer = 0;
        randomSpeedMultiplier = Random.Range(0.5f, 1.5f);
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
        {
            previousPosition = transform.position;

            if (!isDead)
            transform.position += transform.right * projectileSpeed * randomSpeedMultiplier;

            RaycastHit2D hit = Physics2D.Raycast(previousPosition, transform.position - previousPosition,Vector3.Distance(previousPosition, transform.position), raycastLayerMask);

            if (hit)
                Hit(hit);
        }
    }

    private void Hit(RaycastHit2D hit)
    {
        transform.position = hit.point;

        if (hit.transform.GetComponent<Enemy>())
            if (hit.transform.GetComponent<Enemy>().health > 0)
                hit.transform.GetComponent<Enemy>().TakeDamage(damage);

        if (hit.transform.GetComponent<EnemyProjectile>())
            hit.transform.GetComponent<EnemyProjectile>().Kill();

        isDead = true;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
    }
}
