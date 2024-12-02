using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeObject : MonoBehaviour
{
    enum NumberFormat
    {
        Normal,
        MinusPercentage,
        PlusPercentage,
        Time,
        Single
    }

    [Header("Properties")]
    [SerializeField] private string upgradeName;
    [SerializeField] private float upgradeStartingStat;
    [SerializeField] private float upgradeGoesUpBy;
    [SerializeField] private int upgradeEndingLevel;
    [SerializeField] [TextArea (10,7)] private string upgradeDescription;
    [SerializeField] private List<int> upgradeCosts;
    [SerializeField] private NumberFormat numberFormat;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI statText;
    [SerializeField] private Button buyButton;
    [SerializeField] private MouseDetect mouseDetect;
    [SerializeField] private GameObject upgradePipsParent;
    [SerializeField] private Player player;
    [SerializeField] private TextMeshProUGUI descriptionText;

    [Header("Prefabs")]
    [SerializeField] private GameObject upgradePipPrefab;

    [HideInInspector] public Upgrade upgrade = new Upgrade();
    private List<GameObject> upgradePips;

    // Start is called before the first frame update
    void Start()
    {
        ResetUpgrade();
    }

    private void Update()
    {
        UpdatePips();

        if (mouseDetect.isMouseOver)
            descriptionText.text = upgradeDescription;

        if (upgrade.endingUpgradeLevel == upgrade.currentUpgradeLevel)
            buyButton.enabled = false;

        if (upgradeCosts.Count > upgrade.currentUpgradeLevel)
            if (player.geometricScrap < upgradeCosts[upgrade.currentUpgradeLevel])
                buyButton.interactable = false;
            else
                buyButton.interactable = true;

    }

    public void ResetUpgrade()
    {
        if (upgrade.currentUpgradeLevel == 0)
            upgrade = new Upgrade(upgradeName, upgradeStartingStat, upgradeGoesUpBy, upgradeEndingLevel);
        else
        {
            upgrade.name = upgradeName;
            upgrade.endingUpgradeLevel = upgradeEndingLevel;
            upgrade.goesUpBy = upgradeGoesUpBy;
        }

        if (upgradePips != null)
            foreach (GameObject upgradePip in upgradePips)
                Destroy(upgradePip);

        upgradePips = new List<GameObject>();

        int disBetweenPips;

        if (upgrade.endingUpgradeLevel % 2 == 0)
            disBetweenPips = ((upgrade.endingUpgradeLevel / 2) * 28) - 14;
        else
            disBetweenPips = (upgrade.endingUpgradeLevel / 2) * 28;

        for (int i = 0; i < upgrade.endingUpgradeLevel; i++)
        {
            GameObject upgradePip = Instantiate(upgradePipPrefab, upgradePipsParent.transform);
            upgradePip.GetComponent<RectTransform>().localPosition = new Vector2(disBetweenPips, -77);
            upgradePips.Add(upgradePip);
            disBetweenPips -= 28;
        }

        UpdateCost();
        UpdateStat();
    }

    public void BuyUpgrade()
    {
        if (player.geometricScrap < upgradeCosts[upgrade.currentUpgradeLevel])
            return;

        player.geometricScrap -= upgradeCosts[upgrade.currentUpgradeLevel];
        upgrade.LevelUp();
        
        UpdateCost();
        UpdateStat();
    }

    private void UpdateCost()
    {
        if (upgrade.endingUpgradeLevel > upgrade.currentUpgradeLevel)
            costText.text = "Cost: " + string.Format(CultureInfo.InvariantCulture, "{0:N0}", upgradeCosts[upgrade.currentUpgradeLevel]);
        else
            costText.text = "Max";
    }

    private void UpdateStat()
    {
        switch (numberFormat)
        {
            case NumberFormat.Normal:
                if (upgrade.endingUpgradeLevel != upgrade.currentUpgradeLevel)
                    statText.text = upgrade.currentStat + "  >  " + (upgrade.currentStat + upgrade.goesUpBy);
                else
                    statText.text = upgrade.currentStat.ToString();
                break;

            case NumberFormat.MinusPercentage:
                if (upgrade.endingUpgradeLevel != upgrade.currentUpgradeLevel)
                    statText.text = "-" + ((1 - upgrade.currentStat) * 100) + "%" + "  >  " + "-" + ((1 - upgrade.goesUpBy - upgrade.currentStat) * 100) + "%";
                else
                    statText.text = "-" + ((1 - upgrade.currentStat) * 100) + "%";
                break;

            case NumberFormat.PlusPercentage:
                if (upgrade.endingUpgradeLevel != upgrade.currentUpgradeLevel)
                    statText.text = "+" + ((upgrade.currentStat - upgradeStartingStat) * 100) + "%" + "  >  " + "+" + ((upgrade.currentStat + upgrade.goesUpBy - upgradeStartingStat) * 100) + "%";
                else
                    statText.text = "+" + ((upgrade.currentStat - upgradeStartingStat) * 100) + "%";
                break;

            case NumberFormat.Time:
                if (upgrade.endingUpgradeLevel != upgrade.currentUpgradeLevel)
                    statText.text = upgrade.currentStat + "s" + "  >  " + (upgrade.currentStat + upgrade.goesUpBy) + "s";
                else
                    statText.text = upgrade.currentStat + "s";
                break;

            case NumberFormat.Single:
                if (upgrade.endingUpgradeLevel != upgrade.currentUpgradeLevel)
                    statText.text = "Inactive";
                else
                    statText.text = "Activated";
                break;
        }
    }

    private void UpdatePips()
    {
        foreach (GameObject upgradePip in upgradePips)
            upgradePip.GetComponent<Pip>().TurnPipOff();

        for (int i = 0; i < upgrade.currentUpgradeLevel; i++)
        {
            upgradePips[upgrade.endingUpgradeLevel - 1 - i].GetComponent<Pip>().TurnPipOn();
        }
    }


}
