using UnityEngine;
using System.Collections;

/// <summary>
/// 마술모자 효과: 3가지 중 하나의 스탯을 7초간 증가시킴 (MoveSpd, AuraRange, HealthRegen)
/// </summary>
[DisallowMultipleComponent]
public class MagicHatEffect : MonoBehaviour, IBoomBoomEngineEffect
{
    [SerializeField] private float buffDuration = 7f;
    
    private PlayerStatManager mStatManager;
    private bool bBuffActive = false;
    private Coroutine activeRoutine;
    private float mBuffPercent = 0.1f; // 기본값 10%

    private enum EMagicBuffType { MoveSpd, AuraRange, HealthRegen }

    private void Start()
    {
        StartCoroutine(Initialize());
    }

    private IEnumerator Initialize()
    {
        yield return new WaitUntil(() => PlaySystemRefStorage.playerStatManager != null);
        mStatManager = PlaySystemRefStorage.playerStatManager;
    }

    public void Init(int engineID, int level)
    {
        var data = BoomBoomEngineDatabase.GetEngineData(engineID);
        if (data?.GrowthTable != null && data.GrowthTable.Length > level)
        {
            mBuffPercent = data.GrowthTable[level] * 10f;
            Debug.Log($"[MagicHat] Init 완료 ▶ 버프 수치: {mBuffPercent:F1}% (레벨 {level})");
        }
        else
        {
            mBuffPercent = 10f; // 기본값
            Debug.LogWarning("[MagicHat] GrowthTable 없음 → 기본값 10% 사용");
        }
    }

    public void ExecuteEffect()
    {
        if (mStatManager == null)
        {
            mStatManager = PlaySystemRefStorage.playerStatManager;
            if (mStatManager == null)
            {
                Debug.LogWarning("[SandCastleEffect] StatManager 여전히 null");
                return;
            }
        }

        activeRoutine = StartCoroutine(ApplyRandomBuff());
    }

    private IEnumerator ApplyRandomBuff()
    {
        bBuffActive = true;

        var stat = mStatManager.GetStat();
        var bonus = new SCharacterStat();

        // 퍼센트 → 비율 변환
        float bonusRatio = mBuffPercent / 100f;

        EMagicBuffType selected = (EMagicBuffType)Random.Range(0, 3);

        switch (selected)
        {
            case EMagicBuffType.MoveSpd:
                bonus.MoveSpd = stat.BaseStat.MoveSpd * bonusRatio;
                Debug.Log($"[MagicHat] 이동속도 +{mBuffPercent:F1}%");
                break;
            case EMagicBuffType.AuraRange: 
                bonus.AuraRange = stat.BaseStat.AuraRange * bonusRatio;
                Debug.Log($"[MagicHat] 오라 범위 +{mBuffPercent:F1}%");
                break;
            case EMagicBuffType.HealthRegen:
                bonus.HealthRegen = stat.BaseStat.HealthRegen * bonusRatio;
                Debug.Log($"[MagicHat] 체력 재생률 +{mBuffPercent:F1}%");
                break;
        }

        stat.AddBonus(bonus);
        mStatManager.SetStat(stat);

        yield return new WaitForSeconds(buffDuration);

        stat.RemoveBonus(bonus);
        mStatManager.SetStat(stat);
        bBuffActive = false;

        Debug.Log("[MagicHat] 버프 종료");

        PlaySystemRefStorage.engineEffectTriggerManager?.StartSkillInactiveTimer();
    }
}