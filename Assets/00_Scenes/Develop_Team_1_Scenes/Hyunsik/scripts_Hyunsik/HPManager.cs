using UnityEngine;

public class HPManager : MonoBehaviour
{
    [SerializeField] private float maxHP = 100f;
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
        // 데미지 (스페이스바)
        if (Input.GetKeyDown(KeyCode.Space) && !bIsDead)
        {
            TakeDamage(10f);
        }

        // 회복 (F키)
        if (Input.GetKeyDown(KeyCode.F) && !bIsDead)
        {
            Heal(fKeyHealAmount);
        }

        // 지속 회복 (시간 기반)
        if (!bIsDead && currentHP < maxHP)
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

        if (currentHP <= 0f)
        {
            bIsDead = true;
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