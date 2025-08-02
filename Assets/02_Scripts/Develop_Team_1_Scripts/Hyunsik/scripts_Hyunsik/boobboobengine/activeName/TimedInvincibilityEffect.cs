using System.Collections;
using UnityEngine;

public class TimedInvincibilityEffect : MonoBehaviour, IBoomBoomEngineEffect
{
    [SerializeField] private float invincibleDuration = 5f;
    private float repeatCooldown;

    private PlayerStatManager mStatManager;
    private Coroutine loopRoutine;
    private bool bIsInitialized = false;

    public void Init(int engineID, int level)
    {
        var data = BoomBoomEngineDatabase.GetEngineData(engineID);
        if (data != null && data.GrowthTable != null && data.GrowthTable.Length > level)
        {
            repeatCooldown = data.GrowthTable[Mathf.Clamp(level, 0, data.GrowthTable.Length - 1)];
            Debug.Log($"[���� ��ų 310] ��ٿ� ������ �� {repeatCooldown}�� (���� {level})");
            bIsInitialized = true;
        }
        else
        {
            repeatCooldown = 30f; // fallback
            Debug.LogWarning($"[���� ��ų 310] ���� ���̺� ����. �⺻ ��ٿ� ���: {repeatCooldown}��");
        }
    }

    public void ExecuteEffect()
    {
        if (!bIsInitialized)
        {
            Debug.LogWarning("[���� ��ų 310] Init ȣ����� ����! ��ٿ� �� �̼��� ����");
            return;
        }

        if (loopRoutine != null) return;
        loopRoutine = StartCoroutine(InvincibilityLoop());
    }

    private void Start()
    {
        StartCoroutine(Initialize());
    }

    private IEnumerator Initialize()
    {
        yield return new WaitUntil(() => PlaySystemRefStorage.playerStatManager != null);
        mStatManager = PlaySystemRefStorage.playerStatManager;
    }

    private IEnumerator InvincibilityLoop()
    {
        yield return new WaitForSeconds(repeatCooldown); // ó�� ��� �ð�
        while (true)
        {
            mStatManager.SetTemporaryInvincibility(true);
            Debug.Log($"[���� ��ų 310] ���� ���� �� {invincibleDuration}��");

            yield return new WaitForSeconds(invincibleDuration);

            mStatManager.SetTemporaryInvincibility(false);
            Debug.Log("[���� ��ų 310] ���� ����");

            yield return new WaitForSeconds(repeatCooldown);
        }
    }
}