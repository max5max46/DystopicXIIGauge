using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Properties")]
    public List<int> wavePoints;

    [Header("Enemy: Point Values")]
    [SerializeField] private int securitySpherePointVal;
    [SerializeField] private int securityPyramidPointVal;
    [SerializeField] private int supSecuritySpherePointVal;
    [SerializeField] private int supSecurityPyramidPointVal;
    [SerializeField] private int securityCylinderPointVal;
    [SerializeField] private int headOfSecurityPointVal;

    [Header("Enemy: Waves to Spawn")]
    [SerializeField] private List<bool> securitySphereWavesToSpawn;
    [SerializeField] private List<bool> securityPyramidWavesToSpawn;
    [SerializeField] private List<bool> supSecuritySphereWavesToSpawn;
    [SerializeField] private List<bool> supSecurityPyramidWavesToSpawn;
    [SerializeField] private List<bool> securityCylinderWavesToSpawn;
    [SerializeField] private List<bool> headOfSecurityWavesToSpawn;

    [Header("Enemy: Probability of Spawning on Waves")]
    [SerializeField] private List<float> securitySphereWaveSpawnProbability;
    [SerializeField] private List<float> securityPyramidWaveSpawnProbability;
    [SerializeField] private List<float> supSecuritySphereWaveSpawnProbability;
    [SerializeField] private List<float> supSecurityPyramidWaveSpawnProbability;
    [SerializeField] private List<float> securityCylinderWaveSpawnProbability;
    [SerializeField] private List<float> headOfSecurityWaveSpawnProbability;

    [Header("References")]
    [SerializeField] private Spawner[] spawners;
    [SerializeField] private GameObject securitySpherePrefab;
    [SerializeField] private GameObject securityPyramidPrefab;
    [SerializeField] private GameObject supSecuritySpherePrefab;
    [SerializeField] private GameObject supSecurityPyramidPrefab;
    [SerializeField] private GameObject securityCylinderPrefab;
    [SerializeField] private GameObject headOfSecurityPrefab;

    private RoundManager roundManager;
    private GameObject player;
    private SoundHandler soundHandler;

    [HideInInspector] public List<GameObject> enemies;
    [HideInInspector] public int totalEnemiesInCurrentWave;
    [HideInInspector] public bool hasWon;
    [HideInInspector] public int currentWave;
    [HideInInspector] public int totalWaveGS;

    [Header("Debug")]
    [SerializeField] private bool disableWaves = false;

    // Start is called before the first frame update
    void Start()
    {
        roundManager = FindFirstObjectByType<RoundManager>();
        player = FindFirstObjectByType<Player>().gameObject;
        soundHandler = FindFirstObjectByType<SoundHandler>();
        enemies = new List<GameObject>();
        currentWave = 0;
        totalWaveGS = -roundManager.gsReceivedPerHealthPoint * player.GetComponent<Player>().health;
        hasWon = false;
        player.GetComponent<Player>().wavesSurvived--;

        foreach (Spawner spawner in spawners)
        {
            spawner.player = player;
            spawner.soundHandler = soundHandler;
            spawner.waveManager = this;
        }

        // Testing Lists
        int index = wavePoints.Count - 1;

        bool boolTest = securitySphereWavesToSpawn[index];
        boolTest = securityPyramidWavesToSpawn[index];
        boolTest = securityCylinderWavesToSpawn[index];
        boolTest = supSecuritySphereWavesToSpawn[index];
        boolTest = supSecurityPyramidWavesToSpawn[index];
        boolTest = headOfSecurityWavesToSpawn[index];

        float floatTest = securityCylinderWaveSpawnProbability[index];
        floatTest = securityPyramidWaveSpawnProbability[index];
        floatTest = securityCylinderWaveSpawnProbability[index];
        floatTest = supSecuritySphereWaveSpawnProbability[index];
        floatTest = supSecurityPyramidWaveSpawnProbability[index];
        floatTest = headOfSecurityWaveSpawnProbability[index];


    }

    // Update is called once per frame
    void Update()
    {
        if (enemies.Count == 0 && !hasWon && !disableWaves)
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

            totalWaveGS += roundManager.gsReceivedPerHealthPoint * player.GetComponent<Player>().health;
            currentWave++;
            player.GetComponent<Player>().wavesSurvived++;
        }
    }

    void StartWave()
    {
        totalEnemiesInCurrentWave = 0;

        int points = wavePoints[currentWave];

        List<int> idsToSpawn = new List<int>();

        if (securitySphereWavesToSpawn[currentWave])
            idsToSpawn.Add(0);

        if (securityPyramidWavesToSpawn[currentWave])
            idsToSpawn.Add(1);

        if (supSecuritySphereWavesToSpawn[currentWave])
            idsToSpawn.Add(2);

        if (supSecurityPyramidWavesToSpawn[currentWave])
            idsToSpawn.Add(3);

        if (securityCylinderWavesToSpawn[currentWave])
            idsToSpawn.Add(4);

        // Boss Spawn
        if (headOfSecurityWavesToSpawn[currentWave])
        {
            points -= headOfSecurityPointVal;
            totalEnemiesInCurrentWave++;
            enemies.Add(spawners[Random.Range(0, spawners.Length)].SpawnEnemy(headOfSecurityPrefab));
        }

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
                        if (Random.Range(0f, 1f) < securitySphereWaveSpawnProbability[currentWave])
                        {
                            points -= securitySpherePointVal;
                            totalEnemiesInCurrentWave++;
                            enemies.Add(spawners[Random.Range(0, spawners.Length)].SpawnEnemy(securitySpherePrefab));
                        }
                    }
                    break;

                case 1:
                    if (points >= securityPyramidPointVal)
                    {
                        if (Random.Range(0f, 1f) < securityPyramidWaveSpawnProbability[currentWave])
                        {
                            points -= securityPyramidPointVal;
                            totalEnemiesInCurrentWave++;
                            enemies.Add(spawners[Random.Range(0, spawners.Length)].SpawnEnemy(securityPyramidPrefab));
                        }
                    }
                    break;

                case 2:
                    if (points >= supSecuritySpherePointVal)
                    {
                        if (Random.Range(0f, 1f) < supSecuritySphereWaveSpawnProbability[currentWave])
                        {
                            points -= supSecuritySpherePointVal;
                            totalEnemiesInCurrentWave++;
                            enemies.Add(spawners[Random.Range(0, spawners.Length)].SpawnEnemy(supSecuritySpherePrefab));
                        }
                    }
                    break;

                case 3:
                    if (points >= supSecurityPyramidPointVal)
                    {
                        if (Random.Range(0f, 1f) < supSecurityPyramidWaveSpawnProbability[currentWave])
                        {
                            points -= supSecurityPyramidPointVal;
                            totalEnemiesInCurrentWave++;
                            enemies.Add(spawners[Random.Range(0, spawners.Length)].SpawnEnemy(supSecurityPyramidPrefab));
                        }
                    }
                    break;

                case 4:
                    if (points >= securityCylinderPointVal)
                    {
                        if (Random.Range(0f, 1f) < securityCylinderWaveSpawnProbability[currentWave])
                        {
                            points -= securityCylinderPointVal;
                            totalEnemiesInCurrentWave++;
                            enemies.Add(spawners[Random.Range(0, spawners.Length)].SpawnEnemy(securityCylinderPrefab));
                        }
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
        player.GetComponent<Player>().enemiesKilled++;
    }
}
