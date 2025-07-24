using System.Collections;
using UnityEngine;

public class CriticalChanceBoostEffect : MonoBehaviour, IBoomBoomEngineEffect
{
    [SerializeField] private float duration = 10f;

    private PlayerStatManager mStatManager;
    private bool bBuffActive = false;

    private void Start()
    {
        // �÷��̾� ���� �Ŵ��� �ʱ�ȭ (������ ���)
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
            Debug.LogWarning("[CriticalChanceBoostEffect] ���õ�: PlayerStatManager�� null");
            return;
        }

        if (!bBuffActive)
        {
            StartCoroutine(ApplyCriticalBuff());
        }
        else
        {
            Debug.Log("[CriticalChanceBoostEffect] ���� ũ��Ƽ�� ������ �̹� ���� ��");
        }
    }

    private IEnumerator ApplyCriticalBuff()
    {
        bBuffActive = true;

        var stat = mStatManager.GetStat();
        var bonus = new SCharacterStat { CriticalChance = 1.0f };  // 100% Ȯ��

        stat.AddBonus(bonus);
        mStatManager.SetStat(stat);
        Debug.Log("[�غؿ���] ũ��Ƽ�� Ȯ�� 100% ���� �����");

        yield return new WaitForSeconds(duration);

        stat.RemoveBonus(bonus);
        mStatManager.SetStat(stat);
        Debug.Log("[�غؿ���] ũ��Ƽ�� Ȯ�� ���� ����");

        bBuffActive = false;
    }
}
