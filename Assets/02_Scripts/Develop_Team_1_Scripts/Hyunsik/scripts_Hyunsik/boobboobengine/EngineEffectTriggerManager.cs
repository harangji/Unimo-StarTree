using UnityEngine;
using System.Collections;

public class EngineEffectTriggerManager : MonoBehaviour
{
    [SerializeField] private BoomBoomEngineEffectController effectController;
    [SerializeField] private ShieldEffect shieldEffect;
    [SerializeField] private OrbitAuraController orbitAura;
    [SerializeField] private BoundaryChaser cloudAura;
    [SerializeField] private BoundaryChaser lightningAura;

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
        var skillID = BoomBoomEngineDatabase.GetEngineData(GameManager.Instance.SelectedEngineID)?.SkillID ?? -1;

        if (skillID == 305 && shieldEffect != null)
        {
            int level = EngineLevelSystem.GetUniqueLevel(GameManager.Instance.SelectedEngineID);
            shieldEffect.Init(GameManager.Instance.SelectedEngineID, level);
            shieldEffect.ExecuteEffect();
            Debug.Log("[EngineTrigger] 305번 엔진 감지됨 → 실드 생성 타이머 시작");
        }

        if (skillID == 317)
        {
            Debug.Log("[EngineTrigger] 317번(마술모자) 엔진 감지됨 → 비활성화 타이머 시작");
            StartSkillInactiveTimer();
        }

        //if (skillID == 323)
        //{
        //    Debug.Log("[EngineTrigger] 323번(모래성) 엔진 감지됨 → 비활성화 타이머 시작");
        //    StartSkillInactiveTimer();
        //}

        UpdateOrbitAuraState();
        
        BoomBoomEngineData data = BoomBoomEngineDatabase.GetEngineData(GameManager.Instance.SelectedEngineID);
        selectedSkillID = (data != null) ? data.SkillID : -1;
        
        if (data?.TriggerCondition == ETriggerCondition.OnStart)
        {
            Debug.Log("[EngineTrigger] TriggerCondition == OnStart 감지됨 → TryActivateSkill 호출");
            TryActivateSkill(PlaySystemRefStorage.playerStatManager);
        }
    }

    private int selectedSkillID;
    void Update()
    {
        // int selectedSkillID = BoomBoomEngineDatabase.GetEngineData(GameManager.Instance.SelectedEngineID)?.SkillID ?? -1;
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
                    //if (sandCastleEffect != null && !sandCastleEffect.IsActive())
                    //{
                    //    int level = EngineLevelSystem.GetUniqueLevel(GameManager.Instance.SelectedEngineID);
                    //    StartCoroutine(WaitForStatThenActivate(PlaySystemRefStorage.playerStatManager, sandCastleEffect, GameManager.Instance.SelectedEngineID, level));
                    //}

                    sandcastleTimer = 0f;
                }
            }
        }
    }

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
            bool isActive = engineData != null && engineData.SkillID == 318;
            orbitAura.gameObject.SetActive(isActive);
            if (isActive)
                orbitAura.SetTarget(PlaySystemRefStorage.playerStatManager.transform, 4.0f, 0.0f);
        }

        if (cloudAura != null)
        {
            bool isActive = engineData != null && engineData.SkillID == 319;
            cloudAura.gameObject.SetActive(isActive);
        }
    }

    public void OnOrangeFlowerCollected()
    {
        orangeFlowerCount++;
        var data = BoomBoomEngineDatabase.GetEngineData(GameManager.Instance.SelectedEngineID);

        if (data?.TriggerCondition == ETriggerCondition.OnOrangeFlowerCollected &&
            orangeFlowerCount >= 10)
        {
            TryActivateSkill(PlaySystemRefStorage.playerStatManager);
            orangeFlowerCount = 0;
        }
    }

    public void OnYellowFlowerCollected()
    {
        yellowFlowerCount++;
        var data = BoomBoomEngineDatabase.GetEngineData(GameManager.Instance.SelectedEngineID);

        if (data?.TriggerCondition == ETriggerCondition.OnYellowFlowerCollected)
        {
            int level = EngineLevelSystem.GetUniqueLevel(data.EngineID);
            int requiredCount = Mathf.RoundToInt(70f - 50f * (data.GrowthTable[level]));

            if (yellowFlowerCount >= requiredCount)
            {
                TryActivateSkill(PlaySystemRefStorage.playerStatManager);
                yellowFlowerCount = 0;
            }
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
        var data = BoomBoomEngineDatabase.GetEngineData(GameManager.Instance.SelectedEngineID);
        if (data?.TriggerCondition == ETriggerCondition.OnTakeDamage)
        {
            TryActivateSkill(PlaySystemRefStorage.playerStatManager);
        }

        if (data?.SkillID == 323)
        {
            var sandCastle = PlaySystemRefStorage.playerStatManager.GetComponent<AuraRangeSandCastleEffect>();
            sandCastle?.ResetBuff();
        }
    }

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

    public void ResetReviveEffect(PlayerStatManager player)
    {
        var reviveEffect = player.GetComponent<DogHouseReviveEffect>();
        if (reviveEffect != null)
            reviveEffect.ExecuteEffect();
    }

    private void TryActivateSkill(PlayerStatManager target)
    {
        int selectedEngineID = GameManager.Instance.SelectedEngineID;
        var engineData = BoomBoomEngineDatabase.GetEngineData(selectedEngineID);

        if (engineData != null && effectController != null)
        {
            var effect = target.GetComponent<IBoomBoomEngineEffect>();
            int level = EngineLevelSystem.GetUniqueLevel(engineData.EngineID);

            if (effect is AuraRangeSandCastleEffect sandCastle)
            {
                StartCoroutine(WaitForStatThenActivate(target, sandCastle, engineData.EngineID, level));
                return;
            }
            else if (effect is AuraRangeBoostEffect auraBoost)
            {
                auraBoost.Init(engineData.EngineID, level);
                auraBoost.ExecuteEffect();
                return;
            }
            else if (effect is BeeTailInvincibilityEffect beeTail)
            {
                beeTail.Init(engineData.EngineID, level);
                beeTail.ExecuteEffect();
                return;
            }
            else if (effect is ShieldEffect shield)
            {
                shield.Init(engineData.EngineID, level);
                shield.ExecuteEffect();
                return;
            }
            else if (effect is TimedInvincibilityEffect timedInvincibility)
            {
                timedInvincibility.Init(engineData.EngineID, level);
                timedInvincibility.ExecuteEffect();
                return;
            }
            else if (effect is MagicHatEffect magicHat)
            {
                magicHat.Init(engineData.EngineID, level);
                magicHat.ExecuteEffect();
                return;
            }

            effectController.ActivateEffect(engineData.SkillID, target);
        }
    }

    private IEnumerator WaitForStatThenActivate(PlayerStatManager target, AuraRangeSandCastleEffect sandCastle, int engineID, int level)
    {
        yield return new WaitUntil(() => target != null && target.GetStat() != null);

        //sandCastle.Init(engineID, level);
        sandCastle.ExecuteEffect();
    }
}
