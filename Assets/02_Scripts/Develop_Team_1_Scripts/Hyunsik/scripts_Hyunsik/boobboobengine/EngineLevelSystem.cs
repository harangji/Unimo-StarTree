using System.Collections.Generic;
using UnityEngine;


public static class EngineLevelSystem
{
    const int maxLevel = 50;
    
    public enum EEngineStatType
    {
        MoveSpd = 0,
        Health,
        Armor,
        AuraStr,
        AuraRange,
        CriticalChance,
        
        
        
        
        
    }

    // ���� ������ ������ ������Ʈ�� �°� ����
    private static Dictionary<int, int[]> mEngineStatLevels = new Dictionary<int, int[]>();

    public static bool LevelUp(int engineID, EEngineStatType statType, int amount)
    {
        if (!mEngineStatLevels.ContainsKey(engineID))
            mEngineStatLevels[engineID] = new int[System.Enum.GetNames(typeof(EEngineStatType)).Length];

        int idx = (int)statType;
        if (mEngineStatLevels[engineID][idx] < maxLevel)
        {
            mEngineStatLevels[engineID][idx] += amount;
            if (mEngineStatLevels[engineID][idx] > maxLevel)
                mEngineStatLevels[engineID][idx] = maxLevel;
            return true;
        }
        return false;
    }

    public static int GetLevel(int engineID, EEngineStatType statType)
    {
        if (!mEngineStatLevels.ContainsKey(engineID))
            return 0;
        return mEngineStatLevels[engineID][(int)statType];
    }

    public static void ResetEngineStats(int engineID)
    {
        mEngineStatLevels[engineID] = new int[System.Enum.GetNames(typeof(EEngineStatType)).Length];
    }
}