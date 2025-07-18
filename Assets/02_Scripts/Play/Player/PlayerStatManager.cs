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

        hpGauge?.SetGauge(1f);

        int selectedID = GameManager.Instance.SelectedUnimoID > 0
            ? GameManager.Instance.SelectedUnimoID
            : unimoID;  
        
        InitCharacter(selectedID);
        
        //최대 체력으로 hp 초기화
        currentHP = mStat.BaseStat.Health;
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

        // 🔽 기본 스탯 가져오기
        var baseStat = mUnimoData.Stat;

        // 🔽 붕붕엔진 스탯 적용
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
        }

        // 🔽 최종 스탯으로 저장
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

    void Update()
    {
        if (currentHP > 0f && currentHP < mStat.BaseStat.Health)
        {
            var regenHeal = regenAmountPerSecond * Time.deltaTime * healingMultiplier;
            var healEvent = new HealEvent { Heal = regenHeal };

            TakeHeal(healEvent);
        }
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

    private IEnumerator StunCoroutine(float duration, Vector3 hitPos)
    {
        playerMover.IsStop = true;
        auraController.Shrink();
        isInvincible = true;
        visualCtrl.StartHitBlink(duration);
        visualCtrl.SetHitFX(hitPos, duration);

        yield return null;

        playerMover.StunPush(duration, hitPos);
        visualCtrl.SetMovingAnim(false);
        visualCtrl.SetStunAnim(true);

        yield return new WaitForSeconds(0.8f * duration);
        playerMover.IsStop = false;

        yield return new WaitForSeconds(0.2f * duration);
        auraController.Resume();
        visualCtrl.SetStunAnim(false);

        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
        yield break;
    }

    public void TakeDamage(CombatEvent combatEvent)
    {
        var stun = 0.8f;
        var hitPos = combatEvent.HitPosition;
        
        //피격 무적
        if (isInvincible)
        {
            return;
        }

        //  피격 회피 판정
        if (Random.value < bEvadeChance)
        {
            Debug.Log("피격 회피");
            return;
        }

        //  스턴 시간 감소 적용
        stun = Mathf.Max(stun * (1f - fStunReduceRate), 0.2f);
        
        //스턴 처리 로직
        hitPos.y = 0;
        stunCoroutine = StartCoroutine(StunCoroutine(stun, hitPos));
        
        //데미지 처리
        var reducedDamage = combatEvent.Damage * (1f - mStat.BaseStat.Armor);
        currentHP -= reducedDamage;
        
        hpGauge?.SetGauge(currentHP / mStat.BaseStat.Health);
        
        Debug.Log($"Combat System: 피해량: {reducedDamage}");
        Debug.Log($"Combat System: 현재 체력: {currentHP} / {mStat.BaseStat.Health}");
        Debug.Log($"Combat System: 비율: {currentHP / mStat.BaseStat.Health}");
        
        //사망 체크
        if (currentHP <= 0)
        {
            if (PlaySystemRefStorage.stageManager.GetBonusStage()) { return; }
            PlaySystemRefStorage.playProcessController.GameOver();
        }
    }

    public void TakeHeal(HealEvent healEvent)
    {
        currentHP = Mathf.Min(currentHP + healEvent.Heal, mStat.BaseStat.Health);
        
        hpGauge?.SetGauge(currentHP / mStat.BaseStat.Health);
    }
}