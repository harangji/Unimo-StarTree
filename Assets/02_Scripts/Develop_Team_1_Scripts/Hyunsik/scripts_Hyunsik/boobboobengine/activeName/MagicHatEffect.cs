using System.Collections;
using UnityEngine;

public class MagicHatEffect : MonoBehaviour, IBoomBoomEngineEffect
{
    [SerializeField] private float buffDuration = 7f;
    private PlayerStatManager mStatManager;
    private bool bBuffActive = false;
    private Coroutine activeRoutine;

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

    public void ExecuteEffect()
    {
        if (bBuffActive) return;
        activeRoutine = StartCoroutine(ApplyRandomBuff());
    }

    private IEnumerator ApplyRandomBuff()
    {
        bBuffActive = true;

        var stat = mStatManager.GetStat();
        var bonus = new SCharacterStat();
        EMagicBuffType selected = (EMagicBuffType)Random.Range(0, 3);

        switch (selected)
        {
            case EMagicBuffType.MoveSpd:
                bonus.MoveSpd = stat.BaseStat.MoveSpd * 0.1f;
                Debug.Log("[MagicHat] �̵��ӵ� +10%");
                break;
            case EMagicBuffType.AuraRange:
                bonus.AuraRange = stat.BaseStat.AuraRange * 0.1f;
                Debug.Log("[MagicHat] ���� ���� +10%");
                break;
            case EMagicBuffType.HealthRegen:
                bonus.HealthRegen = stat.BaseStat.HealthRegen * 0.1f;
                Debug.Log("[MagicHat] ü�� ����� +10%");
                break;
        }

        stat.AddBonus(bonus);
        Debug.Log($"[MagicHat] ���� ����: bonus.MoveSpd={bonus.MoveSpd}," +
                  $" stat.FinalStat.MoveSpd={stat.FinalStat.MoveSpd} (���� ��)");
        mStatManager.SetStat(stat);

        yield return new WaitForSeconds(buffDuration);
        
        PlaySystemRefStorage.engineEffectTriggerManager?.StartSkillInactiveTimer();
        
        stat.RemoveBonus(bonus);
        mStatManager.SetStat(stat);

        
        Debug.Log("[MagicHat] ���� ����");

        bBuffActive = false;
    }
}