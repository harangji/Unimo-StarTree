using UnityEngine;
using System.Collections;

public class AuraRangeSandCastleEffect : MonoBehaviour, IBoomBoomEngineEffect
{
    [SerializeField][Range(0.01f, 1f)] private float rangeIncreasePercent = 0.5f;   // ���� 50% ����
    [SerializeField] private float growDuration = 20f; // 20�� ���� �ִ�ġ
    private PlayerStatManager mStatManager;
    private Coroutine buffCoroutine;
    private bool isBuffActive = false;

    private void Start()
    {
        StartCoroutine(Initialize());
    }
    
    private IEnumerator Initialize()
    {
        // �÷��̾� ������Ʈ ������ �غ�� ������ ���
        yield return new WaitUntil(() => PlaySystemRefStorage.playerStatManager != null);
        mStatManager = PlaySystemRefStorage.playerStatManager;

        // (���� ���� üũ ���� ����. ���ʿ�� ������ ��)
        var engineData = BoomBoomEngineDatabase.GetEngineData(GameManager.Instance.SelectedEngineID);
        if (engineData != null && engineData.SkillID == 323)
        {
            ExecuteEffect();
        }
    }
    
    /// <summary>
    /// (�����δ� Start��Initialize������ �ڵ� ȣ��)
    /// </summary>
    public void ExecuteEffect()
    {
        Debug.Log("[SandCastleEffect] ExecuteEffect: �ڵ� AuraRange ���� ����");
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
            float totalBuff = rangeIncreasePercent * percent;
            float baseAura = mStatManager.GetStat().BaseStat.AuraRange;
            float newAura = baseAura * (1f + totalBuff);

            mStatManager.SetAuraRange(newAura);

            // �ǽð� �α�
            // Debug.Log($"[SandCastleEffect] AuraRange ���� ��: {newAura} ({percent * 100:0.#}%)");
            yield return null;
        }
        // 20�� �� �ִ�ġ ����
        float finalAura = mStatManager.GetStat().BaseStat.AuraRange * (1f + rangeIncreasePercent);
        mStatManager.SetAuraRange(finalAura);

        Debug.Log("[SandCastleEffect] AuraRange �ִ�ġ ����");
        isBuffActive = false;
        buffCoroutine = null;
    }

    /// <summary>
    /// �ǰ� �� ���� �ʱ�ȭ (PlayerStatManager���� ȣ��)
    /// </summary>
    public void ResetBuff()
    {
        Debug.Log("[SandCastleEffect] ResetBuff ȣ��� - ���� ũ�� ����");
        if (buffCoroutine != null)
            StopCoroutine(buffCoroutine);

        float baseAura = mStatManager.GetStat().BaseStat.AuraRange;
        mStatManager.SetAuraRange(baseAura);
        buffCoroutine = null;
        isBuffActive = false;
    }
}