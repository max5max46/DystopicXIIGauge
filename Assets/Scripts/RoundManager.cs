using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour
{
    [Header("Properties")]
    public int gsReceivedPerWave = 500;

    [Header("References")]
    [SerializeField] private Player player;
    [SerializeField] private Shotgun shotgun;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private ResultsUI resultsUI;
    [SerializeField] private GameplayUI gameplayUI;
    [SerializeField] private UpgradeManager upgradeManager;

    [HideInInspector] public WaveManager waveManager;

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Menu")
        {
            waveManager = null;
            player.canControl = false;
            return;
        }

        if (scene.name == "Gameplay")
        {
            waveManager = FindAnyObjectByType<WaveManager>();
            waveManager.hasWon = false;
            gameplayUI.ResetUI();
            upgradeManager.UpdatePlayerAndShotgunStats();
            player.canControl = true;
            player.health = player.maxHealth;
            player.geometricScrapInRun = 0;
            shotgun.shellsInClip = shotgun.clipSize;
            return;
        }
    }

    public void RoundEnd(bool hasWon)
    {
        player.canControl = false;
        resultsUI.CalculateResults(waveManager.currentWave, gsReceivedPerWave);
        uiManager.SwitchUIScreen("results");
        player.geometricScrap += player.geometricScrapInRun + (waveManager.currentWave * gsReceivedPerWave);
    }
}
