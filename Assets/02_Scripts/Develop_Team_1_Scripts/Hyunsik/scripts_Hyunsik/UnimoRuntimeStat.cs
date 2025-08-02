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
               $"\n�� �̵��ӵ�: {stat.MoveSpd}" +
               $"\n�� ü��: {stat.Health}" +
               $"\n�� ����: {stat.Armor}" +
               $"\n�� ���Ϲ��� Ȯ��: {stat.StunIgnoreChance}" +
               $"\n�� ���� ����: {stat.StunResistanceRate}" +
               $"\n�� ���� ����: {stat.AuraRange}" +
               $"\n�� ���� ����: {stat.AuraStr}" +
               $"\n�� ũ��Ƽ�� Ȯ��: {stat.CriticalChance}" +
               $"\n�� ũ��Ƽ�� ����: {stat.CriticalMult}" +
               $"\n�� ȸ�� ���: {stat.HealingMult}" +
               $"\n�� �ڿ� ȸ��: {stat.HealthRegen}" +
               $"\n�� ������� ���(YF): {stat.YFGainMult}" +
               $"\n�� ��Ȳ���� ���(OF): {stat.OFGainMult}";
    }
}