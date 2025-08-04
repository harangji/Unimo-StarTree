using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatManager : MonoBehaviour, IDamageAble
{
    private static readonly float invincibleTime = 0.5f;
    
    // 스탯 추가. 정현식
    [Header("유닛 ID로 선택")] [SerializeField] private int unimoID = 10101;
    private UnimoData mUnimoData;
    private UnimoRuntimeStat mStat;

    
    private PlayerMover playerMover;
    private AuraController auraController;
    private PlayerVisualController visualCtrl;

    private GameObject renderCam;

    private bool isInvincible = false;
    private Coroutine stunCoroutine;

    [SerializeField] private bool isTestModel = false;
    [SerializeField] private GameObject equipPrefab;
    [SerializeField] private GameObject chaPrefab;
    
    // 회피, 스턴 저항 (스탯 반영 예정)
    [SerializeField] [Range(0f, 1f)] private float bEvadeChance;
    [SerializeField] [Range(0f, 1f)] private float fStunReduceRate;

    private float regenAmountPerSecond;  // 초당 자연 회복
    private float armor;                 // 방어력 (데미지 감소율)
    private float healingMultiplier;     // 회복 배수
    
    //컴벳 시스템에 사용하는 내용
    [SerializeField] private Collider mainCollider;
    public Collider MainCollider => mainCollider;
    public GameObject GameObject => gameObject;
    private float currentHP;
    [SerializeField] private HPGaugeController hpGauge;

    private bool bExternalInvincibility = false;
    
    private ShieldEffect mShieldEffect;
    private DogHouseReviveEffect mReviveEffect;
    public static event System.Action<bool> OnPlayerActiveChanged;
    
    
    private void Awake()
    {
        PlaySystemRefStorage.playerStatManager = this;
        
        mReviveEffect = GetComponent<DogHouseReviveEffect>();
        if (mReviveEffect == null)
        {
            Debug.LogWarning("[PlayerStatManager] DogHouseReviveEffect가 플레이어에 없습니다. 자동 추가.");
            mReviveEffect = gameObject.AddComponent<DogHouseReviveEffect>();
        }
        
        
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.position = -1000f * Vector3.one;
        renderCam = GameObject.Find("RenderCam");
        renderCam.SetActive(false);

        playerMover = GetComponent<PlayerMover>();
        visualCtrl = GetComponent<PlayerVisualController>();
        
        if (isTestModel)
        {
            visualCtrl.test_InitModeling(equipPrefab, chaPrefab);
            transform.position = Vector3.zero;
            FindAnyObjectByType<PlayTimeManager>().InitTimer();
            renderCam.SetActive(true);
            StartCoroutine(CoroutineExtensions.DelayedActionCall(() => { ActivePlayer(); },
                PlayProcessController.InitTimeSTATIC));
        }
        else
        {
            visualCtrl.InitModelingAsync(PlayProcessController.InitTimeSTATIC, () =>
            {
                transform.position = Vector3.zero;
                FindAnyObjectByType<PlayTimeManager>().InitTimer();
                renderCam.SetActive(true);
                StartCoroutine(CoroutineExtensions.DelayedActionCall(() => { ActivePlayer(); },
                    PlayProcessController.InitTimeSTATIC));
            });
        }

        auraController = FindAnyObjectByType<AuraController>();
        playerMover.FindAuraCtrl(auraController);
        auraController.gameObject.SetActive(false);

        var orbitAura = FindObjectOfType<OrbitAuraController>();
        if (orbitAura != null)
        {
            orbitAura.SetTarget(transform, 4.0f, 0.0f);
        }
        
        PlaySystemRefStorage.playProcessController.SubscribeGameoverAction(stopPlay);

        //hpGauge?.SetGauge(1f);

        int selectedID = GameManager.Instance.SelectedUnimoID > 0
            ? GameManager.Instance.SelectedUnimoID
            : unimoID;

        InitCharacter(selectedID);

        //최대 체력으로 hp 초기화
        currentHP = mStat.FinalStat.Health;
        hpGauge?.SetGauge(1f);
        
        int engineID = GameManager.Instance.SelectedEngineID;
        var engineData = BoomBoomEngineDatabase.GetEngineData(engineID);
        if (engineData?.SkillID == 313 && mReviveEffect != null)
        {
            int level = EngineLevelSystem.GetUniqueLevel(engineID);
            mReviveEffect.Init(engineID, level); // ✅ 레벨 기반 부활 비율 적용
            Debug.Log("[PlayerStatManager] 도베르만 Init 호출됨");
        }
        
    }
    
    public void InitCharacter(int id)
    {
        Debug.Log($"[PlayerStatManager] InitCharacter 호출됨: ID = {id}");
        mUnimoData = UnimoDatabase.GetUnimoData(id);

        if (mUnimoData == null)
        {
            Debug.LogError($"[PlayerStatManager] 잘못된 Unimo ID: {id}");
            return;
        }

        int engineID = GameManager.Instance.SelectedEngineID;

        //  기본 스탯 계산
        var baseStat = UnimoLevelSystem.ApplyLevelBonus(mUnimoData.Stat, mUnimoData.UnimoID);

        //  최종 스탯 생성 (기본)
        mStat = new UnimoRuntimeStat(baseStat);

        //  붕붕엔진 레벨 보정 추가
        var engineBonus = BoomBoomEngineDatabase.GetBonusStatWithLevel(engineID);
        mStat.AddBonus(engineBonus);

        //  디버그 출력
        Debug.Log($"[엔진 적용 디버그] 엔진 ID: {engineID}");
        Debug.Log($"[엔진 적용 디버그] YFGainMult: {engineBonus.YFGainMult}");

        //  최종 세팅
        playerMover.SetCharacterStat(mStat);
        auraController.InitAura(mStat.FinalStat.AuraRange, mStat.FinalStat.AuraStr);
        PlaySystemRefStorage.scoreManager.ApplyStatFromCharacter(mStat);

        playerMover.SetCharacterStat(mStat);
        auraController.InitAura(mStat.FinalStat.AuraRange, mStat.FinalStat.AuraStr);
        PlaySystemRefStorage.scoreManager.ApplyStatFromCharacter(mStat);

// 디버그 출력
        Debug.Log("========== [UnimoStat 디버그 출력] ==========");
        Debug.Log(mStat.ToDebugString("Base"));
        Debug.Log(mStat.ToDebugString("Bonus"));
        Debug.Log(mStat.ToDebugString("Final"));
        Debug.Log("===========================================");
        
        // 기타 값 세팅
        bEvadeChance = mStat.FinalStat.StunIgnoreChance;
        fStunReduceRate = mStat.FinalStat.StunResistanceRate;
        regenAmountPerSecond = mStat.FinalStat.HealthRegen;
        armor = mStat.FinalStat.Armor;
        healingMultiplier = mStat.FinalStat.HealingMult;

        Debug.Log($"[PlayerStatManager] 회피확률: {bEvadeChance * 100}% / 스턴저항률: {fStunReduceRate * 100}%");
        Debug.Log($"[PlayerStatManager] 방어력: {armor}, 자연회복: {regenAmountPerSecond}, 회복배수: {healingMultiplier}");
    }
    
    
    
    public UnimoRuntimeStat GetStat()
    {
        return mStat;
    }

    public void SetStat(UnimoRuntimeStat stat)
    {
        mStat = stat;

        Debug.Log($"[SetStat] 최종 이동속도:{mStat.FinalStat.MoveSpd}, 오라범위:{mStat.FinalStat.AuraRange}, 재생률:{mStat.FinalStat.HealthRegen}");
        
        // 주요 시스템에 스탯 재적용
        playerMover.SetCharacterStat(mStat);
        auraController.InitAura(mStat.FinalStat.AuraRange, mStat.FinalStat.AuraStr);
        PlaySystemRefStorage.scoreManager.ApplyStatFromCharacter(mStat);

        Debug.Log("[PlayerStatManager] 스탯 갱신 완료");
    }


    void Update()
    {
        if (currentHP > 0f && currentHP < mStat.FinalStat.Health)
        {
            var regenHeal = regenAmountPerSecond * Time.deltaTime * healingMultiplier;
            var healEvent = new HealEvent { Heal = regenHeal };

            TakeHeal(healEvent);
        }
        
    }

    public float GetCurrentHP()
    {
        return currentHP;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ItemController>(out var item))
        {
            item.UseItem();
        }
    }

    public void ActivePlayer()
    {
        auraController.gameObject.SetActive(true);
        playerMover.IsStop = false;
        GetComponent<Collider>().enabled = true;
        OnPlayerActiveChanged?.Invoke(true);
    }

    public void DeactivePlayer()
    {
        OnPlayerActiveChanged?.Invoke(false);
        auraController.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private void stopPlay()
    {
        if (isInvincible && !PlaySystemRefStorage.stageManager.GetBonusStage())
        {
            StopCoroutine(stunCoroutine);
        }

        isInvincible = true;
        auraController.gameObject.SetActive(false);
        visualCtrl.TriggerDisappear();
        playerMover.IsStop = true;
        GetComponent<Collider>().enabled = false;
        StartCoroutine(CoroutineExtensions.DelayedActionCall(() => { DeactivePlayer(); }, 4f));
    }

    public void RegisterShield(ShieldEffect shield)
    {
        mShieldEffect = shield;
    }
    
    private IEnumerator StunCoroutine(float duration, Vector3 hitPos, float pushPower = 1f)
    {
        playerMover.IsStop = true;
        auraController.Shrink();
        isInvincible = true;
        visualCtrl.StartHitBlink(duration);
        visualCtrl.SetHitFX(hitPos, duration);

        yield return null;

        playerMover.StunPush(duration, hitPos, pushPower);
        visualCtrl.SetMovingAnim(false);
        visualCtrl.SetStunAnim(true);

        yield return new WaitForSeconds(0.8f * duration);
        playerMover.IsStop = false;

        yield return new WaitForSeconds(0.2f * duration);
        auraController.Resume();
        visualCtrl.SetStunAnim(false);

        // 스턴 종료 직후 버프 발동 트리거
        TriggerEngineEffect_OnStunEnd();

        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
        yield break;
    }

    private void TriggerEngineEffect_OnStunEnd()
    {
        var engineData = BoomBoomEngineDatabase.GetEngineData(GameManager.Instance.SelectedEngineID);

        Debug.Log($"[디버그] engineEffectController Null? : {PlaySystemRefStorage.engineEffectController == null}");
        Debug.Log($"[디버그] engineData Null? : {engineData == null} (EngineID : {GameManager.Instance.SelectedEngineID})");

        if (engineData != null && PlaySystemRefStorage.engineEffectController != null)
        {
            //  조건 확인 후 실행
            if (engineData.TriggerCondition == ETriggerCondition.OnStunEnd)
            {
                PlaySystemRefStorage.engineEffectController.ActivateEffect(engineData.SkillID, this);
                Debug.Log($"[PlayerStatManager] 스턴 해제 후 엔진 스킬 발동 ▶ SkillID : {engineData.SkillID}");
            }
            else
            {
                Debug.Log($"[PlayerStatManager] 스턴 해제 ▶ 발동 조건 불일치로 무시됨 (TriggerCondition: {engineData.TriggerCondition})");
            }
        }
        else
        {
            Debug.LogWarning("[PlayerStatManager] 엔진 스킬 발동 실패: 컨트롤러 또는 엔진 데이터가 null");
        }
    }
    public void SetTemporaryInvincibility(bool isActive)
    {
        bExternalInvincibility = isActive;
    }

    public bool IsInvincible()
    {
        return isInvincible || bExternalInvincibility;
    }

    public AuraController GetAuraController()
    {
        return auraController;
    }

    public void TakeDamage(CombatEvent combatEvent)
    {
        var stun = 0.8f;
        var hitPos = combatEvent.HitPosition;
        
        if (IsInvincible())   // 외부 무적 포함해서 처리
        {
            return;
        }
        
        // 실드가 있다면 피해 무효화 + 리턴
        if (mShieldEffect != null && mShieldEffect.TryConsumeShield())
        {
            Debug.Log("[PlayerStatManager] 실드로 인해 피해 무효화됨");
            if(EditorMode.Instance.isShowDamage) DamageUIManager.instance.GetUI(0);
            return;
        }
        
        //  피격 회피 판정
        if (Random.value < bEvadeChance)
        {
            Debug.Log("피격 회피");
            if(EditorMode.Instance.isShowDamage) DamageUIManager.instance.GetUI(0);

            return;
        }

        if (!combatEvent.CanBeStunned)
        {
            //  스턴 시간 감소 적용
            stun = Mathf.Max(stun * (1f - fStunReduceRate), 0.2f);

            //스턴 처리 로직
            hitPos.y = 0;
            stunCoroutine = combatEvent.IsStrongKnockback
                ? StartCoroutine(StunCoroutine(stun, hitPos, 3))
                : StartCoroutine(StunCoroutine(stun, hitPos));
        }

        Debug.Log($"[보정 전 데미지]: {combatEvent.Damage}");
        
        PlaySystemRefStorage.playTimeManager.ChangeTimer(-combatEvent.TimeReduceAmount);
        Debug.Log($"{combatEvent.TimeReduceAmount}초 감소했습니다.");
        
        //데미지 처리
        var reducedDamage = combatEvent.Damage * (1f - mStat.FinalStat.Armor); 
        currentHP -= reducedDamage;

        if(EditorMode.Instance.isShowDamage) DamageUIManager.instance.GetUI(reducedDamage);
        hpGauge?.SetGauge(currentHP / mStat.FinalStat.Health); // 보정된 최대 체력 사용

        Debug.Log($"[Combat System] 피해량: {reducedDamage}");
        Debug.Log($"[Combat System] 현재 체력: {currentHP} / {mStat.BaseStat.Health}");
        Debug.Log($"[Combat System] 비율: {currentHP / mStat.BaseStat.Health}");

        PlaySystemRefStorage.engineEffectTriggerManager.OnTakeDamage();

        //사망 체크
        if (currentHP <= 0 && !PlaySystemRefStorage.stageManager.GetBonusStage())
        {
            if (PlaySystemRefStorage.engineEffectTriggerManager.TryReviveOnDeath(this))
                return;

            // 일반 사망 처리
            if (PlaySystemRefStorage.stageManager.GetBonusStage()) { return; }
            PlaySystemRefStorage.playProcessController.GameOver();
        }
        
    }
    
    public void SetAuraRange(float range)
    {
        var runtimeStat = mStat;
        var final = runtimeStat.FinalStat;
        final.AuraRange = range;
        runtimeStat.SetFinalStat(final);
        mStat = runtimeStat;

        auraController.InitAura(range, mStat.FinalStat.AuraStr);
        Debug.Log($"[PlayerStatManager] SetAuraRange 호출! 값: {range}");
    }
    
    // 강제 체력 회복용 메서드 추가
    public void ForceRevive(float reviveHp)
    {
        currentHP = reviveHp;
        hpGauge?.SetGauge(currentHP / mStat.FinalStat.Health);
        // 추가 이펙트, 무적 등 연출
    }
    
    public void TakeHeal(HealEvent healEvent)
    {
        currentHP = Mathf.Min(currentHP + healEvent.Heal, mStat.FinalStat.Health);
        hpGauge?.SetGauge(currentHP / mStat.FinalStat.Health);
    }
}
