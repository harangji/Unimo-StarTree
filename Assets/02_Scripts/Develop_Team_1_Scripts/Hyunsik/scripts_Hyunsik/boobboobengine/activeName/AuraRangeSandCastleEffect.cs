using UnityEngine;
using System.Collections;

public class AuraRangeSandCastleEffect : MonoBehaviour, IBoomBoomEngineEffect
{
    [SerializeField][Range(0.01f, 1f)] private float rangeIncreasePercent = 0.5f; // 예: 50%
    [SerializeField] private float growDuration = 20f; // 20초 만에 최대치까지 성장
    [SerializeField] private float maxIncreasePercent = 1.0f; // 최대 누적치 100%

    private Coroutine buffCoroutine;
    private bool isBuffActive = false;
    private PlayerStatManager mStatManager;

    // 항상 안전하게 접근하는 StatManager 프로퍼티
    private PlayerStatManager StatManager
    {
        get
        {
            if (mStatManager == null)
                mStatManager = PlaySystemRefStorage.playerStatManager;
            return mStatManager;
        }
    }

    private void Start()
    {
        // 혹시 모를 상황 대비 강제 할당
        mStatManager = PlaySystemRefStorage.playerStatManager;
    }

    public void ExecuteEffect()
    {
        if (buffCoroutine != null)
            StopCoroutine(buffCoroutine);

        buffCoroutine = StartCoroutine(RangeBuffRoutine());
    }

    // 20초 동안 서서히 아우라 증가
    private IEnumerator RangeBuffRoutine()
    {
        isBuffActive = true;
        float elapsed = 0f;
        while (elapsed < growDuration)
        {
            elapsed += Time.deltaTime;
            float percent = Mathf.Clamp01(elapsed / growDuration);
            float buffPercent = percent * rangeIncreasePercent;

            var statMan = StatManager;
            if (statMan == null)
            {
                Debug.LogWarning("[SandCastleEffect] PlayerStatManager가 null입니다! 버프 중단");
                yield break;
            }

            float baseAura = statMan.GetStat().BaseStat.AuraRange;
            float buffedAura = baseAura * (1f + buffPercent);

            statMan.SetAuraRange(buffedAura);

            yield return null;
        }
        // 최대치 도달 후 유지
        var finalStatMan = StatManager;
        if (finalStatMan != null)
        {
            float baseAura = finalStatMan.GetStat().BaseStat.AuraRange;
            float buffedAura = baseAura * (1f + rangeIncreasePercent);
            finalStatMan.SetAuraRange(buffedAura);
        }
        isBuffActive = false;
    }

    /// <summary>
    /// 피격 시 버프 초기화 (PlayerStatManager에서 호출)
    /// </summary>
    public void ResetBuff()
    {
        Debug.Log("[SandCastleEffect] ResetBuff 호출됨! 누적 초기화");
        if (buffCoroutine != null)
            StopCoroutine(buffCoroutine);

        var statMan = StatManager;
        if (statMan != null)
        {
            statMan.SetAuraRange(statMan.GetStat().BaseStat.AuraRange);
        }
        isBuffActive = false;
    }
}