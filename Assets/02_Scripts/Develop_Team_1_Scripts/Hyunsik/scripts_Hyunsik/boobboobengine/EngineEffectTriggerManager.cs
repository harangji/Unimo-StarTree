using UnityEngine;

public class EngineEffectTriggerManager : MonoBehaviour
{
    [SerializeField] private BoomBoomEngineEffectController effectController;

    private int orangeFlowerCount = 0;
    private int yellowFlowerCount = 0;
    private float inactiveSkillTime = 0f;
    private bool bSkillInactiveTimer = false;

    private void Awake()
    {
        PlaySystemRefStorage.engineEffectTriggerManager = this;
    }

    private void Update()
    {
        if (bSkillInactiveTimer)
        {
            inactiveSkillTime += Time.deltaTime;

            if (inactiveSkillTime >= 8f)
            {
                TryActivateSkill();   // 선택된 엔진으로 발동
                bSkillInactiveTimer = false;
                inactiveSkillTime = 0f;
            }
        }
    }

    public void OnOrangeFlowerCollected()
    {
        orangeFlowerCount++;
        if (orangeFlowerCount >= 2)
        {
            TryActivateSkill();  // 선택된 엔진으로 발동
            orangeFlowerCount = 0;
        }
    }

    public void OnYellowFlowerCollected()
    {
        yellowFlowerCount++;
        if (yellowFlowerCount >= 5)
        {
            TryActivateSkill();  // 선택된 엔진으로 발동
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
        TryActivateSkill();  // 선택된 엔진으로 발동
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
