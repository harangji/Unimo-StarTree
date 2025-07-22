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
        TryFindPlayerStatManager();
    }

    private void TryFindPlayerStatManager()
    {
        if (PlaySystemRefStorage.playerStatManager != null)
        {
            mStatManager = PlaySystemRefStorage.playerStatManager;
            Debug.Log("[AuraRangeBoostEffect] PlayerStatManager ���� �Ϸ�.");
        }
        else
        {
            Debug.LogWarning("[AuraRangeBoostEffect] PlayerStatManager�� ���� �ʱ�ȭ���� ����. ���� ȣ�⿡�� ��õ� ����.");
        }
    }
    
    public void ExecuteEffect()
    {
        if (mStatManager == null)
        {
            TryFindPlayerStatManager();
            if (mStatManager == null)
            {
                Debug.LogWarning("[AuraRangeBoostEffect] ���õ�: PlayerStatManager�� ������ null.");
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
        float bonusValue = stat.BaseStat.AuraRange * (bonusPercent / 100f);

        // �� BonusStat �ӽ� ������ ������ ���� �� �ٽ� ����
        var bonusStat = stat.BonusStat;
        bonusStat.AuraRange += bonusValue;
        stat.BonusStat = bonusStat;

        // �� ���� ���� ���� �� ����
        stat.RecalculateFinalStat();
        mStatManager.SetStat(stat);

        // �� �ƿ�� �ݿ� (�ʿ��)
        mStatManager.GetAuraController().SetAuraRange(stat.FinalStat.AuraRange);

        Debug.Log($"[�غؿ���] AuraRange +{bonusValue} ����� ({bonusPercent}%)");

        yield return new WaitForSeconds(duration);

        // �� ���� ���� ��
        bonusStat = stat.BonusStat;
        bonusStat.AuraRange -= bonusValue;
        stat.BonusStat = bonusStat;

        stat.RecalculateFinalStat();
        mStatManager.SetStat(stat);

        mStatManager.GetAuraController().SetAuraRange(stat.FinalStat.AuraRange);

        Debug.Log("[�غؿ���] AuraRange ���� ����");

        bBuffActive = false;
    }
}