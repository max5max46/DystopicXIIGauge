using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Properties")]
    public int amountOfWaves = 3;
    [SerializeField] private int pointsPerWave;

    [Header("References")]
    [SerializeField] private Spawner[] spawners;

    private RoundManager roundManager;
    [HideInInspector] public int currentWave;
    private List<GameObject> enemies;
    [HideInInspector] public bool hasWon;

    // Start is called before the first frame update
    void Start()
    {
        roundManager = FindFirstObjectByType<RoundManager>();
        enemies = new List<GameObject>();
        currentWave = 0;
        hasWon = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (enemies.Count == 0 && !hasWon)
        {
            currentWave++;

            if (currentWave > amountOfWaves)
            {
                currentWave--;
                roundManager.RoundEnd(true);
                hasWon = true;
            }
            else
            {
                StartWave();
            }
        }
    }

    void StartWave()
    {
        for (int i = 0; i < 10; i++)
        {
            enemies.Add(spawners[Random.Range(0, spawners.Length)].SpawnEnemy());
        }
    }

    public void EnemyDied(GameObject enemy)
    {
        enemies.Remove(enemy);
    }
}
