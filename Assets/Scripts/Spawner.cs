using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [HideInInspector] public GameObject player;
    [HideInInspector] public WaveManager waveManager;

    public GameObject SpawnEnemy(GameObject enemyPrefab)
    {
        GameObject enemy = Instantiate(enemyPrefab);
        enemy.GetComponent<Enemy>().player = player;
        enemy.GetComponent<Enemy>().waveManager = waveManager;
        enemy.transform.position = transform.position + new Vector3 (Random.Range(0, 0.05f), Random.Range(0, 0.05f), 0);
        return enemy;
    }
}
