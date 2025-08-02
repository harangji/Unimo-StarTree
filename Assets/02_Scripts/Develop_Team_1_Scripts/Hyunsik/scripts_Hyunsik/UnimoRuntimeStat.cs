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
        FinalStat = new SCharacterStat
        {
            MoveSpd = BaseStat.MoveSpd * (1 + BonusStat.MoveSpd),
            CharSize = BaseStat.CharSize,
            Health = BaseStat.Health * (1 + BonusStat.Health),
            HealthRegen = BaseStat.HealthRegen + BonusStat.HealthRegen,
            HealingMult = BaseStat.HealingMult * (1 + BonusStat.HealingMult),
            Armor = BaseStat.Armor * (1 + BonusStat.Armor),
            StunIgnoreChance = BaseStat.StunIgnoreChance + BonusStat.StunIgnoreChance,
            StunResistanceRate = BaseStat.StunResistanceRate + BonusStat.StunResistanceRate,
            CriticalChance = BaseStat.CriticalChance + BonusStat.CriticalChance,
            CriticalMult = BaseStat.CriticalMult * (1 + BonusStat.CriticalMult),
            YFGainMult = BaseStat.YFGainMult * (1 + BonusStat.YFGainMult),
            OFGainMult = BaseStat.OFGainMult * (1 + BonusStat.OFGainMult),
            AuraRange = BaseStat.AuraRange * (1 + BonusStat.AuraRange),
            AuraStr = BaseStat.AuraStr * (1 + BonusStat.AuraStr)
        };
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

    public string ToDebugString(string type = "Final")
    {
        SCharacterStat stat = type switch
        {
            "Base" => BaseStat,
            "Bonus" => BonusStat,
            _ => FinalStat
        };

        return $"[UnimoStat: {type}]" +
               $"\n▶ 이동속도: {stat.MoveSpd}" +
               $"\n▶ 체력: {stat.Health}" +
               $"\n▶ 방어력: {stat.Armor}" +
               $"\n▶ 스턴무시 확률: {stat.StunIgnoreChance}" +
               $"\n▶ 스턴 저항: {stat.StunResistanceRate}" +
               $"\n▶ 오라 범위: {stat.AuraRange}" +
               $"\n▶ 오라 강도: {stat.AuraStr}" +
               $"\n▶ 크리티컬 확률: {stat.CriticalChance}" +
               $"\n▶ 크리티컬 배율: {stat.CriticalMult}" +
               $"\n▶ 회복 배수: {stat.HealingMult}" +
               $"\n▶ 자연 회복: {stat.HealthRegen}" +
               $"\n▶ 노란별꽃 배수(YF): {stat.YFGainMult}" +
               $"\n▶ 주황별꽃 배수(OF): {stat.OFGainMult}";
    }
}