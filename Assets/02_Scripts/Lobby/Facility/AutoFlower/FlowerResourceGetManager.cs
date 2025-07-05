using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerResourceGetManager : FacilityBehaviour
{
    private readonly double hBoostPerLv = 1.15d;
    private StarHoneyManager honeyManager;
    private double honeyPerFlower = 30d;
    private double honeyBonus = 1d;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        honeyManager = FindAnyObjectByType<StarHoneyManager>();
        FlowerController_Lobby.resourceManager = this;
    }
    public void GetResource()
    {
        honeyManager.AddYellow(honeyPerFlower * honeyBonus);
    }
    private double calculateHoneyBonus(double accum)
    {
        return Math.Pow(hBoostPerLv, accum);
    }

    override protected void checkTierUp(int newLv)
    {
        if (newLv == 0) { return; }
        int oriTier = lvUpTable.CalculateTier(newLv - 1);
        int newTier = lvUpTable.CalculateTier(newLv);
    }
    override protected void checkUpgrade(int newLv)
    {
        var allAccums = lvUpTable.CalculateUpAccumPerTier(newLv);
        double accum = 0f;
        for (int i = 0; i < allAccums.Count; i++)
        {
            accum += (double)allAccums[i][0];
        }
        honeyBonus = calculateHoneyBonus((double)accum);
    }
    override protected string nextUpListText(int newLv)
    {
        List<bool> isUpgrades = lvUpTable.NextUpList(newLv);
        string output = string.Empty;
        if (isUpgrades[0] == true) { output += $"Ground +1\n"; }
        else
        {
            if (isUpgrades[1] == true) { output += $"Unimo harvest +{(hBoostPerLv - 1d)*100d:F0}%\n"; }
            if (isUpgrades[2] == true) { output += $"Light globe appear +10%\n"; }
        }

        return output;
    }
}
