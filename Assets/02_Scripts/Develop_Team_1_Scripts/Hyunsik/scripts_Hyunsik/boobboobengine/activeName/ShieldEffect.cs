using UnityEngine;
using System.Collections;

public class ShieldEffect : MonoBehaviour
{
    private bool bHasShield = false;
    private bool bCooldownInProgress = false;

    [SerializeField] private float shieldCooldown = 10f;
    private PlayerStatManager mStatManager;

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

    public bool TryConsumeShield()
    {
        if (!bHasShield) return false;

        bHasShield = false;
        Debug.Log("[ShieldEffect] 실드 소모됨 → 쿨다운 시작");
        ExecuteEffect(); // 다시 10초 대기 후 생성
        return true;
    }

    public bool HasShield() => bHasShield;
}
