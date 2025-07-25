using System.Collections;
using UnityEngine;

public class TimedInvincibilityEffect : MonoBehaviour, IBoomBoomEngineEffect
{
    [SerializeField] private float initialDelay = 30f;     // 최초 발동까지 시간
    [SerializeField] private float invincibleDuration = 5f; // 무적 지속 시간
    [SerializeField] private float repeatCooldown = 30f;   // 무적 종료 후 재발동까지 시간

    private PlayerStatManager mStatManager;
    private Coroutine loopRoutine;

    private void Start()
    {
        StartCoroutine(Initialize());
    }

    private IEnumerator Initialize()
    {
        yield return new WaitUntil(() => PlaySystemRefStorage.playerStatManager != null);
        mStatManager = PlaySystemRefStorage.playerStatManager;
    }

    public void ExecuteEffect()
    {
        if (loopRoutine != null) return; // 이미 돌고 있으면 중복 실행 방지
        loopRoutine = StartCoroutine(InvincibilityLoop());
    }

    
    private IEnumerator InvincibilityLoop()
    {
        yield return new WaitForSeconds(initialDelay);
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