using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Pellet : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private float projectileSpeed = 1;
    [SerializeField] private float projectileLifeSpan = 2;

    private float randomSpeedMultiplier;
    private float lifeSpanTimer;

    private bool canMove;

    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
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
        if (canMove)
        transform.position += transform.right * projectileSpeed * randomSpeedMultiplier;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Pellet>())
            return;

        if (collision.GetComponent<Enemy>())
            if (collision.GetComponent<Enemy>().health > 0)
                collision.GetComponent<Enemy>().TakeDamage(1);

        canMove = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
    }
}
