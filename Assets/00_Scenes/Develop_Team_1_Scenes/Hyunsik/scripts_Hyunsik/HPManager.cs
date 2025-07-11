using UnityEngine;

public class HPManager : MonoBehaviour
{
    [SerializeField] private float maxHP;
    private float currentHP;

    [SerializeField] private HPGaugeController hpGauge;

    [Header("회복 설정")]
    [SerializeField] private float regenAmountPerSecond = 5f; // 초당 회복량
    [SerializeField] private float fKeyHealAmount = 20f;       // F 키 회복량
    
    
    private bool bIsDead = false;
    
    void Awake()
    {
        currentHP = maxHP;
        hpGauge.SetGauge(1f);
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

        currentHP -= damage;
        currentHP = Mathf.Max(currentHP, 0f);
        hpGauge.SetGauge(currentHP / maxHP);

        Debug.Log($"플레이어가 {damage}의 피해를 입었습니다. 현재 HP: {currentHP}");
        
        if (currentHP <= 0f)
        {
            bIsDead = true;
            Debug.Log("플레이어 사망!");
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
    
    //캐릭터 스탯 적용. 정현식
    public void SetCharacterStat(UnimoRuntimeStat stat)
    {
        maxHP = stat.FinalStat.Health;
        currentHP = maxHP;

        regenAmountPerSecond = stat.FinalStat.HealthRegen;
        hpGauge.SetGauge(1f);
        
        Debug.Log($"[HPManager] 체력 설정됨. HP: {maxHP}, 회복/초: {regenAmountPerSecond}");
    }
    
}