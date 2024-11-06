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

    private List<Upgrade> allUpgrades;

    [Header("References")]
    [SerializeField] private Player player;
    [SerializeField] private Shotgun shotgun;
    [SerializeField] private TextMeshProUGUI walletGSText;
    //[SerializeField] private TextMeshProUGUI totalGSText;

    // Start is called before the first frame update
    void Start()
    {
        pHealth = new Upgrade(player.maxHealth, 1, 3);
        pSpeed = new Upgrade(player.speedMultiplier, 0.2f, 3);
        pReloadMovementReduction = new Upgrade(player.reloadSpeedReduction, 0.15f, 3);
        pExplosiveDefenseSystem = new Upgrade(0, 1, 1);
        pDefensiveReloadSystem = new Upgrade(0, 1, 1);
        sDamage = new Upgrade(shotgun.gunDamage, 1, 2); 
        sPelletAmount = new Upgrade(shotgun.pelletAmount, 2, 3);
        sClipSize = new Upgrade(shotgun.clipSize, 1, 3);
        sReloadTime = new Upgrade(shotgun.reloadTime, -0.05f, 3);
        sMultiShellReload = new Upgrade(shotgun.amountOfShellsToReload, 1, 2);
    }

    // Update is called once per frame
    void Update()
    {
        walletGSText.text = "GS Wallet: " + player.geometricScrap;
    }

    void UpdatePlayerAndShotgunStats()
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
