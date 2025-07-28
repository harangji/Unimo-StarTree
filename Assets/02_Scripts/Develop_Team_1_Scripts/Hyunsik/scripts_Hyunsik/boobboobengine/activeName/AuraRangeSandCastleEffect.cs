using UnityEngine;
using System.Collections;

public class AuraRangeSandCastleEffect : MonoBehaviour, IBoomBoomEngineEffect
{
    [SerializeField][Range(0.01f, 1f)] private float rangeIncreasePercent = 0.5f;   // 최종 50% 증가
    [SerializeField] private float growDuration = 20f; // 20초 만에 최대치
    private PlayerStatManager mStatManager;
    private Coroutine buffCoroutine;
    private bool isBuffActive = false;

    private void Start()
    {
        StartCoroutine(Initialize());
    }
    
    private IEnumerator Initialize()
    {
        // 플레이어 오브젝트 완전히 준비될 때까지 대기
        yield return new WaitUntil(() => PlaySystemRefStorage.playerStatManager != null);
        mStatManager = PlaySystemRefStorage.playerStatManager;

        // (엔진 장착 체크 생략 가능. 불필요시 지워도 됨)
        var engineData = BoomBoomEngineDatabase.GetEngineData(GameManager.Instance.SelectedEngineID);
        if (engineData != null && engineData.SkillID == 323)
        {
            ExecuteEffect();
        }
    }
    
    /// <summary>
    /// (실제로는 Start→Initialize에서만 자동 호출)
    /// </summary>
    public void ExecuteEffect()
    {
        Debug.Log("[SandCastleEffect] ExecuteEffect: 자동 AuraRange 증가 시작");
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
            float totalBuff = rangeIncreasePercent * percent;
            float baseAura = mStatManager.GetStat().BaseStat.AuraRange;
            float newAura = baseAura * (1f + totalBuff);

            mStatManager.SetAuraRange(newAura);

            // 실시간 로그
            // Debug.Log($"[SandCastleEffect] AuraRange 증가 중: {newAura} ({percent * 100:0.#}%)");
            yield return null;
        }
        // 20초 후 최대치 도달
        float finalAura = mStatManager.GetStat().BaseStat.AuraRange * (1f + rangeIncreasePercent);
        mStatManager.SetAuraRange(finalAura);

        Debug.Log("[SandCastleEffect] AuraRange 최대치 도달");
        isBuffActive = false;
        buffCoroutine = null;
    }

    /// <summary>
    /// 피격 시 버프 초기화 (PlayerStatManager에서 호출)
    /// </summary>
    public void ResetBuff()
    {
        Debug.Log("[SandCastleEffect] ResetBuff 호출됨 - 오라 크기 복구");
        if (buffCoroutine != null)
            StopCoroutine(buffCoroutine);

        float baseAura = mStatManager.GetStat().BaseStat.AuraRange;
        mStatManager.SetAuraRange(baseAura);
        buffCoroutine = null;
        isBuffActive = false;
    }
}