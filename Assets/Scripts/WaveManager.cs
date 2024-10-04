using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Properties")]
    public int amountOfWaves = 3;

    [Header("References")]
    [SerializeField] private Spawner spawner;

    [HideInInspector] public int currentWave;
    private List<GameObject> enemies;
    private bool hasWon;

    // Start is called before the first frame update
    void Start()
    {
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
                Debug.Log("Win");
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
        for (int i =0; i < 10; i++)
        {
            enemies.Add(spawner.SpawnEnemy());
        }
    }

    public void EnemyDied(GameObject enemy)
    {
        enemies.Remove(enemy);
    }
}
