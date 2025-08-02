using System.Collections;
using UnityEngine;

public class TimedInvincibilityEffect : MonoBehaviour, IBoomBoomEngineEffect
{
    [SerializeField] private float invincibleDuration = 5f;
    private float repeatCooldown;

    private PlayerStatManager mStatManager;
    private Coroutine loopRoutine;
    private bool bIsInitialized = false;

    public void Init(int engineID, int level)
    {
        var data = BoomBoomEngineDatabase.GetEngineData(engineID);
        if (data != null && data.GrowthTable != null && data.GrowthTable.Length > level)
        {
            repeatCooldown = data.GrowthTable[Mathf.Clamp(level, 0, data.GrowthTable.Length - 1)];
            Debug.Log($"[무적 스킬 310] 쿨다운 설정됨 ▶ {repeatCooldown}초 (레벨 {level})");
            bIsInitialized = true;
        }
        else
        {
            repeatCooldown = 30f; // fallback
            Debug.LogWarning($"[무적 스킬 310] 성장 테이블 없음. 기본 쿨다운 사용: {repeatCooldown}초");
        }
    }

    public void ExecuteEffect()
    {
        if (!bIsInitialized)
        {
            Debug.LogWarning("[무적 스킬 310] Init 호출되지 않음! 쿨다운 값 미설정 상태");
            return;
        }

        if (loopRoutine != null) return;
        loopRoutine = StartCoroutine(InvincibilityLoop());
    }

    private void Start()
    {
        StartCoroutine(Initialize());
    }

    private IEnumerator Initialize()
    {
        yield return new WaitUntil(() => PlaySystemRefStorage.playerStatManager != null);
        mStatManager = PlaySystemRefStorage.playerStatManager;
    }

    private IEnumerator InvincibilityLoop()
    {
        yield return new WaitForSeconds(repeatCooldown); // 처음 대기 시간
        while (true)
        {
            mStatManager.SetTemporaryInvincibility(true);
            Debug.Log($"[무적 스킬 310] 무적 시작 ▶ {invincibleDuration}초");

            yield return new WaitForSeconds(invincibleDuration);

            mStatManager.SetTemporaryInvincibility(false);
            Debug.Log("[무적 스킬 310] 무적 종료");

            yield return new WaitForSeconds(repeatCooldown);
        }
    }
}