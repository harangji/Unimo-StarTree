using UnityEngine;
using System.Collections;

public class ShieldEffect : MonoBehaviour
{
    private bool bHasShield = false;
    private bool bCooldownInProgress = false;

    [SerializeField] private float shieldCooldown = 10f;
    private PlayerStatManager mStatManager;
    
    private int mEngineID;
    private int mLevel;
    
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
    /// ���� ��� ��ٿ� ������ Init
    /// </summary>
    public void Init(int engineID, int level)
    {
        mEngineID = engineID;
        mLevel = level;

        var data = BoomBoomEngineDatabase.GetEngineData(engineID);
        if (data != null && data.GrowthTable != null)
        {
            shieldCooldown = data.GrowthTable[Mathf.Clamp(level, 0, data.GrowthTable.Length - 1)];
            Debug.Log($"[ShieldEffect] ��Ÿ�� ������: {shieldCooldown:F1}�� (���� {level})");
        }
        else
        {
            Debug.LogWarning($"[ShieldEffect] ���� ���̺� ����. �⺻ ��ٿ� ����: {shieldCooldown:F1}��");
        }
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

    /// <summary>
    /// �ǰ� �� �ǵ� �Һ� �� ��ٿ� �ٽ� ����
    /// </summary>
    public bool TryConsumeShield()
    {
        if (!bHasShield) return false;

        bHasShield = false;
        Debug.Log("[ShieldEffect] �ǵ� �Ҹ�� �� ��ٿ� ����");
        ExecuteEffect(); // �ǵ� �Ҹ� �� ��� ��ٿ� ����
        return true;
    }

    public bool HasShield() => bHasShield;
}
