
using System.Collections.Generic;
using UnityEngine;

public static class UnimoLevelSystem
{
    private static Dictionary<int, int[]> unitStatLevels = new();
    
    private static Dictionary<int, SStatCost> mNormalStatCostTable = new();
    private static Dictionary<int, SStatCost> mCriticalStatCostTable = new();

    private const int MaxLevel_NormalStat = 250;
    private const int MaxLevel_CritChance = 100;
    private const int StatCount = 6;

    public enum StatType
    {
        MoveSpd = 0,
        Health,
        Armor,
        AuraStr,
        AuraRange,
        CriticalChance
    }

    static UnimoLevelSystem()
    {
        LoadAllData(); // 게임 시작 시 자동 로드
        LoadStatCostTables(); 
    }
    
    private static void LoadStatCostTables()
    {
        var normalCsv = Resources.Load<TextAsset>("Datas/StatCost/StatCost_Normal");
        var critCsv = Resources.Load<TextAsset>("Datas/StatCost/StatCost_Critical");

        mNormalStatCostTable = StatCostDataLoader.LoadCostTable(normalCsv);
        mCriticalStatCostTable = StatCostDataLoader.LoadCostTable(critCsv);

        Debug.Log($"[StatCost] 일반 스탯 {mNormalStatCostTable.Count}개, 크리티컬 스탯 {mCriticalStatCostTable.Count}개 로드됨");
    }
    
    private static bool TryGetStatCost(StatType stat, int nextLevel, out SStatCost cost)
    {
        if (stat == StatType.CriticalChance)
            return mCriticalStatCostTable.TryGetValue(nextLevel, out cost);
        else
            return mNormalStatCostTable.TryGetValue(nextLevel, out cost);
    }

    public static int GetLevel(int unitID, StatType stat)
    {
        int[] levels = GetOrCreateLevelArray(unitID);
        return levels[(int)stat];
    }

    public static bool LevelUp(int unitID, StatType stat, int amount = 1) // ← 추가
    {
        int[] levels = GetOrCreateLevelArray(unitID);
        int currentLevel = levels[(int)stat];
        int maxLevel = (stat == StatType.CriticalChance) ? MaxLevel_CritChance : MaxLevel_NormalStat;

        if (currentLevel >= maxLevel)
            return false;
        
        int nextLevel = currentLevel + 1;

        if (!TryGetStatCost(stat, nextLevel, out SStatCost cost))
        {
            Debug.LogWarning($"[StatCost] 요구량 정보 없음 - {stat} {nextLevel}레벨");
            return false;
        }

        if (!UserHasEnoughCurrency(cost.RequireYF, cost.RequireOF))
        {
            Debug.Log($"[강화 실패] 재화 부족 - YF:{cost.RequireYF}, OF:{cost.RequireOF}");
            return false;
        }

        DeductCurrency(cost.RequireYF, cost.RequireOF);

        // 한번에 amount 만큼 상승
        levels[(int)stat] = Mathf.Min(currentLevel + amount, maxLevel);

        SaveUnitData(unitID, levels);
        return true;
    }
    
    private static bool UserHasEnoughCurrency(int requireYF, int requireOF)
    {
        var user = Base_Manager.Data.UserData;
        return user.Yellow >= requireYF && user.Red >= requireOF;
    }

    private static void DeductCurrency(int requireYF, int requireOF)
    {
        var user = Base_Manager.Data.UserData;
        user.Yellow -= requireYF;
        user.Red -= requireOF;
    }
    
    public static bool TryGetCost(StatType stat, int level, out SStatCost cost)
    {
        if (stat == StatType.CriticalChance)
            return mCriticalStatCostTable.TryGetValue(level, out cost);
        else
            return mNormalStatCostTable.TryGetValue(level, out cost);
    }

    public static void ResetUnitStats(int unitID)
    {
        if (unitStatLevels.ContainsKey(unitID))
        {
            unitStatLevels[unitID] = new int[StatCount];

            // 저장된 데이터도 삭제
            for (int i = 0; i < StatCount; i++)
            {
                PlayerPrefs.DeleteKey($"Unit_{unitID}_Stat_{i}");
            }

            PlayerPrefs.Save();
            Debug.Log($"[{unitID}] 스탯 전부 리셋됨");
        }
    }

    public static void ResetAll()
    {
        unitStatLevels.Clear();

        // 모든 저장된 데이터 삭제
        foreach (var unitID in GetAllKnownUnitIDs())
        {
            for (int i = 0; i < StatCount; i++)
            {
                PlayerPrefs.DeleteKey($"Unit_{unitID}_Stat_{i}");
            }
        }

        PlayerPrefs.Save();
        Debug.Log("모든 유닛의 스탯 레벨 데이터를 초기화했습니다.");
    }

    private static IEnumerable<int> GetAllKnownUnitIDs()
    {
        // 이미 로드된 유닛만 리셋 (새 유닛 등장 시 필요하면 관리 리스트 확장)
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
        // 필요 시 전체 유닛 ID 리스트로 초기 로드 수행 가능
        // 여기는 요청이 있을 때만 개별 유닛 로드 (성능 최적화)
    }

    public static SCharacterStat ApplyLevelBonus(SCharacterStat baseStat, int unitID)
    {
        int moveSpdLevel = GetLevel(unitID, StatType.MoveSpd);
        int healthLevel = GetLevel(unitID, StatType.Health);
        int armorLevel = GetLevel(unitID, StatType.Armor);
        int auraRangeLevel = GetLevel(unitID, StatType.AuraRange);
        int auraStrLevel = GetLevel(unitID, StatType.AuraStr);
        int critLevel = GetLevel(unitID, StatType.CriticalChance);

        // 비례 증가 (원본 값 × (1 + 증가율 * 레벨))
        baseStat.MoveSpd *= (1f + 0.002f * moveSpdLevel);
        baseStat.AuraRange *= (1f + 0.002f * auraRangeLevel);
        baseStat.AuraStr *= (1f + 0.002f * auraStrLevel);

        // 고정치 증가 (원본 값 + 레벨당 누적)
        baseStat.Health += 5 * healthLevel;
        baseStat.Armor += 0.002f * armorLevel;
        baseStat.CriticalChance += 0.005f * critLevel;

        return baseStat;
    }
}