using UnityEngine;

public class HPManager : MonoBehaviour
{
    [SerializeField] private float maxHP;
    private float currentHP;

    [SerializeField] private HPGaugeController hpGauge;

    [Header("스탯 설정")]
    [SerializeField] private float regenAmountPerSecond; // 초당 회복량
    [SerializeField] private float fKeyHealAmount;       // F 키 회복량
    private float armor;              // 데미지 감소율
    private float healingMultiplier;  // 회복 배수
    
    private bool bIsDead = false;
    
    void Awake()
    {
    }

    void Update()
    {
        // 데미지 테스트 (스페이스바)
        if (Input.GetKeyDown(KeyCode.Space) && !bIsDead)
        {
            TakeDamage(240f);
        }

        // 회복 테스트 (F키)
        if (Input.GetKeyDown(KeyCode.F) && !bIsDead)
        {
            Heal(fKeyHealAmount);
        }
        
        // 자연 회복
        if (!bIsDead)
        {
            Heal(regenAmountPerSecond * Time.deltaTime);
        }
    }

    public void TakeDamage(float damage)
    {
        if (bIsDead) return;

        float reducedDamage = damage * (1f - armor); // 방어력 적용
        currentHP -= reducedDamage;
        currentHP = Mathf.Max(currentHP, 0f);
        hpGauge.SetGauge(currentHP / maxHP);

        Debug.Log($"플레이어가 {reducedDamage:F1} 피해를 입었습니다. (원래 {damage}, 방어력 {armor * 100:F0}%)");

        if (currentHP <= 0f)
        {
            bIsDead = true;
            Debug.Log("플레이어 사망!");
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
    
    //캐릭터 스탯 적용. 정현식
    public void SetCharacterStat(UnimoRuntimeStat stat)
    {
        maxHP = stat.FinalStat.Health;
        currentHP = maxHP;

        regenAmountPerSecond = stat.FinalStat.HealthRegen;
        healingMultiplier = stat.FinalStat.HealingMult;
        armor = stat.FinalStat.Armor;
        hpGauge.SetGauge(1f);
        
        Debug.Log($"[HPManager] 체력 설정됨. HP: {maxHP}," +
                  $" 회복/초: {regenAmountPerSecond}," +
                  $" 방어력: {armor}," +
                  $" 힐배수: {healingMultiplier}");
    }
}