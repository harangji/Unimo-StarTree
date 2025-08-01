using System.Collections.Generic;
using UnityEngine;

public static class EngineLevelSystem
{
    private const int MaxLevel = 50;

    // --------- 일반 StatType 엔진 ---------
    public enum EEngineStatType
    {
        MoveSpd = 0,
        Health,
        Armor,
        AuraStr,
        AuraRange,
        CriticalChance
    }
    private static readonly int StatCount = System.Enum.GetNames(typeof(EEngineStatType)).Length;
    private static Dictionary<int, int[]> mEngineStatLevels = new Dictionary<int, int[]>();

    // --------- 고유 효과 엔진 ---------
    private static Dictionary<int, int> mEngineUniqueLevels = new Dictionary<int, int>();

    // --------- 일반 StatType 엔진 함수 ---------
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
        int[] levels = GetOrCreateStatLevelArray(engineID);
        return levels[(int)statType];
    }

    private static int[] GetOrCreateStatLevelArray(int engineID)
    {
        if (!mEngineStatLevels.TryGetValue(engineID, out int[] levels))
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
            PlayerPrefs.SetInt($"Engine_{engineID}_Stat_{i}", levels[i]);
        }
        PlayerPrefs.Save();
    }

    private static int[] LoadStatData(int engineID)
    {
        int[] levels = new int[StatCount];
        for (int i = 0; i < StatCount; i++)
        {
            levels[i] = PlayerPrefs.GetInt($"Engine_{engineID}_Stat_{i}", 0);
        }
        return levels;
    }

    // --------- 고유 효과 엔진 함수 ---------
    public static bool LevelUpUnique(int engineID, int amount = 1)
    {
        int cur = GetUniqueLevel(engineID);
        if (cur >= MaxLevel) return false;
        mEngineUniqueLevels[engineID] = Mathf.Min(cur + amount, MaxLevel);
        PlayerPrefs.SetInt($"Engine_{engineID}_UniqueLevel", mEngineUniqueLevels[engineID]);
        PlayerPrefs.Save();
        return true;
    }

    public static int GetUniqueLevel(int engineID)
    {
        if (!mEngineUniqueLevels.TryGetValue(engineID, out int lv))
        {
            lv = PlayerPrefs.GetInt($"Engine_{engineID}_UniqueLevel", 0);
            mEngineUniqueLevels[engineID] = lv;
        }
        return lv;
    }

    public static void ResetUniqueEngine(int engineID)
    {
        mEngineUniqueLevels[engineID] = 0;
        PlayerPrefs.SetInt($"Engine_{engineID}_UniqueLevel", 0);
        PlayerPrefs.Save();
    }

    // --------- 전체/리셋 함수 ---------
    public static void ResetEngineStats(int engineID)
    {
        mEngineStatLevels[engineID] = new int[StatCount];
        for (int i = 0; i < StatCount; i++)
            PlayerPrefs.DeleteKey($"Engine_{engineID}_Stat_{i}");
        PlayerPrefs.Save();
    }

    public static void ResetEngine(int engineID)
    {
        // 고유 레벨 초기화
        mEngineUniqueLevels[engineID] = 1;
        PlayerPrefs.SetInt($"Engine_{engineID}_UniqueLevel", 1);

        // 일반 스탯 초기화
        mEngineStatLevels[engineID] = new int[StatCount];
        for (int i = 0; i < StatCount; i++)
            PlayerPrefs.DeleteKey($"Engine_{engineID}_Stat_{i}");

        PlayerPrefs.Save();
    }
}
