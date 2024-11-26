using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    
    [Header("References")]
    [SerializeField] private UpgradeObject pHealth;
    [SerializeField] private UpgradeObject pSpeed;
    [SerializeField] private UpgradeObject pReloadMovementReduction;
    [SerializeField] private UpgradeObject pExplosiveDefenseSystem;
    [SerializeField] private UpgradeObject pDefensiveReloadSystem;
    [SerializeField] private UpgradeObject sDamage;
    [SerializeField] private UpgradeObject sPelletAmount;
    [SerializeField] private UpgradeObject sClipSize;
    [SerializeField] private UpgradeObject sReloadTime;
    [SerializeField] private UpgradeObject sMultiShellReload;
    [SerializeField] private Player player;
    [SerializeField] private Shotgun shotgun;
    [SerializeField] private TextMeshProUGUI walletGSText;

    private void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        walletGSText.text = "GS Wallet: " + player.geometricScrap;
    }

    public void UpdatePlayerAndShotgunStats()
    {
        player.maxHealth = (int)pHealth.upgrade.currentStat;
        player.speedMultiplier = pSpeed.upgrade.currentStat;
        player.reloadSpeedReduction = pReloadMovementReduction.upgrade.currentStat;
        if (pExplosiveDefenseSystem.upgrade.currentStat > 0)
            player.isEDSActive = true;
        if (pReloadMovementReduction.upgrade.currentStat > 0)
            player.isDRSActive = true;
        shotgun.gunDamage = (int)sDamage.upgrade.currentStat;
        shotgun.pelletAmount = (int)sPelletAmount.upgrade.currentStat;
        shotgun.clipSize = (int)sClipSize.upgrade.currentStat;
        shotgun.reloadTime = sReloadTime.upgrade.currentStat;
        shotgun.amountOfShellsToReload = (int)sMultiShellReload.upgrade.currentStat;
    }
}
