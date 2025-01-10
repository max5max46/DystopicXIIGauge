using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class ProgramManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Player player;
    [SerializeField] private UpgradeManager upgradeManager;
    [SerializeField] private LoadingScreenManager loadingScreenManager;
    [SerializeField] private SoundHandler soundHandler;
    [SerializeField] private MusicHandler musicHandler;

    private void Start()
    {
        Load();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void SwitchSceneToGameplay()
    {
        loadingScreenManager.StartSlideIn(1);
    }

    public void SwitchSceneToMenu()
    {
        loadingScreenManager.StartSlideIn(0);
    }

    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(LoadSceneAsynchronously(sceneIndex));
    }

    IEnumerator LoadSceneAsynchronously (int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.completed += OperationCompleted;
        while (!operation.isDone)
        {

            loadingScreenManager.loadingBar.fillAmount = operation.progress;
            yield return null;
        }
    }
    private void OperationCompleted(AsyncOperation operation)
    {
        loadingScreenManager.StartSlideOut();
    }

    public void ExitGame()
    {
        Application.Quit();
    }


    public bool DoesSaveExist()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
            return true;
        else
            return false;
    }

    public void DeleteSave()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            Debug.Log("Deleted Save");
            File.Delete(Application.persistentDataPath + "/playerInfo.dat");

            PlayerData data = new PlayerData();

            player.geometricScrap = 0;
            upgradeManager.SetAllUpgrades(data);
        }
        else
            Debug.Log("No Save to Delete");

    }

    public void Save()
    {
        Debug.Log("Saved");

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");

        PlayerData data = new PlayerData();

        data.GS = player.geometricScrap;
        data.volumeSFX = soundHandler.volumeSlider.value;
        data.volumeMusic = musicHandler.volumeSlider.value;
        data.enemiesKilled = player.enemiesKilled;
        data.wavesSurvived = player.wavesSurvived;

        data.pHealthCurrentStat = upgradeManager.pHealth.upgrade.currentStat;
        data.pHealthCurrentLevel = upgradeManager.pHealth.upgrade.currentUpgradeLevel;

        data.pSpeedCurrentStat = upgradeManager.pSpeed.upgrade.currentStat;
        data.pSpeedCurrentLevel = upgradeManager.pSpeed.upgrade.currentUpgradeLevel;

        data.pReloadMovementReductionCurrentStat = upgradeManager.pReloadMovementReduction.upgrade.currentStat;
        data.pReloadMovementReductionCurrentLevel = upgradeManager.pReloadMovementReduction.upgrade.currentUpgradeLevel;

        data.pExplosiveDefenseSystemCurrentStat = upgradeManager.pExplosiveDefenseSystem.upgrade.currentStat;
        data.pExplosiveDefenseSystemCurrentLevel = upgradeManager.pExplosiveDefenseSystem.upgrade.currentUpgradeLevel;

        data.pDefensiveReloadSystemCurrentStat = upgradeManager.pDefensiveReloadSystem.upgrade.currentStat;
        data.pDefensiveReloadSystemCurrentLevel = upgradeManager.pDefensiveReloadSystem.upgrade.currentUpgradeLevel;

        data.sDamageCurrentStat = upgradeManager.sDamage.upgrade.currentStat;
        data.sDamageCurrentLevel = upgradeManager.sDamage.upgrade.currentUpgradeLevel;

        data.sPelletAmountCurrentStat = upgradeManager.sPelletAmount.upgrade.currentStat;
        data.sPelletAmountCurrentLevel = upgradeManager.sPelletAmount.upgrade.currentUpgradeLevel;

        data.sClipSizeCurrentStat = upgradeManager.sClipSize.upgrade.currentStat;
        data.sClipSizeCurrentLevel = upgradeManager.sClipSize.upgrade.currentUpgradeLevel;

        data.sReloadTimeCurrentStat = upgradeManager.sReloadTime.upgrade.currentStat;
        data.sReloadTimeCurrentLevel = upgradeManager.sReloadTime.upgrade.currentUpgradeLevel;

        data.sMultiShellReloadCurrentStat = upgradeManager.sMultiShellReload.upgrade.currentStat;
        data.sMultiShellReloadCurrentLevel = upgradeManager.sMultiShellReload.upgrade.currentUpgradeLevel;

        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            Debug.Log("Loaded");

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            player.geometricScrap = data.GS;
            soundHandler.volumeSlider.value = data.volumeSFX;
            musicHandler.volumeSlider.value = data.volumeMusic;
            player.enemiesKilled = data.enemiesKilled;
            player.wavesSurvived = data.wavesSurvived;
            upgradeManager.SetAllUpgrades(data);

        }
        else
            Debug.Log("No Save to Load");

    }
}

[Serializable]
public class PlayerData
{
    public int GS;
    public float volumeSFX;
    public float volumeMusic;
    public int wavesSurvived;
    public int enemiesKilled;

    public float pHealthCurrentStat;
    public int pHealthCurrentLevel;

    public float pSpeedCurrentStat;
    public int pSpeedCurrentLevel;

    public float pReloadMovementReductionCurrentStat;
    public int pReloadMovementReductionCurrentLevel;

    public float pExplosiveDefenseSystemCurrentStat;
    public int pExplosiveDefenseSystemCurrentLevel;

    public float pDefensiveReloadSystemCurrentStat;
    public int pDefensiveReloadSystemCurrentLevel;

    public float sDamageCurrentStat;
    public int sDamageCurrentLevel;

    public float sPelletAmountCurrentStat;
    public int sPelletAmountCurrentLevel;

    public float sClipSizeCurrentStat;
    public int sClipSizeCurrentLevel;

    public float sReloadTimeCurrentStat;
    public int sReloadTimeCurrentLevel;

    public float sMultiShellReloadCurrentStat;
    public int sMultiShellReloadCurrentLevel;

}
