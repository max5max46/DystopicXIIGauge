using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeObject : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private string upgradeName;
    [SerializeField] private float upgradeStartingStat;
    [SerializeField] private float upgradeGoesUpBy;
    [SerializeField] private int upgradeEndingLevel;
    [SerializeField] [TextArea (10,7)] private string upgradeDescription;
    [SerializeField] private List<int> upgradeCosts;

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

    [HideInInspector] public Upgrade upgrade;
    private List<GameObject> upgradePips;

    // Start is called before the first frame update
    void Start()
    {
        upgrade = new Upgrade(upgradeName, upgradeStartingStat, upgradeGoesUpBy, upgradeEndingLevel);

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

    private void Update()
    {
        UpdatePips();

        if (mouseDetect.isMouseOver)
            descriptionText.text = upgradeDescription;

        if (upgrade.endingUpgradeLevel == upgrade.currentUpgradeLevel)
            buyButton.enabled = false;

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
        if (upgrade.endingUpgradeLevel != upgrade.currentUpgradeLevel)
            costText.text = "Cost: " + upgradeCosts[upgrade.currentUpgradeLevel];
        else
            costText.text = "Max";
    }

    private void UpdateStat()
    {
        if (upgrade.endingUpgradeLevel != upgrade.currentUpgradeLevel)
            statText.text = upgrade.currentStat + "  >  " + (upgrade.currentStat + upgrade.goesUpBy);
        else
            statText.text = upgrade.currentStat.ToString();
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
