using System.Collections.Generic;
using UnityEngine;

public static class UnimoLevelSystem
{
    private static Dictionary<int, int[]> unitStatLevels = new();

    private const int MaxLevel_NormalStat = 250;
    private const int MaxLevel_CritChance = 100;
    private const int StatCount = 6;

    public enum StatType
    {
        MoveSpd = 0,
        Health,
        Armor,
        AuraRange,
        AuraStr,
        CriticalChance
    }

    static UnimoLevelSystem()
    {
        LoadAllData(); // ���� ���� �� �ڵ� �ε�
    }

    public static int GetLevel(int unitID, StatType stat)
    {
        int[] levels = GetOrCreateLevelArray(unitID);
        return levels[(int)stat];
    }

    public static bool LevelUp(int unitID, StatType stat, int amount = 1) // �� �߰�
    {
        int[] levels = GetOrCreateLevelArray(unitID);

        int currentLevel = levels[(int)stat];
        int maxLevel = (stat == StatType.CriticalChance) ? MaxLevel_CritChance : MaxLevel_NormalStat;

        if (currentLevel >= maxLevel)
            return false;

        // �ѹ��� amount ��ŭ ���
        levels[(int)stat] = Mathf.Min(currentLevel + amount, maxLevel);

        SaveUnitData(unitID, levels);
        return true;
    }

    public static void ResetUnitStats(int unitID)
    {
        if (unitStatLevels.ContainsKey(unitID))
        {
            unitStatLevels[unitID] = new int[StatCount];

            // ����� �����͵� ����
            for (int i = 0; i < StatCount; i++)
            {
                PlayerPrefs.DeleteKey($"Unit_{unitID}_Stat_{i}");
            }

            PlayerPrefs.Save();
            Debug.Log($"[{unitID}] ���� ���� ���µ�");
        }
    }

    public static void ResetAll()
    {
        unitStatLevels.Clear();

        // ��� ����� ������ ����
        foreach (var unitID in GetAllKnownUnitIDs())
        {
            for (int i = 0; i < StatCount; i++)
            {
                PlayerPrefs.DeleteKey($"Unit_{unitID}_Stat_{i}");
            }
        }

        PlayerPrefs.Save();
        Debug.Log("��� ������ ���� ���� �����͸� �ʱ�ȭ�߽��ϴ�.");
    }

    private static IEnumerable<int> GetAllKnownUnitIDs()
    {
        // �̹� �ε�� ���ָ� ���� (�� ���� ���� �� �ʿ��ϸ� ���� ����Ʈ Ȯ��)
        return unitStatLevels.Keys;
    }



    private static int[] GetOrCreateLevelArray(int unitID)
    {
        if (!unitStatLevels.TryGetValue(unitID, out int[] levels))
        {
            levels = LoadUnitData(unitID);
            unitStatLevels[unitID] = levels;
        }

        return levels;
    }

    private static void SaveUnitData(int unitID, int[] levels)
    {
        for (int i = 0; i < StatCount; i++)
        {
            PlayerPrefs.SetInt($"Unit_{unitID}_Stat_{i}", levels[i]);
        }

        PlayerPrefs.Save();
    }

    private static int[] LoadUnitData(int unitID)
    {
        int[] levels = new int[StatCount];
        for (int i = 0; i < StatCount; i++)
        {
            levels[i] = PlayerPrefs.GetInt($"Unit_{unitID}_Stat_{i}", 0);
        }

        return levels;
    }

    private static void LoadAllData()
    {
        // �ʿ� �� ��ü ���� ID ����Ʈ�� �ʱ� �ε� ���� ����
        // ����� ��û�� ���� ���� ���� ���� �ε� (���� ����ȭ)
    }

    public static SCharacterStat ApplyLevelBonus(SCharacterStat baseStat, int unitID)
    {
        int moveSpdLevel = GetLevel(unitID, StatType.MoveSpd);
        int healthLevel = GetLevel(unitID, StatType.Health);
        int armorLevel = GetLevel(unitID, StatType.Armor);
        int auraRangeLevel = GetLevel(unitID, StatType.AuraRange);
        int auraStrLevel = GetLevel(unitID, StatType.AuraStr);
        int critLevel = GetLevel(unitID, StatType.CriticalChance);

        // ��� ���� (���� �� �� (1 + ������ * ����))
        baseStat.MoveSpd *= (1f + 0.002f * moveSpdLevel);
        baseStat.AuraRange *= (1f + 0.002f * auraRangeLevel);
        baseStat.AuraStr *= (1f + 0.002f * auraStrLevel);

        // ����ġ ���� (���� �� + ������ ����)
        baseStat.Health += 5 * healthLevel;
        baseStat.Armor += 0.002f * armorLevel;
        baseStat.CriticalChance += 0.005f * critLevel;

        return baseStat;
    }
}