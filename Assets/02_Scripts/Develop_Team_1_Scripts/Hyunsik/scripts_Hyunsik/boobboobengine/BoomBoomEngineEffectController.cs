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
    [SerializeField] private AuraRangeSandCastleEffect sandCastleEffect;
    [SerializeField] private GameObject orbitAuraObject;
    [SerializeField] private GameObject cloudAuraObject;
    
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
    public void ActivateEffect(int skillID, PlayerStatManager target)
    {
        var engineData = BoomBoomEngineDatabase.GetEngineData(GameManager.Instance.SelectedEngineID);
        
        switch (skillID)
        {
            case 301:  // ���� ���� (���� ����)
                if (beeTailEffect != null)
                {
                    int engineID = GameManager.Instance.SelectedEngineID;
                    int level = EngineLevelSystem.GetUniqueLevel(engineID);

                    //  Init ȣ��
                    beeTailEffect.Init(engineID, level);

                    //  ExecuteEffect ����
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
                    int engineID = GameManager.Instance.SelectedEngineID;
                    int level = EngineLevelSystem.GetUniqueLevel(engineID); // ���� ���� ���

                    auraRangeEffect.Init(engineID, level);
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
                    int level = EngineLevelSystem.GetUniqueLevel(GameManager.Instance.SelectedEngineID);
                    shieldEffect.Init(GameManager.Instance.SelectedEngineID, level); //  �߰�
                    shieldEffect.ExecuteEffect();
                    Debug.Log("[EffectController] �ǵ� ���� �õ��� (���� ��� ��ٿ� ����)");
                }
                else
                {
                    Debug.LogWarning("[EffectController] shieldEffect ������� ����");
                }
                break;
            
            case 310:
                if (timedInvincibleEffect != null)
                {
                    int level = EngineLevelSystem.GetUniqueLevel(GameManager.Instance.SelectedEngineID);
                    timedInvincibleEffect.Init(GameManager.Instance.SelectedEngineID, level); //  �߰�
                    timedInvincibleEffect.ExecuteEffect();
                    Debug.Log("[EffectController] 310 ���� ��ų �ߵ� (���� ��� ����)");
                }
                else
                {
                    Debug.LogWarning("[EffectController] timedInvincibleEffect ������� ����");
                }
                break;
         
            case 317: // ���� ����
                if (magicHatEffect != null)
                {
                    magicHatEffect.ExecuteEffect();
                    Debug.Log("[EffectController] �������� ���� �ߵ�");
                }
                break;
            
            case 313: // �������� ���� (��Ȱ ����)
                break;
            
            case 323: // �𷡼� ���� (Aura_Range 20�� ���� ����)
                if (sandCastleEffect != null)
                {
                    Debug.Log($"[SandCastleEffect] ExecuteEffect ȣ���");
                    sandCastleEffect.ExecuteEffect(); // **�Ķ���� ����!**
                    Debug.Log("[EffectController] �𷡼� ����(323) ���� �ߵ�");
                }
                else
                {
                    Debug.LogWarning("[EffectController] SandCastleEffect ���� �ȵ�");
                }
                break;
            case 318: // ����׸� ���� (OrbitAura ON)
                if (orbitAuraObject != null)
                {
                    orbitAuraObject.SetActive(true); // Ȱ��ȭ
                    Debug.Log("[EffectController] OrbitAura Ȱ��ȭ (318 ����)");
                }
                else
                {
                    Debug.LogWarning("[EffectController] OrbitAura ������Ʈ �̿���");
                }
                
                break;
            case 319: // ���� ���� (CloudAura ON)
                if (cloudAuraObject != null)
                {
                    cloudAuraObject.SetActive(true); // CloudAura Ȱ��ȭ
                    Debug.Log("[EffectController] CloudAura Ȱ��ȭ (319 ����)");
                }
                else
                {
                    Debug.LogWarning("[EffectController] CloudAura ������Ʈ �̿���");
                }
                break;
            
            
        }
    }
}