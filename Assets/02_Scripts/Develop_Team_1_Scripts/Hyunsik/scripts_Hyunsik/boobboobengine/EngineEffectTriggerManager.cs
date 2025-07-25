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
            Debug.Log("[EngineTrigger] 305�� ���� ������ �� �ǵ� ���� Ÿ�̸� ����");
            shieldEffect.ExecuteEffect();
            
        }
        // ��������(317) �����̸� �ݵ�� Ÿ�̸� ����
        if (skillID == 317)
        {
            Debug.Log("[EngineTrigger] 317��(��������) ���� ������ �� ��Ȱ��ȭ Ÿ�̸� ����");
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
                Debug.Log("[TriggerManager] �������� ȿ�� �ߵ� �õ�");
                if (selectedSkillID == 317)
                {
                    Debug.Log("[TriggerManager] TryActivateSkill ȣ��");
                    TryActivateSkill();
                }

                bSkillInactiveTimer = false;
                inactiveSkillTime = 0f;
            }
        }
    }
    
    // �߰���, ���� �ǰ� �� ���ǿ� ���� �ǵ� ����� Ʈ���� ����
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

        // ��: 301 ���� ���� (����)�� �ǰ����� �ߵ�
        if (selectedSkillID == 301)
        {
            TryActivateSkill();
        }
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
