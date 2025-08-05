using UnityEngine;
using System.Collections;

/// <summary>
/// �������� ȿ��: 3���� �� �ϳ��� ������ 7�ʰ� ������Ŵ (MoveSpd, AuraRange, HealthRegen)
/// </summary>
[DisallowMultipleComponent]
public class MagicHatEffect : MonoBehaviour, IBoomBoomEngineEffect
{
    [SerializeField] private float buffDuration = 7f;
    
    private PlayerStatManager mStatManager;
    private bool bBuffActive = false;
    private Coroutine activeRoutine;
    private float mBuffPercent = 0.1f; // �⺻�� 10%

    private enum EMagicBuffType { MoveSpd, AuraRange, HealthRegen }

    private void Start()
    {
        StartCoroutine(Initialize());
    }

    private IEnumerator Initialize()
    {
        yield return new WaitUntil(() => PlaySystemRefStorage.playerStatManager != null);
        mStatManager = PlaySystemRefStorage.playerStatManager;
    }

    public void Init(int engineID, int level)
    {
        var data = BoomBoomEngineDatabase.GetEngineData(engineID);
        if (data?.GrowthTable != null && data.GrowthTable.Length > level)
        {
            mBuffPercent = data.GrowthTable[level] * 10f;
            Debug.Log($"[MagicHat] Init �Ϸ� �� ���� ��ġ: {mBuffPercent:F1}% (���� {level})");
        }
        else
        {
            mBuffPercent = 10f; // �⺻��
            Debug.LogWarning("[MagicHat] GrowthTable ���� �� �⺻�� 10% ���");
        }
    }

    public void ExecuteEffect()
    {
        if (mStatManager == null)
        {
            mStatManager = PlaySystemRefStorage.playerStatManager;
            if (mStatManager == null)
            {
                Debug.LogWarning("[SandCastleEffect] StatManager ������ null");
                return;
            }
        }

        activeRoutine = StartCoroutine(ApplyRandomBuff());
    }

    private IEnumerator ApplyRandomBuff()
    {
        bBuffActive = true;

        var stat = mStatManager.GetStat();
        var bonus = new SCharacterStat();

        // �ۼ�Ʈ �� ���� ��ȯ
        float bonusRatio = mBuffPercent / 100f;

        EMagicBuffType selected = (EMagicBuffType)Random.Range(0, 3);

        switch (selected)
        {
            case EMagicBuffType.MoveSpd:
                bonus.MoveSpd = stat.BaseStat.MoveSpd * bonusRatio;
                Debug.Log($"[MagicHat] �̵��ӵ� +{mBuffPercent:F1}%");
                break;
            case EMagicBuffType.AuraRange: 
                bonus.AuraRange = stat.BaseStat.AuraRange * bonusRatio;
                Debug.Log($"[MagicHat] ���� ���� +{mBuffPercent:F1}%");
                break;
            case EMagicBuffType.HealthRegen:
                bonus.HealthRegen = stat.BaseStat.HealthRegen * bonusRatio;
                Debug.Log($"[MagicHat] ü�� ����� +{mBuffPercent:F1}%");
                break;
        }

        stat.AddBonus(bonus);
        mStatManager.SetStat(stat);

        yield return new WaitForSeconds(buffDuration);

        stat.RemoveBonus(bonus);
        mStatManager.SetStat(stat);
        bBuffActive = false;

        Debug.Log("[MagicHat] ���� ����");

        PlaySystemRefStorage.engineEffectTriggerManager?.StartSkillInactiveTimer();
    }
}