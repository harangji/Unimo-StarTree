using UnityEngine;

public interface IBlessingEffect
{
    void Apply(PlayerStatManager target, int grade);
}

public class Blessing_MoveSpeed : IBlessingEffect
{
    private readonly float[] moveSpeedRates = { 0.10f, 0.12f, 0.14f, 0.15f };
    
    public void Apply(PlayerStatManager target, int grade)
    {
        UnimoRuntimeStat baseStat = target.GetStat();
        
        SCharacterStat bonus = new SCharacterStat
        {
            MoveSpd = baseStat.BaseStat.MoveSpd * moveSpeedRates[grade]
        };
        
        baseStat.AddBonus(bonus);
    }
}

public class Blessing_Armor : IBlessingEffect
{
    private readonly float[] armorRates = { 0.15f, 0.17f, 0.20f, 0.25f };

    public void Apply(PlayerStatManager target, int grade)
    {
        UnimoRuntimeStat baseStat = target.GetStat();

        SCharacterStat bonus = new SCharacterStat
        {
            Armor = baseStat.BaseStat.Armor * armorRates[grade],
        };
        
        baseStat.AddBonus(bonus);
    }
}

public class Blessing_ResourceGain : IBlessingEffect
{
    private readonly float[] yfGain = { 0.10f, 0.15f, 0.20f, 0.25f };
    private readonly float[] ofGain = { 0.05f, 0.07f, 0.09f, 0.10f };
    
    public void Apply(PlayerStatManager target, int grade)
    {
        UnimoRuntimeStat baseStat = target.GetStat();

        SCharacterStat bonus = new SCharacterStat
        {
            YFGainMult = baseStat.BaseStat.YFGainMult * yfGain[grade],
            OFGainMult = baseStat.BaseStat.OFGainMult * ofGain[grade]
        };
        
        baseStat.AddBonus(bonus);
    }
}

public class Blessing_AuraBoost : IBlessingEffect
{
    private readonly float[] auraRates = { 0.05f, 0.07f, 0.09f, 0.10f };
    
    public void Apply(PlayerStatManager target, int grade)
    {
        UnimoRuntimeStat baseStat = target.GetStat();

        SCharacterStat bonus = new SCharacterStat
        {
            AuraStr = baseStat.BaseStat.AuraStr * auraRates[grade],
        };
        
        baseStat.AddBonus(bonus);
    }
}
