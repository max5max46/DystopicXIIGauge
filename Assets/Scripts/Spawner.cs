using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject enemyPrefab;

    private GameObject player;
    private WaveManager waveManager;

    // Start is called before the first frame update
    void Start()
    {
        player = FindFirstObjectByType<Player>().gameObject;
        waveManager = FindFirstObjectByType<WaveManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab);
        enemy.GetComponent<Enemy>().player = player;
        enemy.GetComponent<Enemy>().waveManager = waveManager;
        enemy.transform.position = transform.position + new Vector3 (Random.Range(0, 0.05f), Random.Range(0, 0.05f), 0);
        return enemy;
    }
}
