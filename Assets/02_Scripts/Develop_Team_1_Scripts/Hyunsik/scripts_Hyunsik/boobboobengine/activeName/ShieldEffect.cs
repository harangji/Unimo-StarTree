using UnityEngine;
using System.Collections;

public class ShieldEffect : MonoBehaviour
{
    private bool bHasShield = false;
    private bool bCooldownInProgress = false;

    [SerializeField] private float shieldCooldown = 10f;
    private PlayerStatManager mStatManager;
    
    private int mEngineID;
    private int mLevel;
    
    private void Start()
    {
        StartCoroutine(Initialize());
    }


    private IEnumerator Initialize()
    {
        yield return new WaitUntil(() => PlaySystemRefStorage.playerStatManager != null);
        mStatManager = PlaySystemRefStorage.playerStatManager;
        mStatManager.RegisterShield(this);
    }

    /// <summary>
    /// 레벨 기반 쿨다운 설정용 Init
    /// </summary>
    public void Init(int engineID, int level)
    {
        mEngineID = engineID;
        mLevel = level;

        var data = BoomBoomEngineDatabase.GetEngineData(engineID);
        if (data != null && data.GrowthTable != null)
        {
            shieldCooldown = data.GrowthTable[Mathf.Clamp(level, 0, data.GrowthTable.Length - 1)];
            Debug.Log($"[ShieldEffect] 쿨타임 설정됨: {shieldCooldown:F1}초 (레벨 {level})");
        }
        else
        {
            Debug.LogWarning($"[ShieldEffect] 성장 테이블 없음. 기본 쿨다운 사용됨: {shieldCooldown:F1}초");
        }
    }
    
    /// <summary>
    /// 외부에서 실드 생성 시도 (게임 시작 또는 조건 만족 시)
    /// </summary>
    public void ExecuteEffect()
    {
        if (bHasShield || bCooldownInProgress)
        {
            Debug.Log("[ShieldEffect] 실드 이미 있음 또는 쿨다운 중");
            return;
        }

        StartCoroutine(SpawnShieldWithDelay());
    }

    private IEnumerator SpawnShieldWithDelay()
    {
        bCooldownInProgress = true;
        Debug.Log("[ShieldEffect] 실드 대기 중...");
        yield return new WaitForSeconds(shieldCooldown);

        bHasShield = true;
        bCooldownInProgress = false;

        Debug.Log("[ShieldEffect] 실드 생성됨");
    }

    /// <summary>
    /// 피격 시 실드 소비 → 쿨다운 다시 시작
    /// </summary>
    public bool TryConsumeShield()
    {
        if (!bHasShield) return false;

        bHasShield = false;
        Debug.Log("[ShieldEffect] 실드 소모됨 → 쿨다운 시작");
        ExecuteEffect(); // 실드 소모 시 즉시 쿨다운 시작
        return true;
    }

    public bool HasShield() => bHasShield;
}
