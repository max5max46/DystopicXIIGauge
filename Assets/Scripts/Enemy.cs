using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int damage = 1;
    [SerializeField] private float speed = 0.1f;

    [Header("References")]
    [SerializeField] private GameObject player;

    private int health;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        transform.position += (player.transform.position - transform.position).normalized * speed;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health < 1)
            Die();
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(damage);
        }
    }
}
