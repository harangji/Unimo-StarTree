using System.Collections.Generic;
using UnityEngine;

public class BuffManager
{
    private readonly UnimoRuntimeStat mTargetStat;
    private readonly List<ActiveBuff> mActiveBuffs = new();

    public BuffManager(UnimoRuntimeStat stat)
    {
        mTargetStat = stat;
    }

    /// <summary>
    /// 새로운 버프 적용
    /// </summary>
    public void ApplyBuff(SCharacterStat statBonus, float duration)
    {
        var buff = new ActiveBuff(statBonus, duration);
        mActiveBuffs.Add(buff);

        mTargetStat.AddBonus(statBonus);  // 즉시 스탯에 적용
    }

    /// <summary>
    /// 버프 시간 관리 및 해제
    /// </summary>
    public void Update(float deltaTime)
    {
        for (int i = mActiveBuffs.Count - 1; i >= 0; i--)
        {
            var buff = mActiveBuffs[i];
            buff.UpdateBuff(deltaTime);

            if (buff.IsExpired)
            {
                mTargetStat.AddBonus(buff.StatBonus.Negate());  // 버프 제거
                mActiveBuffs.RemoveAt(i);
            }
        }
    }
}