using System.Collections;
using UnityEngine;

/// <summary>
/// 5�� ���� Aura_Range n% ����
/// </summary>
public class AuraRangeBoostEffect : MonoBehaviour, IBoomBoomEngineEffect
{
    public int EngineID { get; set; }
    public int EngineLevel { get; set; }

    [SerializeField] private float duration = 5f;
    private float bonusPercent = 0f;         // �ۼ�Ʈ (��: 20.3)
    private float cachedBonusRatio = 0f;     // 0.203 ����
    private float cachedBonusValue = 0f;     // ��: 6.92 * 0.203 = 1.40

    private PlayerStatManager mStatManager;
    private bool bBuffActive = false;

    private void Awake()
    {
        TryFindPlayerStatManager();
    }

    public void Init(int engineID, int level)
    {
        EngineID = engineID;
        EngineLevel = level;

        var data = BoomBoomEngineDatabase.GetEngineData(engineID);
        if (data != null && data.GrowthTable != null)
        {
            bonusPercent = data.GrowthTable[Mathf.Clamp(level, 0, data.GrowthTable.Length - 1)] * 100f;
            cachedBonusRatio = bonusPercent / 100f;

            Debug.Log($"[AuraRangeBoostEffect] Init �Ϸ�: {bonusPercent}% (���� {level})");
        }
        else
        {
            bonusPercent = 0f;
            cachedBonusRatio = 0f;
            Debug.LogWarning($"[AuraRangeBoostEffect] GrowthTable ����: EngineID={engineID}");
        }
    }

    private void TryFindPlayerStatManager()
    {
        mStatManager = PlaySystemRefStorage.playerStatManager;

        if (mStatManager != null)
            Debug.Log("[AuraRangeBoostEffect] PlayerStatManager ���� �Ϸ�.");
        else
            Debug.LogWarning("[AuraRangeBoostEffect] PlayerStatManager�� ���� �ʱ�ȭ���� ����.");
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
        float baseRange = stat.BaseStat.AuraRange;
        cachedBonusValue = baseRange * cachedBonusRatio;

        // ���� ����: BonusStat�� ���� ����
        var bonusStat = stat.BonusStat;
        bonusStat.AuraRange += cachedBonusRatio;
        stat.BonusStat = bonusStat;

        stat.RecalculateFinalStat();
        mStatManager.SetStat(stat);

        mStatManager.GetAuraController().SetAuraRange(stat.FinalStat.AuraRange);

        Debug.Log($"[�غؿ���] AuraRange +{cachedBonusValue:F2} ����� (����: {baseRange}, ������: {bonusPercent}%)");

        yield return new WaitForSeconds(duration);

        // ���� ����
        bonusStat = stat.BonusStat;
        bonusStat.AuraRange -= cachedBonusRatio;
        stat.BonusStat = bonusStat;

        stat.RecalculateFinalStat();
        mStatManager.SetStat(stat);

        mStatManager.GetAuraController().SetAuraRange(stat.FinalStat.AuraRange);
        Debug.Log("[�غؿ���] AuraRange ���� ����");

        bBuffActive = false;
    }
}
