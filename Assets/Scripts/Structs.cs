using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Upgrade
{
    public string name;
    public float currentStat;
    public float goesUpBy;
    public int currentUpgradeLevel;
    public int endingUpgradeLevel;

    public Upgrade(string name, float startingStat, float goesUpBy, int endingUpgradeLevel)
    {
        this.name = name;
        this.currentStat = startingStat;
        this.goesUpBy = goesUpBy;
        this.currentUpgradeLevel = 0;
        this.endingUpgradeLevel = endingUpgradeLevel;
    }

    public void LevelUp()
    {
        if (currentUpgradeLevel < endingUpgradeLevel)
            return;

        currentStat += goesUpBy;
        currentUpgradeLevel++;
    }
}
