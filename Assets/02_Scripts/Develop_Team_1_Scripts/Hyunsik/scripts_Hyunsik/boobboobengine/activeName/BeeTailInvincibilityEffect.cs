using System.Collections;
using UnityEngine;

public class BeeTailInvincibilityEffect : MonoBehaviour, IBoomBoomEngineEffect
{
    public int EngineID { get; set; }
    public int EngineLevel { get; set; }

    private float duration; // GrowthTable ��� ���� �ð�

    private PlayerStatManager mStatManager;
    private bool bIsBuffActive = false;

    /// <summary>
    /// �ʿ��� ��� �ܺο��� ���� �ð� �ʱ�ȭ (GrowthTable ���)
    /// </summary>
    public void Init(int engineID, int level)
    {
        EngineID = engineID;
        EngineLevel = level;

        var data = BoomBoomEngineDatabase.GetEngineData(engineID);
        if (data == null || data.GrowthTable == null || data.GrowthTable.Length == 0)
        {
            Debug.LogError($"[BeeTail] EngineData ���� - ID: {engineID}");
            duration = 0f;
            return;
        }

        duration = data.GrowthTable[Mathf.Clamp(level - 1, 0, data.GrowthTable.Length - 1)];
        Debug.Log($"[BeeTail] Init �Ϸ� - ID: {engineID}, Level: {level}, Duration: {duration}");
    }

    /// <summary>
    /// �ʿ��� ������ �ֽ� PlayerStatManager�� ������
    /// </summary>
    private PlayerStatManager StatManager
    {
        get
        {
            if (mStatManager == null)
            {
                mStatManager = PlaySystemRefStorage.playerStatManager;
            }
            return mStatManager;
        }
    }

    public void ExecuteEffect()
    {
        if (bIsBuffActive)
            return;

        if (duration <= 0f)
        {
            Debug.LogWarning($"[BeeTail] duration�� 0 ������. Init ȣ�� ���� Ȯ�� �ʿ�");
            return;
        }

        StartCoroutine(ApplyInvincibilityBuff());
    }

    private IEnumerator ApplyInvincibilityBuff()
    {
        bIsBuffActive = true;
        StatManager.SetTemporaryInvincibility(true);

        yield return new WaitForSeconds(duration);

        StatManager.SetTemporaryInvincibility(false);
        bIsBuffActive = false;

        Debug.Log($"[BeeTail] ���� ���� �Ϸ� ({duration}��)");
    }
}