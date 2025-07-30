using UnityEngine;

public class EngineEffectTriggerManager : MonoBehaviour
{
    [SerializeField] private BoomBoomEngineEffectController effectController;
    [SerializeField] private ShieldEffect shieldEffect;
    [SerializeField] private OrbitAuraController orbitAura;
    
    private int orangeFlowerCount = 0;
    private int yellowFlowerCount = 0;
    private float inactiveSkillTime = 0f;
    private bool bSkillInactiveTimer = false;
    private float sandcastleTimer = 0f;
    private const float sandcastleTriggerInterval = 5f; 
    
    private void Awake()
    {
        PlaySystemRefStorage.engineEffectTriggerManager = this;
    }

    private void Start()
    {
        var skillID = BoomBoomEngineDatabase.GetEngineData
            (GameManager.Instance.SelectedEngineID)?.SkillID ?? -1;

        if (skillID == 305 && shieldEffect != null)
        {
            Debug.Log("[EngineTrigger] 305번 엔진 감지됨 → 실드 생성 타이머 시작");
            shieldEffect.ExecuteEffect();
            
        }
        // 마술모자(317) 엔진이면 반드시 타이머 시작
        if (skillID == 317)
        {
            Debug.Log("[EngineTrigger] 317번(마술모자) 엔진 감지됨 → 비활성화 타이머 시작");
            StartSkillInactiveTimer();
        }
        
        UpdateOrbitAuraState();
    }
    
    void Update()
    {
        int selectedSkillID = BoomBoomEngineDatabase.GetEngineData
            (GameManager.Instance.SelectedEngineID)?.SkillID ?? -1;
        if (bSkillInactiveTimer)
        {
            
            inactiveSkillTime += Time.deltaTime;

            if (inactiveSkillTime >= 7f)
            {
                Debug.Log("[TriggerManager] 마술모자 효과 발동 시도");
                if (selectedSkillID == 317)
                {
                    Debug.Log("[TriggerManager] TryActivateSkill 호출");
                    TryActivateSkill(PlaySystemRefStorage.playerStatManager);
                }

                bSkillInactiveTimer = false;
                inactiveSkillTime = 0f;
            }
            
            if (selectedSkillID == 323)
            {
                sandcastleTimer += Time.deltaTime;
                if (sandcastleTimer >= sandcastleTriggerInterval)
                {
                    var sandCastleEffect = PlaySystemRefStorage.playerStatManager.GetComponent<AuraRangeSandCastleEffect>();
                    if (sandCastleEffect != null)
                        sandCastleEffect.ExecuteEffect();

                    sandcastleTimer = 0f;
                }
            }
            
        }
    }
    
    // 추가로, 향후 피격 외 조건에 따라 실드 재생성 트리거 가능
    public void TryForceShield()
    {
        if (shieldEffect != null)
        {
            shieldEffect.ExecuteEffect();
        }
    }

    public void UpdateOrbitAuraState()
    {
        var engineData = BoomBoomEngineDatabase.GetEngineData(GameManager.Instance.SelectedEngineID);

        if (orbitAura != null)
        {
            if (engineData != null && engineData.SkillID == 318)
            {
                orbitAura.gameObject.SetActive(true);
                orbitAura.SetTarget(PlaySystemRefStorage.playerStatManager.transform, 4.0f, 0.0f);
            }
            else
            {
                orbitAura.gameObject.SetActive(false);
            }
        }
    }
    
    
    public void OnOrangeFlowerCollected()
    {
        orangeFlowerCount++;

        int selectedSkillID = BoomBoomEngineDatabase.GetEngineData(GameManager.Instance.SelectedEngineID)?.SkillID ?? -1;

        if (selectedSkillID == 303 && orangeFlowerCount >= 5)
        {
            TryActivateSkill(PlaySystemRefStorage.playerStatManager);
            orangeFlowerCount = 0;
        }
    }

    public void OnYellowFlowerCollected()
    {
        yellowFlowerCount++;

        int selectedSkillID = BoomBoomEngineDatabase.GetEngineData(GameManager.Instance.SelectedEngineID)?.SkillID ?? -1;

        if (selectedSkillID == 304 && yellowFlowerCount >= 5)
        {
            TryActivateSkill(PlaySystemRefStorage.playerStatManager);
            yellowFlowerCount = 0;
        }
    }

    public void StartSkillInactiveTimer()
    {
        if (!bSkillInactiveTimer)
        {
            bSkillInactiveTimer = true;
            inactiveSkillTime = 0f;
        }
    }

    public void OnTakeDamage()
    {
        int selectedSkillID = BoomBoomEngineDatabase.GetEngineData(GameManager.Instance.SelectedEngineID)?.SkillID ?? -1;

        // 예: 301 리비 엔진 (무적)만 피격으로 발동
        if (selectedSkillID == 301)
        {
            TryActivateSkill(PlaySystemRefStorage.playerStatManager);
        }
        
        // 모래성(323) 엔진 버프 초기화
        if (selectedSkillID == 323)
        {
            var sandCastleEffect = PlaySystemRefStorage.playerStatManager.GetComponent<AuraRangeSandCastleEffect>();
            if (sandCastleEffect != null)
                sandCastleEffect.ResetBuff();
        }
        
    }

    // 플레이어 사망 시 TryReviveOnDeath로 위임 호출
    public bool TryReviveOnDeath(PlayerStatManager player)
    {
        int skillID = BoomBoomEngineDatabase.GetEngineData(GameManager.Instance.SelectedEngineID)?.SkillID ?? -1;
        if (skillID == 313 && player != null)
        {
            var reviveEffect = player.GetComponent<DogHouseReviveEffect>();
            if (reviveEffect != null && !reviveEffect.IsReviveUsed)
                return reviveEffect.TryRevive(player);
        }
        return false;
    }
    
    // 필요시 ExecuteEffect 리셋도 아래처럼!
    public void ResetReviveEffect(PlayerStatManager player)
    {
        var reviveEffect = player.GetComponent<DogHouseReviveEffect>();
        if (reviveEffect != null)
            reviveEffect.ExecuteEffect();
    }
    
    /// <summary>
    /// 현재 선택된 엔진 스킬 발동
    /// </summary>
    private void TryActivateSkill(PlayerStatManager target)
    {
        int selectedEngineID = GameManager.Instance.SelectedEngineID;
        var engineData = BoomBoomEngineDatabase.GetEngineData(selectedEngineID);

        if (engineData != null && effectController != null)
        {
            effectController.ActivateEffect(engineData.SkillID, target); //  정상
        }
    }
}
