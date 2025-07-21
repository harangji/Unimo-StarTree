using System.Collections;
using UnityEngine;

/// <summary>
/// 5초 동안 Aura_Range n% 증가
/// </summary>
public class AuraRangeBoostEffect : MonoBehaviour, IBoomBoomEngineEffect
{
    [SerializeField] private float bonusPercent = 50f;
    [SerializeField] private float duration = 5f;

    private PlayerStatManager mStatManager;
    private bool bBuffActive = false;

    private void Awake()
    {
        mStatManager = PlaySystemRefStorage.playerStatManager;
    }

    public void ExecuteEffect()
    {
        if (!bBuffActive)
            StartCoroutine(ApplyAuraRangeBuff());
    }

    private IEnumerator ApplyAuraRangeBuff()
    {
        bBuffActive = true;

        var stat = mStatManager.GetStat();
        var finalStat = stat.FinalStat;

        float original = finalStat.AuraRange;
        finalStat.AuraRange = original * (1f + bonusPercent / 100f);

       // stat.FinalStat = finalStat;
        mStatManager.SetStat(stat);  // 혹은 적절한 메서드로 반영

        Debug.Log($"[붕붕엔진] Aura_Range {bonusPercent}% 증가 적용됨");

        yield return new WaitForSeconds(duration);

        finalStat.AuraRange = original;
        //stat.FinalStat = finalStat;
        mStatManager.SetStat(stat);

        Debug.Log("[붕붕엔진] Aura_Range 버프 종료");

        bBuffActive = false;
    }
}