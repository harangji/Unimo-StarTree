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

    // HP 매니저 추가.
    [SerializeField] private HPManager hpManager;

    private bool isInvincible = false;
    private Coroutine stunCoroutine;

    [SerializeField] private bool isTestModel = false;
    [SerializeField] private GameObject equipPrefab;
    [SerializeField] private GameObject chaPrefab;

    // 임시로 작성.정현식
    // 회피, 스턴 저항 (스탯 반영 예정)
    [SerializeField] [Range(0f, 1f)] private float bEvadeChance = 0.5f;
    [SerializeField] [Range(0f, 1f)] private float fStunReduceRate = 0.5f;

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


        InitCharacter(unimoID);
        
        //최대 체력으로 hp 초기화
        currentHP = mStat.BaseStat.Health;
    }

    // 스탯 추가.정현식
    public void InitCharacter(int id)
    {
        Debug.Log($"[PlayerStatManager] InitCharacter 호출됨: ID = {id}");
        mUnimoData = UnimoDatabase.GetUnimoData(id);
        Debug.Log($"[PlayerStatManager] 불러온 체력 = {mUnimoData.Stat.Health}");
        if (mUnimoData == null)
        {
            Debug.LogError($"[PlayerStatManager] 잘못된 Unimo ID: {id}");
            return;
        }

        mStat = new UnimoRuntimeStat(mUnimoData.Stat);
        playerMover.SetCharacterStat(mStat);
        hpManager.SetCharacterStat(mStat);
        auraController.InitAura(mStat.FinalStat.AuraRange, mStat.FinalStat.AuraStr);
        PlaySystemRefStorage.scoreManager.ApplyStatFromCharacter(mStat);

        bEvadeChance = mStat.FinalStat.StunIgnoreChance;
        fStunReduceRate = mStat.FinalStat.StunResistanceRate;
        Debug.Log($"[PlayerStatManager] 회피확률: {bEvadeChance * 100}% / 스턴저항률: {fStunReduceRate * 100}%");
    }

    public UnimoRuntimeStat GetStat()
    {
        return mStat;
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

    // Hit 수정 버전. 정현식
    public void Hit(float stun, Vector3 hitPos)
    {
        if (isInvincible) return;
    
        // 회피 판정
        if (Random.value < bEvadeChance)
        {
            Debug.Log("피격 회피: 넉백/스턴/오라 감소 없음, 데미지만 적용");
    
            // 넉백, 스턴, 오라 감소 없이 데미지만 적용
            hpManager.TakeDamage(20f);
            return;
        }
    
        // 회피 실패 → 스턴 및 전체 피격 처리
        stun = Mathf.Max(stun * (1f - fStunReduceRate), 0.2f);
        hitPos.y = 0;
    
        stunCoroutine = StartCoroutine(StunCoroutine(stun, hitPos));
        PlaySystemRefStorage.harvestLvController.LossExp(stun);
        hpManager.TakeDamage(20f);
    }
    //원본 Hit 코드. 정현식
    //public void Hit(float stun, Vector3 hitPos)
    //{
    //    stun = Mathf.Max(stun, 0.2f);
    //    
    //    if (isInvincible) { return; }
    //    else
    //    {
    //        hitPos.y = 0;
    //        stunCoroutine = StartCoroutine(StunCoroutine(stun, hitPos));
    //        PlaySystemRefStorage.harvestLvController.LossExp(stun);
    //    }
    //}

    // 오라 작아짐 + 스턴 애니메이션 + 무적 판정 + 데미지 적용
    //임시로 만듦. 컴벳 시스템의 피격 메서드를 대체하고 있음.
    public void Hit(float stun, Vector3 hitPos, int damage)
    {
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

        //스턴 로직
        hitPos.y = 0;
        stunCoroutine = StartCoroutine(StunCoroutine(stun, hitPos));

        PlaySystemRefStorage.harvestLvController.LossExp(stun);

        // HPManager를 통해 데미지 처리
        hpManager.TakeDamage(20f);
    }

    private void stopPlay()
    {
        if (isInvincible)
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
        hpGauge.SetGauge(currentHP / mStat.BaseStat.Health);
        
        //사망 체크
        if (currentHP <= 0)
        {
            PlaySystemRefStorage.playProcessController.TimeUp();
        }
    }

    public void TakeHeal(HealEvent combatEvent)
    {
        throw new System.NotImplementedException();
    }
}