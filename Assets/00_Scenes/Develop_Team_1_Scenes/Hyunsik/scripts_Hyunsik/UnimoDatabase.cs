using System;
using System.Collections.Generic;
using UnityEngine;

public static class UnimoDatabase
{
    private static readonly Dictionary<int, Func<UnimoData>> mDataTable = new()
    {
        { 10101, CreateUnimo_Ribi },
        { 10201, CreateUnimo_Toaster },
        //{ 10401, CreateUnimo_Ku }
        // ... 다른 캐릭터도 추가
    };
    
    public static UnimoData GetUnimoData(int id)
    {
        if (mDataTable.TryGetValue(id, out var creator))
            return creator.Invoke();

        Debug.LogError($"Unimo ID {id} not found.");
        return null;
    }
    
    // 리비
    private static UnimoData CreateUnimo_Ribi() => new UnimoData
    {
        UnimoID = 10101,
        Name = "리비",
        Rarity = "Normal",
        ModelID = "CH001",
        Stat = new SCharacterStat
        {
            MoveSpd = 8.2f,
            CharSize = new Vector2Int(120, 120),
            Health = 250, // 적용
            HealthRegen = 1, // 적용
            HealingMult = 1f, // 적용
            Armor = 0.05f, // 적용
            StunIgnoreChance = 0f,
            StunResistanceRate = 0f,
            CriticalChance = 0f,
            CriticalMult = 0f, 
            YFGainMult = 1.3f,
            OFGainMult = 1.5f,
            AuraRange = 6.92f,
            AuraStr = 1f
        },
        Grade = new EStatGradeSet { MoveSpd = EStatGrade.C /* 생략 */ }
    };

    // 토스터
    private static UnimoData CreateUnimo_Toaster() => new UnimoData
    {
        UnimoID = 10201,
        Name = "토스터",
        Rarity = "Rare",
        ModelID = "CH013",
        Stat = new SCharacterStat
        {
            MoveSpd = 9f,
            CharSize = new Vector2Int(120, 120),
            Health = 350,
            HealthRegen = 0,
            HealingMult = 1f,
            Armor = 0.1f,
            StunIgnoreChance = 0.07f,
            StunResistanceRate = 0.3f,
            CriticalChance = 0.1f,
            CriticalMult = 0.1f,
            YFGainMult = 1.5f,
            OFGainMult = 2f,
            AuraRange = 8.73f,
            AuraStr = 1.2f
        },
        Grade = new EStatGradeSet { MoveSpd = EStatGrade.B /* 생략 */ }
    };
}

