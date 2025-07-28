using UnityEngine;

[DisallowMultipleComponent] // �ߺ� AddComponent ����
public class DogHouseReviveEffect : MonoBehaviour, IBoomBoomEngineEffect
{
    [SerializeField][Range(0.01f, 1f)] private float revivePercent = 0.05f;
    private bool bReviveUsed = false;
    public bool IsReviveUsed => bReviveUsed; // �б�����
    private PlayerStatManager mStatManager;

    void Start() {
        var arr = GetComponents<DogHouseReviveEffect>();
        Debug.Log($"[DogHouseReviveEffect] Player�� ���� ����: {arr.Length}");
    }
    
    private void Awake()
    {
        // �ݵ�� �÷��̾ �پ�� ��
        mStatManager = GetComponent<PlayerStatManager>();
        if (mStatManager == null)
        {
            Debug.LogError("[��������] PlayerStatManager ����! DogHouseReviveEffect�� �÷��̾�� �ٿ��� ��");
        }
    }

    public void ExecuteEffect()
    {
        // �������� ���� ��� ȣ���� ����
        bReviveUsed = false;
        Debug.Log("[��������] ExecuteEffect()�� bReviveUsed�� false�� ����");
    }

    public bool TryRevive(PlayerStatManager statManager)
    {
        Debug.Log($"[��������] TryRevive ȣ��! bReviveUsed={bReviveUsed}");
        if (!IsReviveUsed)
        {
            
            float maxHp = statManager.GetStat().BaseStat.Health;
            float reviveHp = Mathf.Max(1f, maxHp * revivePercent);
            statManager.ForceRevive(reviveHp);
            bReviveUsed = true;   // **�� ������ ������ private �ʵ��**
            Debug.Log($"[��������] ��Ȱ ����, bReviveUsed={bReviveUsed}");
            return true;
        }
        Debug.Log("[��������] �̹� ��Ȱ��, ����!");
        return false;
    }
    
}