using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    private Upgrade pHealth;
    private Upgrade pSpeed;
    private Upgrade pReloadMovementReduction;
    private Upgrade pExplosiveDefenseSystem;
    private Upgrade pDefensiveReloadSystem;
    private Upgrade sDamage;
    private Upgrade sPelletAmount;
    private Upgrade sClipSize;
    private Upgrade sReloadTime;
    private Upgrade sMultiShellReload;

    [HideInInspector] public List<Upgrade> upgrades;

    [Header("References")]
    public Player player;
    [SerializeField] private Shotgun shotgun;
    [SerializeField] private TextMeshProUGUI walletGSText;

    private void Awake()
    {
        upgrades = new List<Upgrade>();

        pHealth = new Upgrade("health", player.maxHealth, 1, 3); upgrades.Add(pHealth);
        pSpeed = new Upgrade("speed", player.speedMultiplier, 0.2f, 3); upgrades.Add(pSpeed);
        pReloadMovementReduction = new Upgrade("reloadMovementReduction", player.reloadSpeedReduction, 0.15f, 3); upgrades.Add(pReloadMovementReduction);
        pExplosiveDefenseSystem = new Upgrade("eds", 0, 1, 1); upgrades.Add(pExplosiveDefenseSystem);
        pDefensiveReloadSystem = new Upgrade("drs", 0, 1, 1); upgrades.Add(pDefensiveReloadSystem);
        sDamage = new Upgrade("damage", shotgun.gunDamage, 1, 2); upgrades.Add(sDamage);
        sPelletAmount = new Upgrade("pelletAmount", shotgun.pelletAmount, 2, 3); upgrades.Add(sPelletAmount);
        sClipSize = new Upgrade("clipSize", shotgun.clipSize, 1, 3); upgrades.Add(sClipSize);
        sReloadTime = new Upgrade("reloadTime", shotgun.reloadTime, -0.05f, 3); upgrades.Add(sReloadTime);
        sMultiShellReload = new Upgrade("multiShellReload", shotgun.amountOfShellsToReload, 1, 2); upgrades.Add(sMultiShellReload);
    }

    // Update is called once per frame
    void Update()
    {
        walletGSText.text = "GS Wallet: " + player.geometricScrap;
    }

    public void UpdatePlayerAndShotgunStats()
    {
        player.maxHealth = (int)pHealth.currentStat;
        player.speedMultiplier = pSpeed.currentStat;
        player.reloadSpeedReduction = pReloadMovementReduction.currentStat;
        if (pExplosiveDefenseSystem.currentStat > 0)
            player.isEDSActive = true;
        if (pDefensiveReloadSystem.currentStat > 0)
            player.isDRSActive = true;
        shotgun.gunDamage = (int)sDamage.currentStat;
        shotgun.pelletAmount = (int)sPelletAmount.currentStat;
        shotgun.clipSize = (int)sClipSize.currentStat;
        shotgun.reloadTime = sReloadTime.currentStat;
        shotgun.amountOfShellsToReload = (int)sMultiShellReload.currentStat;
    }
}
