using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private int maxHealth = 3;

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

    public void TakeDamage(int damage)
    {
        health -= damage;

        Debug.Log("Enemy is at " + health + " Health");

        if (health < 1)
            Die();
    }

    public void Die()
    {
        Destroy(this);
    }
}
