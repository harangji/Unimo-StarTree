using System.Collections.Generic;
using UnityEngine;

public static class UnimoUpgradeManager
{
    private static Dictionary<int, int> upgradeLevels = new();

    public static int GetLevel(int unitID)
    {
        if (!upgradeLevels.TryGetValue(unitID, out int level))
        {
            level = 0;
            upgradeLevels[unitID] = 0;
        }
        return level;
    }

    public static void LevelUp(int unitID)
    {
        if (!upgradeLevels.ContainsKey(unitID))
            upgradeLevels[unitID] = 0;

        if (upgradeLevels[unitID] < 250)
            upgradeLevels[unitID]++;
    }

    public static void ResetAllUpgrades()
    {
        upgradeLevels.Clear();
    }

    public static float GetStatMultiplier(int unitID)
    {
        int level = GetLevel(unitID);
        return 1f + 0.01f * level;   // 레벨당 +1% 증가 예시
    }
}