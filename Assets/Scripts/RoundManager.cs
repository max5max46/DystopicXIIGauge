using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour
{
    [Header("Properties")]
    public int gsReceivedPerHealthPoint = 500;

    [Header("References")]
    [SerializeField] private Player player;
    [SerializeField] private Shotgun shotgun;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private ResultsUI resultsUI;
    [SerializeField] private GameplayUI gameplayUI;
    [SerializeField] private UpgradeManager upgradeManager;
    [SerializeField] private ProgramManager programManager;

    [HideInInspector] public WaveManager waveManager;

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            waveManager = null;
            player.canControl = false;
            return;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Gameplay")
        {
            waveManager = FindAnyObjectByType<WaveManager>();
            waveManager.hasWon = false;
            upgradeManager.UpdatePlayerAndShotgunStats();
            gameplayUI.ResetUI();
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
        resultsUI.CalculateResults(waveManager.totalWaveGS);
        player.geometricScrap += player.geometricScrapInRun + waveManager.totalWaveGS;
        programManager.Save();

        if (hasWon)
        {
            programManager.SwitchSceneToMenu();
            uiManager.SwitchUIScreen("win");
        }
        else
        {
            uiManager.SwitchUIScreen("results");
        }
    }
}
