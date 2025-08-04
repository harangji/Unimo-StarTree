using System.Collections.Generic;
using UnityEngine;

public static class EngineLevelSystem
{
    private const int MaxLevel = 50;

    public enum EEngineStatType
    {
        MoveSpd = 0,
        Health,
        Armor,
        AuraStr,
        AuraRange,
        CriticalChance,
        YFGainMult
    }

    private static readonly int StatCount = System.Enum.GetNames(typeof(EEngineStatType)).Length;
    private static Dictionary<int, int[]> mEngineStatLevels = new();
    private static Dictionary<int, int> mEngineUniqueLevels = new();

    // ----------------- �Ϲ� ���� ���� -----------------
    public static bool LevelUpStat(int engineID, EEngineStatType statType, int amount = 1)
    {
        int[] levels = GetOrCreateStatLevelArray(engineID);
        int idx = (int)statType;
        int currentLevel = levels[idx];
        if (currentLevel >= MaxLevel) return false;

        levels[idx] = Mathf.Min(currentLevel + amount, MaxLevel);
        SaveStatData(engineID, levels);
        return true;
    }

    public static int GetStatLevel(int engineID, EEngineStatType statType)
    {
        return GetOrCreateStatLevelArray(engineID)[(int)statType];
    }

    private static int[] GetOrCreateStatLevelArray(int engineID)
    {
        if (!mEngineStatLevels.TryGetValue(engineID, out var levels))
        {
            levels = LoadStatData(engineID);
            mEngineStatLevels[engineID] = levels;
        }
        return levels;
    }

    private static void SaveStatData(int engineID, int[] levels)
    {
        for (int i = 0; i < StatCount; i++)
        {
            PlayerPrefs.SetInt(FormatStatKey(engineID, i), levels[i]);
        }
        PlayerPrefs.Save();
    }

    private static int[] LoadStatData(int engineID)
    {
        var levels = new int[StatCount];
        for (int i = 0; i < StatCount; i++)
        {
            levels[i] = PlayerPrefs.GetInt(FormatStatKey(engineID, i), 0);
        }
        return levels;
    }

    // ----------------- ���� ȿ�� ���� -----------------
    public static bool LevelUpUnique(int engineID, int amount = 1)
    {
        int cur = GetUniqueLevel(engineID);
        if (cur >= MaxLevel) return false;
        
        var yellowCost = RewardCalculator.EngineYellowCost(cur);
        var redCost = RewardCalculator.EngineOrangeCost(cur);

        if (Base_Manager.Data.UserData.Yellow < yellowCost || Base_Manager.Data.UserData.Red < redCost) 
            return false;

        Base_Manager.Data.UserData.Yellow -= yellowCost;
        Base_Manager.Data.UserData.Red -= redCost;

        int newLevel = Mathf.Min(cur + amount, MaxLevel);
        SetUniqueLevel(engineID, newLevel);
        return true;
    }

    public static int GetUniqueLevel(int engineID)
    {
        if (!mEngineUniqueLevels.TryGetValue(engineID, out int lv))
        {
            lv = PlayerPrefs.GetInt(FormatUniqueKey(engineID), 0);
            mEngineUniqueLevels[engineID] = lv;

            Debug.Log($"[EngineLevelSystem] PlayerPrefs �ε��: {FormatUniqueKey(engineID)} = {lv}");
        }
        return lv;
    }

    public static void ForceReloadUniqueLevel(int engineID)
    {
        int lv = PlayerPrefs.GetInt(FormatUniqueKey(engineID), 0);
        mEngineUniqueLevels[engineID] = lv;
        Debug.Log($"[EngineLevelSystem] ĳ�� ���� ����: {FormatUniqueKey(engineID)} = {lv}");
    }

    public static void SetUniqueLevel(int engineID, int level)
    {
        mEngineUniqueLevels[engineID] = level;
        PlayerPrefs.SetInt(FormatUniqueKey(engineID), level);
        PlayerPrefs.Save();

        Debug.Log($"[EngineLevelSystem] ���� ���� ���� �Ϸ�: {FormatUniqueKey(engineID)} = {level}");
    }

    // ----------------- �ʱ�ȭ ��� -----------------
    public static void ResetEngine(int engineID)
    {
        //  ���� ���� ĳ�� ����
        mEngineUniqueLevels.Remove(engineID); 
        PlayerPrefs.DeleteKey($"Engine_{engineID}_UniqueLevel");

        //  �Ϲ� ���� ���� �ʱ�ȭ
        mEngineStatLevels[engineID] = new int[StatCount];
        for (int i = 0; i < StatCount; i++)
            PlayerPrefs.DeleteKey($"Engine_{engineID}_Stat_{i}");

        PlayerPrefs.Save();
    }

    // ----------------- Key ���� -----------------
    private static string FormatStatKey(int engineID, int statIndex)
    {
        return $"Engine_{engineID}_Stat_{statIndex}";
    }

    private static string FormatUniqueKey(int engineID)
    {
        return $"Engine_{engineID}_UniqueLevel";
    }
}
