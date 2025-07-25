using UnityEngine;
using System.Collections;

public class AuraRangeSandCastleEffect : MonoBehaviour, IBoomBoomEngineEffect
{
    [SerializeField][Range(0.01f, 1f)] private float rangeIncreasePercent = 0.5f; // ��: 50%
    [SerializeField] private float growDuration = 20f; // 20�� ���� �ִ�ġ���� ����
    [SerializeField] private float maxIncreasePercent = 1.0f; // �ִ� ����ġ 100%

    private Coroutine buffCoroutine;
    private bool isBuffActive = false;
    private PlayerStatManager mStatManager;

    // �׻� �����ϰ� �����ϴ� StatManager ������Ƽ
    private PlayerStatManager StatManager
    {
        get
        {
            if (mStatManager == null)
                mStatManager = PlaySystemRefStorage.playerStatManager;
            return mStatManager;
        }
    }

    private void Start()
    {
        // Ȥ�� �� ��Ȳ ��� ���� �Ҵ�
        mStatManager = PlaySystemRefStorage.playerStatManager;
    }

    public void ExecuteEffect()
    {
        if (buffCoroutine != null)
            StopCoroutine(buffCoroutine);

        buffCoroutine = StartCoroutine(RangeBuffRoutine());
    }

    // 20�� ���� ������ �ƿ�� ����
    private IEnumerator RangeBuffRoutine()
    {
        isBuffActive = true;
        float elapsed = 0f;
        while (elapsed < growDuration)
        {
            elapsed += Time.deltaTime;
            float percent = Mathf.Clamp01(elapsed / growDuration);
            float buffPercent = percent * rangeIncreasePercent;

            var statMan = StatManager;
            if (statMan == null)
            {
                Debug.LogWarning("[SandCastleEffect] PlayerStatManager�� null�Դϴ�! ���� �ߴ�");
                yield break;
            }

            float baseAura = statMan.GetStat().BaseStat.AuraRange;
            float buffedAura = baseAura * (1f + buffPercent);

            statMan.SetAuraRange(buffedAura);

            yield return null;
        }
        // �ִ�ġ ���� �� ����
        var finalStatMan = StatManager;
        if (finalStatMan != null)
        {
            float baseAura = finalStatMan.GetStat().BaseStat.AuraRange;
            float buffedAura = baseAura * (1f + rangeIncreasePercent);
            finalStatMan.SetAuraRange(buffedAura);
        }
        isBuffActive = false;
    }

    /// <summary>
    /// �ǰ� �� ���� �ʱ�ȭ (PlayerStatManager���� ȣ��)
    /// </summary>
    public void ResetBuff()
    {
        Debug.Log("[SandCastleEffect] ResetBuff ȣ���! ���� �ʱ�ȭ");
        if (buffCoroutine != null)
            StopCoroutine(buffCoroutine);

        var statMan = StatManager;
        if (statMan != null)
        {
            statMan.SetAuraRange(statMan.GetStat().BaseStat.AuraRange);
        }
        isBuffActive = false;
    }
}