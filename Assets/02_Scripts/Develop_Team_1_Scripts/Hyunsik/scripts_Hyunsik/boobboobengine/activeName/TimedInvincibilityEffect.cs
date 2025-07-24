using System.Collections;
using UnityEngine;

public class TimedInvincibilityEffect : MonoBehaviour, IBoomBoomEngineEffect
{
    [SerializeField] private float initialDelay = 30f;     // ���� �ߵ����� �ð�
    [SerializeField] private float invincibleDuration = 5f; // ���� ���� �ð�
    [SerializeField] private float repeatCooldown = 30f;   // ���� ���� �� ��ߵ����� �ð�

    private PlayerStatManager mStatManager;
    private Coroutine loopRoutine;

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
        if (loopRoutine != null) return; // �̹� ���� ������ �ߺ� ���� ����
        loopRoutine = StartCoroutine(InvincibilityLoop());
    }

    
    private IEnumerator InvincibilityLoop()
    {
        yield return new WaitForSeconds(initialDelay);
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