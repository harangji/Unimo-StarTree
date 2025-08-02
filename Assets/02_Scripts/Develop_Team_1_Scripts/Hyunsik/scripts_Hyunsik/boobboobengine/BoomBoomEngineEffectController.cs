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
    [SerializeField] private AuraRangeSandCastleEffect sandCastleEffect;
    [SerializeField] private GameObject orbitAuraObject;
    [SerializeField] private GameObject cloudAuraObject;
    
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
    public void ActivateEffect(int skillID, PlayerStatManager target)
    {
        var engineData = BoomBoomEngineDatabase.GetEngineData(GameManager.Instance.SelectedEngineID);
        
        switch (skillID)
        {
            case 301:  // 리비 엔진 (무적 버프)
                if (beeTailEffect != null)
                {
                    int engineID = GameManager.Instance.SelectedEngineID;
                    int level = EngineLevelSystem.GetUniqueLevel(engineID);

                    //  Init 호출
                    beeTailEffect.Init(engineID, level);

                    //  ExecuteEffect 실행
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
                    int engineID = GameManager.Instance.SelectedEngineID;
                    int level = EngineLevelSystem.GetUniqueLevel(engineID); // 고유 레벨 기반

                    auraRangeEffect.Init(engineID, level);
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
                    int level = EngineLevelSystem.GetUniqueLevel(GameManager.Instance.SelectedEngineID);
                    shieldEffect.Init(GameManager.Instance.SelectedEngineID, level); //  추가
                    shieldEffect.ExecuteEffect();
                    Debug.Log("[EffectController] 실드 생성 시도됨 (레벨 기반 쿨다운 적용)");
                }
                else
                {
                    Debug.LogWarning("[EffectController] shieldEffect 연결되지 않음");
                }
                break;
            
            case 310:
                if (timedInvincibleEffect != null)
                {
                    int level = EngineLevelSystem.GetUniqueLevel(GameManager.Instance.SelectedEngineID);
                    timedInvincibleEffect.Init(GameManager.Instance.SelectedEngineID, level); //  추가
                    timedInvincibleEffect.ExecuteEffect();
                    Debug.Log("[EffectController] 310 무적 스킬 발동 (레벨 기반 적용)");
                }
                else
                {
                    Debug.LogWarning("[EffectController] timedInvincibleEffect 연결되지 않음");
                }
                break;
         
            case 317: // 마술 모자
                if (magicHatEffect != null)
                {
                    magicHatEffect.ExecuteEffect();
                    Debug.Log("[EffectController] 마술모자 버프 발동");
                }
                break;
            
            case 313: // 도베르만 엔진 (부활 리셋)
                break;
            
            case 323: // 모래성 엔진 (Aura_Range 20초 누적 성장)
                if (sandCastleEffect != null)
                {
                    Debug.Log($"[SandCastleEffect] ExecuteEffect 호출됨");
                    sandCastleEffect.ExecuteEffect(); // **파라미터 없이!**
                    Debug.Log("[EffectController] 모래성 엔진(323) 버프 발동");
                }
                else
                {
                    Debug.LogWarning("[EffectController] SandCastleEffect 연결 안됨");
                }
                break;
            case 318: // 개밥그릇 엔진 (OrbitAura ON)
                if (orbitAuraObject != null)
                {
                    orbitAuraObject.SetActive(true); // 활성화
                    Debug.Log("[EffectController] OrbitAura 활성화 (318 엔진)");
                }
                else
                {
                    Debug.LogWarning("[EffectController] OrbitAura 오브젝트 미연결");
                }
                
                break;
            case 319: // 구름 엔진 (CloudAura ON)
                if (cloudAuraObject != null)
                {
                    cloudAuraObject.SetActive(true); // CloudAura 활성화
                    Debug.Log("[EffectController] CloudAura 활성화 (319 엔진)");
                }
                else
                {
                    Debug.LogWarning("[EffectController] CloudAura 오브젝트 미연결");
                }
                break;
            
            
        }
    }
}