using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    
    [Header("References")]
    public UpgradeObject pHealth;
    public UpgradeObject pSpeed;
    public UpgradeObject pReloadMovementReduction;
    public UpgradeObject pExplosiveDefenseSystem;
    public UpgradeObject pDefensiveReloadSystem;
    public UpgradeObject sDamage;
    public UpgradeObject sPelletAmount;
    public UpgradeObject sClipSize;
    public UpgradeObject sReloadTime;
    public UpgradeObject sMultiShellReload;
    [SerializeField] private TextMeshProUGUI walletGSText;
    [SerializeField] private Player player;
    [SerializeField] private Shotgun shotgun;
    [SerializeField] private ProgramManager programManager;

    [Header("DEBUG")]
    [SerializeField] private bool moneyCheat = false;

    private void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (moneyCheat && Input.GetKeyDown(KeyCode.M))
            player.geometricScrap += 100000;

        walletGSText.text = string.Format(CultureInfo.InvariantCulture, "{0:N0}", player.geometricScrap);
    }

    public void UpdatePlayerAndShotgunStats()
    {
        player.maxHealth = (int)pHealth.upgrade.currentStat;
        player.speedMultiplier = pSpeed.upgrade.currentStat;
        player.reloadSpeedReduction = pReloadMovementReduction.upgrade.currentStat;
        if (pExplosiveDefenseSystem.upgrade.currentStat > 0)
            player.isEDSActive = true;
        if (pDefensiveReloadSystem.upgrade.currentStat > 0)
            player.isDRSActive = true;
        shotgun.gunDamage = (int)sDamage.upgrade.currentStat;
        shotgun.pelletAmount = (int)sPelletAmount.upgrade.currentStat;
        shotgun.clipSize = (int)sClipSize.upgrade.currentStat;
        shotgun.reloadTime = sReloadTime.upgrade.currentStat;
        shotgun.amountOfShellsToReload = (int)sMultiShellReload.upgrade.currentStat;
    }

    public void SetAllUpgrades(PlayerData data)
    {
        pHealth.upgrade.currentStat = data.pHealthCurrentStat;
        pHealth.upgrade.currentUpgradeLevel = data.pHealthCurrentLevel;
        pHealth.ResetUpgrade();

        pSpeed.upgrade.currentStat = data.pSpeedCurrentStat;
        pSpeed.upgrade.currentUpgradeLevel = data.pSpeedCurrentLevel;
        pSpeed.ResetUpgrade();

        pReloadMovementReduction.upgrade.currentStat = data.pReloadMovementReductionCurrentStat;
        pReloadMovementReduction.upgrade.currentUpgradeLevel = data.pReloadMovementReductionCurrentLevel;
        pReloadMovementReduction.ResetUpgrade();

        pExplosiveDefenseSystem.upgrade.currentStat = data.pExplosiveDefenseSystemCurrentStat;
        pExplosiveDefenseSystem.upgrade.currentUpgradeLevel = data.pExplosiveDefenseSystemCurrentLevel;
        pExplosiveDefenseSystem.ResetUpgrade();

        pDefensiveReloadSystem.upgrade.currentStat = data.pDefensiveReloadSystemCurrentStat;
        pDefensiveReloadSystem.upgrade.currentUpgradeLevel = data.pDefensiveReloadSystemCurrentLevel;
        pDefensiveReloadSystem.ResetUpgrade();

        sDamage.upgrade.currentStat = data.sDamageCurrentStat;
        sDamage.upgrade.currentUpgradeLevel = data.sDamageCurrentLevel;
        sDamage.ResetUpgrade();

        sPelletAmount.upgrade.currentStat = data.sPelletAmountCurrentStat;
        sPelletAmount.upgrade.currentUpgradeLevel = data.sPelletAmountCurrentLevel;
        sPelletAmount.ResetUpgrade();

        sClipSize.upgrade.currentStat = data.sClipSizeCurrentStat;
        sClipSize.upgrade.currentUpgradeLevel = data.sClipSizeCurrentLevel;
        sClipSize.ResetUpgrade();

        sReloadTime.upgrade.currentStat = data.sReloadTimeCurrentStat;
        sReloadTime.upgrade.currentUpgradeLevel = data.sReloadTimeCurrentLevel;
        sReloadTime.ResetUpgrade();

        sMultiShellReload.upgrade.currentStat = data.sMultiShellReloadCurrentStat;
        sMultiShellReload.upgrade.currentUpgradeLevel = data.sMultiShellReloadCurrentLevel;
        sMultiShellReload.ResetUpgrade();
    }
}
