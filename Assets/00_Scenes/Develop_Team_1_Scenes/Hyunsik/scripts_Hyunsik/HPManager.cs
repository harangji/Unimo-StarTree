using UnityEngine;

public class HPManager : MonoBehaviour
{
    [SerializeField] private float maxHP = 100f;
    private float currentHP;

    [SerializeField] private HPGaugeController hpGauge;

    [Header("ȸ�� ����")]
    [SerializeField] private float regenAmountPerSecond = 5f; // �ʴ� ȸ����
    [SerializeField] private float fKeyHealAmount = 20f;       // F Ű ȸ����
    
    
    private bool bIsDead = false;
    
    void Awake()
    {
        currentHP = maxHP;
        hpGauge.SetGauge(1f);
    }

    void Update()
    {
        // ������ �׽�Ʈ (�����̽���)
        if (Input.GetKeyDown(KeyCode.Space) && !bIsDead)
        {
            TakeDamage(20f);
        }

        // ȸ�� �׽�Ʈ (FŰ)
        if (Input.GetKeyDown(KeyCode.F) && !bIsDead)
        {
            Heal(fKeyHealAmount);
        }
        
        // �ڿ� ȸ��
        if (!bIsDead)
        {
            Heal(regenAmountPerSecond * Time.deltaTime);
        }
    }

    public void TakeDamage(float damage)
    {
        if (bIsDead) return;

        currentHP -= damage;
        currentHP = Mathf.Max(currentHP, 0f);
        hpGauge.SetGauge(currentHP / maxHP);

        Debug.Log($"�÷��̾ {damage}�� ���ظ� �Ծ����ϴ�. ���� HP: {currentHP}");
        
        if (currentHP <= 0f)
        {
            bIsDead = true;
            Debug.Log("�÷��̾� ���!");
            PlaySystemRefStorage.playProcessController.TimeUp();
        }
    }

    public void Heal(float amount)
    {
        if (bIsDead) return;

        currentHP += amount;
        currentHP = Mathf.Min(currentHP, maxHP);
        hpGauge.SetGauge(currentHP / maxHP);
    }
}