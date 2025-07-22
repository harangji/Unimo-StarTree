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
    /// ���ο� ���� ����
    /// </summary>
    public void ApplyBuff(SCharacterStat statBonus, float duration)
    {
        var buff = new ActiveBuff(statBonus, duration);
        mActiveBuffs.Add(buff);

        mTargetStat.AddBonus(statBonus);  // ��� ���ȿ� ����
    }

    /// <summary>
    /// ���� �ð� ���� �� ����
    /// </summary>
    public void Update(float deltaTime)
    {
        for (int i = mActiveBuffs.Count - 1; i >= 0; i--)
        {
            var buff = mActiveBuffs[i];
            buff.UpdateBuff(deltaTime);

            if (buff.IsExpired)
            {
                mTargetStat.AddBonus(buff.StatBonus.Negate());  // ���� ����
                mActiveBuffs.RemoveAt(i);
            }
        }
    }
}