using System.Collections;
using UnityEngine;

public class CriticalChanceBoostEffect : MonoBehaviour, IBoomBoomEngineEffect
{
    [SerializeField] private float duration = 10f;

    private PlayerStatManager mStatManager;
    private bool bBuffActive = false;

    private void Start()
    {
        // 플레이어 스탯 매니저 초기화 (딜레이 허용)
        StartCoroutine(InitializeStatManager());
    }

    private IEnumerator InitializeStatManager()
    {
        yield return new WaitUntil(() => PlaySystemRefStorage.playerStatManager != null);
        mStatManager = PlaySystemRefStorage.playerStatManager;
    }

    public void ExecuteEffect()
    {
        if (mStatManager == null)
        {
            Debug.LogWarning("[CriticalChanceBoostEffect] 무시됨: PlayerStatManager가 null");
            return;
        }

        if (!bBuffActive)
        {
            StartCoroutine(ApplyCriticalBuff());
        }
        else
        {
            Debug.Log("[CriticalChanceBoostEffect] 현재 크리티컬 버프가 이미 적용 중");
        }
    }

    private IEnumerator ApplyCriticalBuff()
    {
        bBuffActive = true;

        var stat = mStatManager.GetStat();
        var bonus = new SCharacterStat { CriticalChance = 1.0f };  // 100% 확률

        stat.AddBonus(bonus);
        mStatManager.SetStat(stat);
        Debug.Log("[붕붕엔진] 크리티컬 확률 100% 증가 적용됨");

        yield return new WaitForSeconds(duration);

        stat.RemoveBonus(bonus);
        mStatManager.SetStat(stat);
        Debug.Log("[붕붕엔진] 크리티컬 확률 버프 종료");

        bBuffActive = false;
    }
}
