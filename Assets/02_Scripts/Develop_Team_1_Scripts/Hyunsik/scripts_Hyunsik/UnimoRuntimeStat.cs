using UnityEngine;

public class UnimoRuntimeStat
{
    public SCharacterStat BaseStat { get; private set; }
    public SCharacterStat BonusStat { get; set; }
    public SCharacterStat FinalStat { get; private set; }

    public UnimoRuntimeStat(SCharacterStat baseStat)
    {
        BaseStat = baseStat;
        BonusStat = new SCharacterStat();
        RecalculateFinalStat();
    }

    public void AddBonus(SCharacterStat bonus)
    {
        BonusStat = AddStat(BonusStat, bonus);
        RecalculateFinalStat();
    }
 
    public void RemoveBonus(SCharacterStat bonus)
    {
        BonusStat = SubtractStat(BonusStat, bonus);
        RecalculateFinalStat();
    }
    
    public void AddAuraRangeBonus(float bonusValue)
    {
        var bonus = BonusStat;
        bonus.AuraRange += bonusValue;
        BonusStat = bonus;
        RecalculateFinalStat();
    }

    public void SetFinalStat(SCharacterStat stat)
    {
        FinalStat = stat;
    }
    
    public void RemoveAuraRangeBonus(float bonusValue)
    {
        var bonus = BonusStat;
        bonus.AuraRange -= bonusValue;
        BonusStat = bonus;
        RecalculateFinalStat();
    }
    
    
    public void RecalculateFinalStat()
    {
        FinalStat = AddStat(BaseStat, BonusStat);
    }
    
    private SCharacterStat AddStat(SCharacterStat a, SCharacterStat b)
    {
        return new SCharacterStat
        {
            MoveSpd = a.MoveSpd + b.MoveSpd,
            CharSize = a.CharSize,
            Health = a.Health + b.Health,
            HealthRegen = a.HealthRegen + b.HealthRegen,
            HealingMult = a.HealingMult + b.HealingMult,
            Armor = a.Armor + b.Armor,
            StunIgnoreChance = a.StunIgnoreChance + b.StunIgnoreChance,
            StunResistanceRate = a.StunResistanceRate + b.StunResistanceRate,
            CriticalChance = a.CriticalChance + b.CriticalChance,
            CriticalMult = a.CriticalMult + b.CriticalMult,
            YFGainMult = a.YFGainMult + b.YFGainMult,
            OFGainMult = a.OFGainMult + b.OFGainMult,
            AuraRange = a.AuraRange + b.AuraRange,
            AuraStr = a.AuraStr + b.AuraStr
        };
    }
    
    private SCharacterStat SubtractStat(SCharacterStat a, SCharacterStat b)
    {
        return new SCharacterStat
        {
            MoveSpd = a.MoveSpd - b.MoveSpd,
            CharSize = a.CharSize,
            Health = a.Health - b.Health,
            HealthRegen = a.HealthRegen - b.HealthRegen,
            HealingMult = a.HealingMult - b.HealingMult,
            Armor = a.Armor - b.Armor,
            StunIgnoreChance = a.StunIgnoreChance - b.StunIgnoreChance,
            StunResistanceRate = a.StunResistanceRate - b.StunResistanceRate,
            CriticalChance = a.CriticalChance - b.CriticalChance,
            CriticalMult = a.CriticalMult - b.CriticalMult,
            YFGainMult = a.YFGainMult - b.YFGainMult,
            OFGainMult = a.OFGainMult - b.OFGainMult,
            AuraRange = a.AuraRange - b.AuraRange,
            AuraStr = a.AuraStr - b.AuraStr
        };
    }

    
}