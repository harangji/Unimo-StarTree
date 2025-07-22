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
        TryFindPlayerStatManager();
    }

    private void TryFindPlayerStatManager()
    {
        if (PlaySystemRefStorage.playerStatManager != null)
        {
            mStatManager = PlaySystemRefStorage.playerStatManager;
            Debug.Log("[AuraRangeBoostEffect] PlayerStatManager 연결 완료.");
        }
        else
        {
            Debug.LogWarning("[AuraRangeBoostEffect] PlayerStatManager가 아직 초기화되지 않음. 다음 호출에서 재시도 예정.");
        }
    }
    
    public void ExecuteEffect()
    {
        if (mStatManager == null)
        {
            TryFindPlayerStatManager();
            if (mStatManager == null)
            {
                Debug.LogWarning("[AuraRangeBoostEffect] 무시됨: PlayerStatManager가 여전히 null.");
                return;
            }
        }

        if (!bBuffActive)
            StartCoroutine(ApplyAuraRangeBuff());
    }
    
    private IEnumerator ApplyAuraRangeBuff()
    {
        bBuffActive = true;

        var stat = mStatManager.GetStat();
        float bonusValue = stat.BaseStat.AuraRange * (bonusPercent / 100f);

        // ① BonusStat 임시 변수로 가져와 수정 후 다시 대입
        var bonusStat = stat.BonusStat;
        bonusStat.AuraRange += bonusValue;
        stat.BonusStat = bonusStat;

        // ② 최종 스탯 재계산 및 적용
        stat.RecalculateFinalStat();
        mStatManager.SetStat(stat);

        // ③ 아우라 반영 (필요시)
        mStatManager.GetAuraController().SetAuraRange(stat.FinalStat.AuraRange);

        Debug.Log($"[붕붕엔진] AuraRange +{bonusValue} 적용됨 ({bonusPercent}%)");

        yield return new WaitForSeconds(duration);

        // ④ 버프 해제 시
        bonusStat = stat.BonusStat;
        bonusStat.AuraRange -= bonusValue;
        stat.BonusStat = bonusStat;

        stat.RecalculateFinalStat();
        mStatManager.SetStat(stat);

        mStatManager.GetAuraController().SetAuraRange(stat.FinalStat.AuraRange);

        Debug.Log("[붕붕엔진] AuraRange 버프 종료");

        bBuffActive = false;
    }
}