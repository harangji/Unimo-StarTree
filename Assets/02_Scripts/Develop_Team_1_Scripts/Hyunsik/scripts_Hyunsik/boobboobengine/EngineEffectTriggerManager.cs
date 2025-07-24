using UnityEngine;

public class EngineEffectTriggerManager : MonoBehaviour
{
    [SerializeField] private BoomBoomEngineEffectController effectController;
    [SerializeField] private ShieldEffect shieldEffect;
    
    private int orangeFlowerCount = 0;
    private int yellowFlowerCount = 0;
    private float inactiveSkillTime = 0f;
    private bool bSkillInactiveTimer = false;

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
                    TryActivateSkill();
                }

                bSkillInactiveTimer = false;
                inactiveSkillTime = 0f;
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

    
    public void OnOrangeFlowerCollected()
    {
        orangeFlowerCount++;

        int selectedSkillID = BoomBoomEngineDatabase.GetEngineData(GameManager.Instance.SelectedEngineID)?.SkillID ?? -1;

        if (selectedSkillID == 303 && orangeFlowerCount >= 5)
        {
            TryActivateSkill();
            orangeFlowerCount = 0;
        }
    }

    public void OnYellowFlowerCollected()
    {
        yellowFlowerCount++;

        int selectedSkillID = BoomBoomEngineDatabase.GetEngineData(GameManager.Instance.SelectedEngineID)?.SkillID ?? -1;

        if (selectedSkillID == 304 && yellowFlowerCount >= 5)
        {
            TryActivateSkill();
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
            TryActivateSkill();
        }
    }

    /// <summary>
    /// 현재 선택된 엔진 스킬 발동
    /// </summary>
    private void TryActivateSkill()
    {
        int selectedEngineID = GameManager.Instance.SelectedEngineID;
        var engineData = BoomBoomEngineDatabase.GetEngineData(selectedEngineID);

        if (engineData != null)
        {
            if (effectController != null)
            {
                effectController.ActivateEffect(engineData.SkillID);
                Debug.Log($"[EngineEffectTriggerManager] 발동됨 ▶ EngineID {selectedEngineID}, SkillID {engineData.SkillID}");
            }
            else
            {
                Debug.LogError("[EngineEffectTriggerManager] EffectController가 연결되지 않았습니다.");
            }
        }
        else
        {
            Debug.LogWarning($"[EngineEffectTriggerManager] EngineID {selectedEngineID}에 대한 데이터가 없습니다.");
        }
    }
}
