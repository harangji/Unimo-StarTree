using System.Collections;
using UnityEngine;

/// <summary>
/// 5초 동안 Aura_Range n% 증가
/// </summary>
public class AuraRangeBoostEffect : MonoBehaviour, IBoomBoomEngineEffect
{
    public int EngineID { get; set; }
    public int EngineLevel { get; set; }

    [SerializeField] private float duration = 5f;
    private float bonusPercent = 0f;         // 퍼센트 (예: 20.3)
    private float cachedBonusRatio = 0f;     // 0.203 형태
    private float cachedBonusValue = 0f;     // 예: 6.92 * 0.203 = 1.40

    private PlayerStatManager mStatManager;
    private bool bBuffActive = false;

    private void Awake()
    {
        TryFindPlayerStatManager();
    }

    public void Init(int engineID, int level)
    {
        EngineID = engineID;
        EngineLevel = level;

        var data = BoomBoomEngineDatabase.GetEngineData(engineID);
        if (data != null && data.GrowthTable != null)
        {
            bonusPercent = data.GrowthTable[Mathf.Clamp(level, 0, data.GrowthTable.Length - 1)] * 100f;
            cachedBonusRatio = bonusPercent / 100f;

            Debug.Log($"[AuraRangeBoostEffect] Init 완료: {bonusPercent}% (레벨 {level})");
        }
        else
        {
            bonusPercent = 0f;
            cachedBonusRatio = 0f;
            Debug.LogWarning($"[AuraRangeBoostEffect] GrowthTable 없음: EngineID={engineID}");
        }
    }

    private void TryFindPlayerStatManager()
    {
        mStatManager = PlaySystemRefStorage.playerStatManager;

        if (mStatManager != null)
            Debug.Log("[AuraRangeBoostEffect] PlayerStatManager 연결 완료.");
        else
            Debug.LogWarning("[AuraRangeBoostEffect] PlayerStatManager가 아직 초기화되지 않음.");
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
        float baseRange = stat.BaseStat.AuraRange;
        cachedBonusValue = baseRange * cachedBonusRatio;

        // 버프 적용: BonusStat에 비율 적용
        var bonusStat = stat.BonusStat;
        bonusStat.AuraRange += cachedBonusRatio;
        stat.BonusStat = bonusStat;

        stat.RecalculateFinalStat();
        mStatManager.SetStat(stat);

        mStatManager.GetAuraController().SetAuraRange(stat.FinalStat.AuraRange);

        Debug.Log($"[붕붕엔진] AuraRange +{cachedBonusValue:F2} 적용됨 (기준: {baseRange}, 증가율: {bonusPercent}%)");

        yield return new WaitForSeconds(duration);

        // 버프 해제
        bonusStat = stat.BonusStat;
        bonusStat.AuraRange -= cachedBonusRatio;
        stat.BonusStat = bonusStat;

        stat.RecalculateFinalStat();
        mStatManager.SetStat(stat);

        mStatManager.GetAuraController().SetAuraRange(stat.FinalStat.AuraRange);
        Debug.Log("[붕붕엔진] AuraRange 버프 종료");

        bBuffActive = false;
    }
}
