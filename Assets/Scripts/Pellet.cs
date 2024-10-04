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

    // Start is called before the first frame update
    void Start()
    {
        lifeSpanTimer = 0;
        randomSpeedMultiplier = Random.Range(0.5f, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (lifeSpanTimer > projectileLifeSpan)
            Destroy(this.gameObject);

        lifeSpanTimer += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        transform.position += transform.right * projectileSpeed * randomSpeedMultiplier;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>())
        {
            other.GetComponent<Enemy>().TakeDamage(1);
            Destroy(this);
        }
    }
}
