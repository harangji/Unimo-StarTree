using System.Collections;
using UnityEngine;

/// <summary>
/// 5�� ���� Aura_Range n% ����
/// </summary>
public class AuraRangeBoostEffect : MonoBehaviour, IBoomBoomEngineEffect
{
    [SerializeField] private float bonusPercent = 50f;
    [SerializeField] private float duration = 5f;

    private PlayerStatManager mStatManager;
    private bool bBuffActive = false;

    private void Awake()
    {
        mStatManager = PlaySystemRefStorage.playerStatManager;
    }

    public void ExecuteEffect()
    {
        if (!bBuffActive)
            StartCoroutine(ApplyAuraRangeBuff());
    }

    private IEnumerator ApplyAuraRangeBuff()
    {
        bBuffActive = true;

        var stat = mStatManager.GetStat();
        var finalStat = stat.FinalStat;

        float original = finalStat.AuraRange;
        finalStat.AuraRange = original * (1f + bonusPercent / 100f);

       // stat.FinalStat = finalStat;
        mStatManager.SetStat(stat);  // Ȥ�� ������ �޼���� �ݿ�

        Debug.Log($"[�غؿ���] Aura_Range {bonusPercent}% ���� �����");

        yield return new WaitForSeconds(duration);

        finalStat.AuraRange = original;
        //stat.FinalStat = finalStat;
        mStatManager.SetStat(stat);

        Debug.Log("[�غؿ���] Aura_Range ���� ����");

        bBuffActive = false;
    }
}