using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeObject : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private string upgradeName;
    [SerializeField] private List<int> upgradeCosts;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private UpgradeManager upgradeManager;

    [Header("Prefabs")]
    [SerializeField] private GameObject upgradePipPrefab;

    private Upgrade upgrade;
    private List<GameObject> upgradePips;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Upgrade upgrade in upgradeManager.upgrades)
            if (upgradeName == upgrade.name)
                this.upgrade = upgrade;

        upgradePips = new List<GameObject>();

        int disBetweenPips;

        if (upgrade.endingUpgradeLevel % 2 == 0)
            disBetweenPips = ((upgrade.endingUpgradeLevel / 2) * 22) + 11;
        else
            disBetweenPips = (upgrade.endingUpgradeLevel / 2) * 22;

        for (int i = 0; i < upgrade.endingUpgradeLevel; i++)
        {
            GameObject upgradePip = Instantiate(upgradePipPrefab, this.gameObject.transform);
            upgradePip.GetComponent<RectTransform>().localPosition = new Vector2(disBetweenPips, -45);
            upgradePip.GetComponent<Pip>().TurnPipOff();
            upgradePips.Add(upgradePip);
            disBetweenPips -= 22;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuyUpgrade()
    {
        if (upgradeManager.player.geometricScrap < upgradeCosts[upgrade.currentUpgradeLevel])
            return;

        upgradeManager.player.geometricScrap -= upgradeCosts[upgrade.currentUpgradeLevel];
        upgrade.LevelUp();
    }

    /*private void UpdatePips()
    {
        for (int i = 0; i < (shotgun.clipSize - shotgun.shellsInClip); i++)
        {
            shells[i + shotgun.shellsInClip].GetComponent<Pip>().TurnPipOff();
        }
    }*/


}
