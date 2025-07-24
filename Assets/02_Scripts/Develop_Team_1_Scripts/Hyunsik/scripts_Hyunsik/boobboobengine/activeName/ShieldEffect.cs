using UnityEngine;
using System.Collections;

public class ShieldEffect : MonoBehaviour
{
    private bool bHasShield = false;
    private bool bCooldownInProgress = false;

    [SerializeField] private float shieldCooldown = 10f;
    private PlayerStatManager mStatManager;

    private void Start()
    {
        StartCoroutine(Initialize());
    }

    private IEnumerator Initialize()
    {
        yield return new WaitUntil(() => PlaySystemRefStorage.playerStatManager != null);
        mStatManager = PlaySystemRefStorage.playerStatManager;
        mStatManager.RegisterShield(this);
    }

    /// <summary>
    /// �ܺο��� �ǵ� ���� �õ� (���� ���� �Ǵ� ���� ���� ��)
    /// </summary>
    public void ExecuteEffect()
    {
        if (bHasShield || bCooldownInProgress)
        {
            Debug.Log("[ShieldEffect] �ǵ� �̹� ���� �Ǵ� ��ٿ� ��");
            return;
        }

        StartCoroutine(SpawnShieldWithDelay());
    }

    private IEnumerator SpawnShieldWithDelay()
    {
        bCooldownInProgress = true;
        Debug.Log("[ShieldEffect] �ǵ� ��� ��...");
        yield return new WaitForSeconds(shieldCooldown);
        bHasShield = true;
        bCooldownInProgress = false;
        Debug.Log("[ShieldEffect] �ǵ� ������");
    }

    public bool TryConsumeShield()
    {
        if (!bHasShield) return false;

        bHasShield = false;
        Debug.Log("[ShieldEffect] �ǵ� �Ҹ�� �� ��ٿ� ����");
        ExecuteEffect(); // �ٽ� 10�� ��� �� ����
        return true;
    }

    public bool HasShield() => bHasShield;
}
