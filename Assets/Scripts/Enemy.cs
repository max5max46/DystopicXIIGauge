using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int damage = 1;
    [SerializeField] private float speed = 0.1f;
    [SerializeField] private int amountOfParts = 100;

    private GameObject player;
    private WaveManager waveManager;

    private int health;

    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<Player>().gameObject;
        waveManager = FindAnyObjectByType<WaveManager>();
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
        if (health < 1)
            return;

        health -= damage;

        if (health < 1)
            Die();
    }

    public void Die()
    {
        player.GetComponent<Player>().ReceiveParts(amountOfParts);
        waveManager.EnemyDied(gameObject);
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
