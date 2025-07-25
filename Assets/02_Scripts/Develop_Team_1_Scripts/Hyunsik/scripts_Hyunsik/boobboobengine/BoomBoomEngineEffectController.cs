using UnityEngine;

public class BoomBoomEngineEffectController : MonoBehaviour
{
    [Header("붕붕엔진 버프 클래스 연결")]
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
            Debug.Log("[EffectController] 초기화 완료");
        }
    }
    
    /// <summary>
    /// SkillID에 따라 대응하는 버프 실행
    /// </summary>
    public void ActivateEffect(int skillID)
    {
        switch (skillID)
        {
            case 301:  // 리비 엔진 (무적 버프)
                if (beeTailEffect != null)
                {
                    beeTailEffect.ExecuteEffect();
                    Debug.Log("[EffectController] BeeTailEffect 버프 발동");
                }
                else
                {
                    Debug.LogWarning("[EffectController] BeeTailEffect 연결 안됨");
                }
                break;

            case 303:  // 5초 Aura_Range 증가
                if (auraRangeEffect != null)
                {
                    auraRangeEffect.ExecuteEffect();
                    Debug.Log("[EffectController] Aura Range 버프 발동");
                }
                else
                {
                    Debug.LogWarning("[EffectController] AuraRangeEffect 연결 안됨");
                }
                break;

            case 304: // 너구리 엔진 (크리티컬 확률 100%)
                if (criticalEffect != null)
                {
                    criticalEffect.ExecuteEffect();
                    Debug.Log("[EffectController] 크리티컬 버프 발동");
                }
                else
                {
                    Debug.LogWarning("[EffectController] CriticalEffect 연결 안됨");
                }
                break;

            case 305: // 실드 엔진
                if (shieldEffect != null)
                {
                    shieldEffect.ExecuteEffect();
                    Debug.Log("[EffectController] 실드 엔진 발동");
                }
                break;
            
            case 310:
                if (timedInvincibleEffect != null)
                {
                    timedInvincibleEffect.ExecuteEffect();
                    Debug.Log("[EffectController] 310 무적 스킬 발동");
                }
                break;
         
            case 317: // 마술 모자
                if (magicHatEffect != null)
                {
                    magicHatEffect.ExecuteEffect();
                    Debug.Log("[EffectController] 마술모자 버프 발동");
                }
                break;
            
        }
    }
}