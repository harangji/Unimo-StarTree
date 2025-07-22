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
                TryActivateSkill();   // ���õ� �������� �ߵ�
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
            TryActivateSkill();  // ���õ� �������� �ߵ�
            orangeFlowerCount = 0;
        }
    }

    public void OnYellowFlowerCollected()
    {
        yellowFlowerCount++;
        if (yellowFlowerCount >= 5)
        {
            TryActivateSkill();  // ���õ� �������� �ߵ�
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
        TryActivateSkill();  // ���õ� �������� �ߵ�
    }

    /// <summary>
    /// ���� ���õ� ���� ��ų �ߵ�
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
                Debug.Log($"[EngineEffectTriggerManager] �ߵ��� �� EngineID {selectedEngineID}, SkillID {engineData.SkillID}");
            }
            else
            {
                Debug.LogError("[EngineEffectTriggerManager] EffectController�� ������� �ʾҽ��ϴ�.");
            }
        }
        else
        {
            Debug.LogWarning($"[EngineEffectTriggerManager] EngineID {selectedEngineID}�� ���� �����Ͱ� �����ϴ�.");
        }
    }
}
