using UnityEngine;

public class BoomBoomEngineEffectController : MonoBehaviour
{
    [Header("�غؿ��� ���� Ŭ���� ����")]
    [SerializeField] private BeeTailInvincibilityEffect beeTailEffect;
    [SerializeField] private AuraRangeBoostEffect auraRangeEffect;
    [SerializeField] private CriticalChanceBoostEffect criticalEffect;
    //[SerializeField] private ShieldEffect shieldEffect;
    
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

            case 305:
                Debug.Log("[EffectController] �� ���� ���� �ߵ� (�̱���)");
                break;

            default:
                Debug.LogWarning($"[EffectController] ��ϵ��� ���� ��ųID: {skillID}");
                break;
        }
    }
}