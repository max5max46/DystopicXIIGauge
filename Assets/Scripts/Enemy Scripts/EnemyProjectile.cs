using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private float projectileSpeed = 1;
    [SerializeField] private float projectileLifeSpan = 2;

    private float lifeSpanTimer;
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
            Destroy(gameObject);

        lifeSpanTimer += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        transform.position += transform.right * projectileSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyProjectile>() || collision.GetComponent<Enemy>())
            return;


        if (collision.GetComponent<Player>())
            collision.GetComponent<Player>().TakeDamage(damage);


        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
    }

    public void Kill()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
    }
}
