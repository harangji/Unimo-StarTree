using UnityEngine;

public class UnimoRuntimeStat
{
    public SCharacterStat BaseStat { get; private set; }
    public SCharacterStat BonusStat { get; private set; }
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

    public void ResetBonus()
    {
        BonusStat = new SCharacterStat();
        RecalculateFinalStat();
    }

    private void RecalculateFinalStat()
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
}