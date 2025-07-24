using UnityEngine;

public class BoomBoomEngineEffectController : MonoBehaviour
{
    [Header("�غؿ��� ���� Ŭ���� ����")]
    [SerializeField] private BeeTailInvincibilityEffect beeTailEffect;
    [SerializeField] private AuraRangeBoostEffect auraRangeEffect;
    [SerializeField] private CriticalChanceBoostEffect criticalEffect;
    [SerializeField] private ShieldEffect shieldEffect;
    [SerializeField] private TimedInvincibilityEffect timedInvincibleEffect;
    [SerializeField] private MagicHatEffect magicHatEffect;

    
    private void Awake()
    {
        if (PlaySystemRefStorage.engineEffectController == null)
        {
            PlaySystemRefStorage.engineEffectController = this;
            Debug.Log("[EffectController] �ʱ�ȭ �Ϸ�");
        }
    }
    
    /// <summary>
    /// SkillID�� ���� �����ϴ� ���� ����
    /// </summary>
    public void ActivateEffect(int skillID)
    {
        switch (skillID)
        {
            case 301:  // ���� ���� (���� ����)
                if (beeTailEffect != null)
                {
                    beeTailEffect.ExecuteEffect();
                    Debug.Log("[EffectController] BeeTailEffect ���� �ߵ�");
                }
                else
                {
                    Debug.LogWarning("[EffectController] BeeTailEffect ���� �ȵ�");
                }
                break;

            case 303:  // 5�� Aura_Range ����
                if (auraRangeEffect != null)
                {
                    auraRangeEffect.ExecuteEffect();
                    Debug.Log("[EffectController] Aura Range ���� �ߵ�");
                }
                else
                {
                    Debug.LogWarning("[EffectController] AuraRangeEffect ���� �ȵ�");
                }
                break;

            case 304: // �ʱ��� ���� (ũ��Ƽ�� Ȯ�� 100%)
                if (criticalEffect != null)
                {
                    criticalEffect.ExecuteEffect();
                    Debug.Log("[EffectController] ũ��Ƽ�� ���� �ߵ�");
                }
                else
                {
                    Debug.LogWarning("[EffectController] CriticalEffect ���� �ȵ�");
                }
                break;

            case 305: // �ǵ� ����
                if (shieldEffect != null)
                {
                    shieldEffect.ExecuteEffect();
                    Debug.Log("[EffectController] �ǵ� ���� �ߵ�");
                }
                break;
            
            case 310:
                if (timedInvincibleEffect != null)
                {
                    timedInvincibleEffect.ExecuteEffect();
                    Debug.Log("[EffectController] 310 ���� ��ų �ߵ�");
                }
                break;
         
            case 317: // ���� ����
                if (magicHatEffect != null)
                {
                    magicHatEffect.ExecuteEffect();
                    Debug.Log("[EffectController] �������� ���� �ߵ�");
                }
                break;
            
        }
    }
}