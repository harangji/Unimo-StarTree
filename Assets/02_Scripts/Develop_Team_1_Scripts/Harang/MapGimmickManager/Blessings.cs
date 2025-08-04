// using UnityEngine;
//
// public interface IBlessingEffect
// {
//     void Apply(PlayerStatManager target, int grade);
// }
//
// public class Blessing_MoveSpeed : IBlessingEffect
// {
//     private readonly float[] moveSpeedRates = { 0.10f, 0.12f, 0.14f, 0.15f };
//
//     public void Apply(PlayerStatManager target, int grade)
//     {
//         float rate = moveSpeedRates[grade];
//         SCharacterStat stat = new SCharacterStat(moveSpd: rate);
//         
//         target.mStat.AddStat(stat);
//     }
// }
//
// public class Blessing_Armor : IBlessingEffect
// {
//     private readonly float[] armorRates = { 0.15f, 0.17f, 0.20f, 0.25f };
//
//     public void Apply(PlayerStatManager target, int grade)
//     {
//         float rate = armorRates[grade];
//         SCharacterStat stat = new SCharacterStat
//         {
//             Armor = rate,
//         };
//         
//         target.mStat.MultiplicationStat(stat);
//     }
// }
//
// public class Blessing_ResourceGain : IBlessingEffect
// {
//     private readonly float[] yfGain = { 0.10f, 0.15f, 0.20f, 0.25f };
//     private readonly float[] ofGain = { 0.05f, 0.07f, 0.09f, 0.10f };
//
//     public void Apply(PlayerStatManager target, int grade)
//     {
//         SCharacterStat stat = new SCharacterStat(yfGainMult: yfGain[grade], ofGainMult: ofGain[grade]);
//         
//         target.mStat.MultiplicationStat(stat);
//     }
// }
//
// public class Blessing_AuraBoost : IBlessingEffect
// {
//     private readonly float[] auraRates = { 0.05f, 0.07f, 0.09f, 0.10f };
//
//     public void Apply(PlayerStatManager target, int grade)
//     {
//         float rate = auraRates[grade];
//         SCharacterStat stat = new SCharacterStat
//         {
//             AuraStr = rate,
//         };
//         
//         target.mStat.MultiplicationStat(stat);
//     }
// }
