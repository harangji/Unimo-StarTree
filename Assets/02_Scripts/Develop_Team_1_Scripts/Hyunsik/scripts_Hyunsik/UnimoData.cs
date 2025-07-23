using UnityEngine;

public enum EStatGrade { S, A, B, C, D, E, F, None }

public struct SCharacterStat
{
    public float MoveSpd;
    public Vector2Int CharSize;
    public float Health;
    public float HealthRegen;
    public float HealingMult;
    public float Armor;
    public float StunIgnoreChance;
    public float StunResistanceRate;
    public float CriticalChance;
    public float CriticalMult;
    public float YFGainMult;
    public float OFGainMult;
    public float AuraRange;
    public float AuraStr;
    
    /// <summary>
    /// 모든 수치를 반전시킨 새 스탯 반환 (버프 해제용)
    /// </summary>
    public SCharacterStat Negate()
    {
        return new SCharacterStat
        {
            MoveSpd = -MoveSpd,
            Health = -Health,
            AuraRange = -AuraRange,
            AuraStr = -AuraStr,
            CriticalChance = -CriticalChance,
            CriticalMult = -CriticalMult,
            Armor = -Armor,
            HealthRegen = -HealthRegen,
            HealingMult = -HealingMult,
            YFGainMult = -YFGainMult,
            OFGainMult = -OFGainMult,
            StunIgnoreChance = -StunIgnoreChance,
            StunResistanceRate = -StunResistanceRate,
            CharSize = CharSize  // 크기(CharSize)는 음수화하지 않음
        };
    }
    
}

public struct EStatGradeSet
{
    public EStatGrade MoveSpd;
    public EStatGrade CharSize;
    public EStatGrade Health;
    public EStatGrade HealthRegen;
    public EStatGrade HealingMult;
    public EStatGrade Armor;
    public EStatGrade StunIgnoreChance;
    public EStatGrade StunResistanceRate;
    public EStatGrade CriticalChance;
    public EStatGrade CriticalMult;
    public EStatGrade YFGainMult;
    public EStatGrade OFGainMult;
    public EStatGrade AuraRange;
    public EStatGrade AuraStr;
}

public class UnimoData
{
    public int UnimoID;
    public string Name;
    public string Rarity;
    public string ModelID;
    public SCharacterStat Stat;
    public EStatGradeSet Grade;
}
