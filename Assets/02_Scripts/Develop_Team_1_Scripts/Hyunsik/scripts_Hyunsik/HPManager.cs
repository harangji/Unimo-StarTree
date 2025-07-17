using UnityEngine;

public class HPManager : MonoBehaviour
{
    [SerializeField] private float maxHP;
    private float currentHP;

    [SerializeField] private HPGaugeController hpGauge;

    [Header("���� ����")]
    [SerializeField] private float regenAmountPerSecond; // �ʴ� ȸ����
    [SerializeField] private float fKeyHealAmount;       // F Ű ȸ����
    private float armor;              // ������ ������
    private float healingMultiplier;  // ȸ�� ���
    
    private bool bIsDead = false;
    
    void Awake()
    {
    }

    void Update()
    {
        // ������ �׽�Ʈ (�����̽���)
        if (Input.GetKeyDown(KeyCode.Space) && !bIsDead)
        {
            TakeDamage(240f);
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

        float reducedDamage = damage * (1f - armor); // ���� ����
        currentHP -= reducedDamage;
        currentHP = Mathf.Max(currentHP, 0f);
        hpGauge.SetGauge(currentHP / maxHP);

        Debug.Log($"�÷��̾ {reducedDamage:F1} ���ظ� �Ծ����ϴ�. (���� {damage}, ���� {armor * 100:F0}%)");

        if (currentHP <= 0f)
        {
            bIsDead = true;
            Debug.Log("�÷��̾� ���!");
            PlaySystemRefStorage.playProcessController.GameOver();
        }
    }

    public void Heal(float amount)
    {
        if (bIsDead) return;

        float actualHeal = amount * healingMultiplier;
        currentHP += actualHeal;
        currentHP = Mathf.Min(currentHP, maxHP);
        hpGauge.SetGauge(currentHP / maxHP);
        
    }
    
    //ĳ���� ���� ����. ������
    public void SetCharacterStat(UnimoRuntimeStat stat)
    {
        maxHP = stat.FinalStat.Health;
        currentHP = maxHP;

        regenAmountPerSecond = stat.FinalStat.HealthRegen;
        healingMultiplier = stat.FinalStat.HealingMult;
        armor = stat.FinalStat.Armor;
        hpGauge.SetGauge(1f);
        
        Debug.Log($"[HPManager] ü�� ������. HP: {maxHP}," +
                  $" ȸ��/��: {regenAmountPerSecond}," +
                  $" ����: {armor}," +
                  $" �����: {healingMultiplier}");
    }
}