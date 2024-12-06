using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private float projectileLifeSpan = 2;


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
        if (collision.GetComponent<EnemyProjectile>() || collision.GetComponent<Enemy>())
            return;

        if (collision.GetComponent<Player>())
            collision.GetComponent<Player>().TakeDamage(damage);

        isDead = true;
        gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
    }

    public void Kill()
    {
        isDead = true;
        gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
    }
}
