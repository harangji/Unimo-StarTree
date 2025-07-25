using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatManager : MonoBehaviour, IDamageAble
{
    private static readonly float invincibleTime = 0.5f;
    
    [SerializeField][Range(0.01f,1f)] private float revivePercent = 0.05f; // 30% (인스펙터에서 조절)
    private bool bReviveUsed = false; // 1회 한정 부활이라면
    
    // 스탯 추가. 정현식
    [Header("유닛 ID로 선택")] [SerializeField] private int unimoID = 10101;
    private UnimoData mUnimoData;
    private UnimoRuntimeStat mStat;

    //private PlayerHPspriter HPspriter;
    private PlayerMover playerMover;
    private AuraController auraController;
    private PlayerVisualController visualCtrl;

    private GameObject renderCam;

    private bool isInvincible = false;
    private Coroutine stunCoroutine;

    [SerializeField] private bool isTestModel = false;
    [SerializeField] private GameObject equipPrefab;
    [SerializeField] private GameObject chaPrefab;

    // 임시로 작성.정현식
    // 회피, 스턴 저항 (스탯 반영 예정)
    [SerializeField] [Range(0f, 1f)] private float bEvadeChance = 0.5f;
    [SerializeField] [Range(0f, 1f)] private float fStunReduceRate = 0.5f;

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
    
    private void Awake()
    {
        PlaySystemRefStorage.playerStatManager = this;
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

        PlaySystemRefStorage.playProcessController.SubscribeGameoverAction(stopPlay);

        //hpGauge?.SetGauge(1f);

        int selectedID = GameManager.Instance.SelectedUnimoID > 0
            ? GameManager.Instance.SelectedUnimoID
            : unimoID;

        InitCharacter(selectedID);

        //최대 체력으로 hp 초기화
        currentHP = mStat.BaseStat.Health;
        hpGauge?.SetGauge(1f);
    }

    // 스탯 추가.정현식
    public void InitCharacter(int id)
    {
        Debug.Log($"[PlayerStatManager] InitCharacter 호출됨: ID = {id}");
        mUnimoData = UnimoDatabase.GetUnimoData(id);

        if (mUnimoData == null)
        {
            Debug.LogError($"[PlayerStatManager] 잘못된 Unimo ID: {id}");
            return;
        }

        //  기본 스탯 가져오기
        var baseStat = mUnimoData.Stat;

        //  붕붕엔진 스탯 적용
        var engineData = BoomBoomEngineDatabase.GetEngineData(GameManager.Instance.SelectedEngineID);

        if (engineData != null)
        {
            var engineStat = engineData.StatBonus;

            baseStat.MoveSpd += engineStat.MoveSpd;
            baseStat.Health += engineStat.Health;
            baseStat.Armor += engineStat.Armor;
            baseStat.AuraRange += engineStat.AuraRange;
            baseStat.AuraStr += engineStat.AuraStr;
            baseStat.CriticalChance += engineStat.CriticalChance;
            baseStat.CriticalMult += engineStat.CriticalMult;
            baseStat.HealingMult += engineStat.HealingMult;
            baseStat.HealthRegen += engineStat.HealthRegen;
            baseStat.YFGainMult += engineStat.YFGainMult;
            baseStat.OFGainMult += engineStat.OFGainMult;

            Debug.Log($"[PlayerStatManager] 붕붕엔진 스탯 적용됨: {engineData.Name}");
            Debug.Log($"[붕붕엔진 스탯 증가]" +
                      $"\n▶ 이동속도: +{engineStat.MoveSpd}" +
                      $"\n▶ 체력: +{engineStat.Health}" +
                      $"\n▶ 방어력: +{engineStat.Armor}" +
                      $"\n▶ 오라 범위: +{engineStat.AuraRange}" +
                      $"\n▶ 오라 강도: +{engineStat.AuraStr}" +
                      $"\n▶ 크리티컬 확률: +{engineStat.CriticalChance}" +
                      $"\n▶ 크리티컬 배율: +{engineStat.CriticalMult}" +
                      $"\n▶ 회복 배수: +{engineStat.HealingMult}" +
                      $"\n▶ 자연 회복: +{engineStat.HealthRegen}" +
                      $"\n▶ 노란별꽃 배수(YF): +{engineStat.YFGainMult}" +
                      $"\n▶ 주황별꽃 배수(OF): +{engineStat.OFGainMult}");
        }

        //  최종 스탯으로 저장
        mStat = new UnimoRuntimeStat(baseStat);

        playerMover.SetCharacterStat(mStat);
        auraController.InitAura(mStat.FinalStat.AuraRange, mStat.FinalStat.AuraStr);
        PlaySystemRefStorage.scoreManager.ApplyStatFromCharacter(mStat);

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
        if (currentHP > 0f && currentHP < mStat.BaseStat.Health)
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
    }

    public void DeactivePlayer()
    {
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
            PlaySystemRefStorage.engineEffectController.ActivateEffect(engineData.SkillID);
            Debug.Log($"[PlayerStatManager] 스턴 해제 후 엔진 스킬 발동 ▶ SkillID : {engineData.SkillID}");
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

        if (IsInvincible()) // 외부 무적 포함해서 처리
        {
            return;
        }

        // 실드가 있다면 피해 무효화 + 리턴
        if (mShieldEffect != null && mShieldEffect.TryConsumeShield())
        {
            Debug.Log("[PlayerStatManager] 실드로 인해 피해 무효화됨");
            return;
        }
        
        //  피격 회피 판정
        if (Random.value < bEvadeChance)
        {
            Debug.Log("피격 회피");
            return;
        }

        if (!combatEvent.CanBeStunned)
        {
            //  스턴 시간 감소 적용
            stun = Mathf.Max(stun * (1f - fStunReduceRate), 0.2f);

            //스턴 처리 로직
            hitPos.y = 0;
            // stunCoroutine = combatEvent.IsStrongKnockback
            //     ? StartCoroutine(StunCoroutine(stun, hitPos, 3))
            //     : StartCoroutine(StunCoroutine(stun, hitPos));

            if (combatEvent.IsStrongKnockback)
            {
                Debug.Log("강한 넉백!");
                StartCoroutine(StunCoroutine(stun, hitPos, 3));
            }
            else
            {
                Debug.Log("일반 넉백");
                StartCoroutine(StunCoroutine(stun, hitPos));
            }
        }

        //데미지 처리
        var reducedDamage = combatEvent.Damage * (1f - mStat.BaseStat.Armor);
        currentHP -= reducedDamage;

        hpGauge?.SetGauge(currentHP / mStat.BaseStat.Health);

        Debug.Log($"[Combat System] 피해량: {reducedDamage}");
        Debug.Log($"[Combat System] 현재 체력: {currentHP} / {mStat.BaseStat.Health}");
        Debug.Log($"[Combat System] 비율: {currentHP / mStat.BaseStat.Health}");

        PlaySystemRefStorage.engineEffectTriggerManager.OnTakeDamage();

        //사망 체크
        if (currentHP <= 0)
        {
            int skillID = BoomBoomEngineDatabase.GetEngineData(GameManager.Instance.SelectedEngineID)?.SkillID ?? -1;
            if (skillID == 313 && !bReviveUsed)
            {
                float maxHp = mStat.BaseStat.Health;
                float reviveHp = Mathf.Max(1f, maxHp * revivePercent);
                currentHP = reviveHp;
                hpGauge?.SetGauge(currentHP / maxHp);
                bReviveUsed = true; // 1회 부활만 허용하려면
                Debug.Log($"[도베르만 엔진] 부활! 체력 {currentHP}/{maxHp} ({revivePercent * 100}%)");
                // 부활 이펙트/애니메이션/사운드 추가 가능
                return; // 사망 처리 막음
            }

            // 일반 사망 처리
            if (PlaySystemRefStorage.stageManager.GetBonusStage()) { return; }
            PlaySystemRefStorage.playProcessController.GameOver();
        }
        
    }

    // 강제 체력 회복용 메서드 추가
    public void ForceRevive(float reviveHp)
    {
        currentHP = reviveHp;
        hpGauge?.SetGauge(currentHP / mStat.BaseStat.Health);
        // 추가 이펙트, 무적 등 연출
    }
    
    public void TakeHeal(HealEvent healEvent)
    {
        currentHP = Mathf.Min(currentHP + healEvent.Heal, mStat.BaseStat.Health);

        hpGauge?.SetGauge(currentHP / mStat.BaseStat.Health);
    }
}