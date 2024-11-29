using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Properties")]
    public List<int> wavePoints;
    [SerializeField] private int gsPerWave;

    [Header("Enemy Point Values")]
    [SerializeField] private int securitySpherePointVal;
    [SerializeField] private int securityPyramidPointVal;
    [SerializeField] private int supSecuritySpherePointVal;
    [SerializeField] private int supSecurityPyramidPointVal;
    [SerializeField] private int securityCylinderPointVal;

    [Header("Enemy Wave to Begin Spawning")]
    [SerializeField] private int securitySphereWaveToBeginSpawning;
    [SerializeField] private int securityPyramidWaveToBeginSpawning;
    [SerializeField] private int supSecuritySphereWaveToBeginSpawning;
    [SerializeField] private int supSecurityPyramidWaveToBeginSpawning;
    [SerializeField] private int securityCylinderWaveToBeginSpawning;

    [Header("References")]
    [SerializeField] private Spawner[] spawners;
    [SerializeField] private GameObject securitySpherePrefab;
    [SerializeField] private GameObject securityPyramidPrefab;
    [SerializeField] private GameObject supSecuritySpherePrefab;
    [SerializeField] private GameObject supSecurityPyramidPrefab;
    [SerializeField] private GameObject securityCylinderPrefab;

    private RoundManager roundManager;
    private GameObject player;
    private List<GameObject> enemies;

    [HideInInspector] public bool hasWon;
    [HideInInspector] public int currentWave;

    // Start is called before the first frame update
    void Start()
    {
        roundManager = FindFirstObjectByType<RoundManager>();
        player = FindFirstObjectByType<Player>().gameObject;
        enemies = new List<GameObject>();
        currentWave = 0;
        hasWon = false;

        foreach(Spawner spawner in spawners)
        {
            spawner.player = player;
            spawner.waveManager = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (enemies.Count == 0 && !hasWon)
        {

            if (currentWave >= wavePoints.Count)
            {
                roundManager.RoundEnd(true);
                hasWon = true;
            }
            else
            {
                StartWave();
            }

            currentWave++;
        }
    }

    void StartWave()
    {
        int points = wavePoints[currentWave];

        List<int> idsToSpawn = new List<int>();

        if (securitySphereWaveToBeginSpawning <= currentWave)
            idsToSpawn.Add(0);

        if (securityPyramidWaveToBeginSpawning <= currentWave)
            idsToSpawn.Add(1);

        if (supSecuritySphereWaveToBeginSpawning <= currentWave)
            idsToSpawn.Add(2);

        if (supSecurityPyramidWaveToBeginSpawning <= currentWave)
            idsToSpawn.Add(3);

        if (securityCylinderWaveToBeginSpawning <= currentWave)
            idsToSpawn.Add(4);

        if (idsToSpawn.Count == 0)
        {
            Debug.LogError("No enemies can spawn on wave 1");
            return;
        }

        while (points > 0)
        {
            int enemyToSpawn = Random.Range(0, idsToSpawn.Count);

            switch (idsToSpawn[enemyToSpawn])
            {
                case 0:
                    if (points >= securitySpherePointVal)
                    {
                        points -= securitySpherePointVal;
                        enemies.Add(spawners[Random.Range(0, spawners.Length)].SpawnEnemy(securitySpherePrefab));
                    }
                    break;

                case 1:
                    if (points >= securityPyramidPointVal)
                    {
                        points -= securityPyramidPointVal;
                        enemies.Add(spawners[Random.Range(0, spawners.Length)].SpawnEnemy(securityPyramidPrefab));
                    }
                    break;

                case 2:
                    if (points >= supSecuritySpherePointVal)
                    {
                        points -= supSecuritySpherePointVal;
                        enemies.Add(spawners[Random.Range(0, spawners.Length)].SpawnEnemy(supSecuritySpherePrefab));
                    }
                    break;

                case 3:
                    if (points >= supSecurityPyramidPointVal)
                    {
                        points -= supSecurityPyramidPointVal;
                        enemies.Add(spawners[Random.Range(0, spawners.Length)].SpawnEnemy(supSecurityPyramidPrefab));
                    }
                    break;

                case 4:
                    if (points >= securityCylinderPointVal)
                    {
                        points -= securityCylinderPointVal;
                        enemies.Add(spawners[Random.Range(0, spawners.Length)].SpawnEnemy(securityCylinderPrefab));
                    }
                    break;

                default:
                    Debug.LogError("Invaild id given, unable to spawn an unassigned enemy");
                    break;
            }
        }
    }

    public void EnemyDied(GameObject enemy)
    {
        enemies.Remove(enemy);
    }
}
