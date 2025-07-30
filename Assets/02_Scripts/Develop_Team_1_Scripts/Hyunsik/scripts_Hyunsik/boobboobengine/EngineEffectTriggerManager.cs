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
            Debug.Log("[EngineTrigger] 305�� ���� ������ �� �ǵ� ���� Ÿ�̸� ����");
            shieldEffect.ExecuteEffect();
            
        }
        // ��������(317) �����̸� �ݵ�� Ÿ�̸� ����
        if (skillID == 317)
        {
            Debug.Log("[EngineTrigger] 317��(��������) ���� ������ �� ��Ȱ��ȭ Ÿ�̸� ����");
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
                Debug.Log("[TriggerManager] �������� ȿ�� �ߵ� �õ�");
                if (selectedSkillID == 317)
                {
                    Debug.Log("[TriggerManager] TryActivateSkill ȣ��");
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
    
    // �߰���, ���� �ǰ� �� ���ǿ� ���� �ǵ� ����� Ʈ���� ����
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

        // ��: 301 ���� ���� (����)�� �ǰ����� �ߵ�
        if (selectedSkillID == 301)
        {
            TryActivateSkill(PlaySystemRefStorage.playerStatManager);
        }
        
        // �𷡼�(323) ���� ���� �ʱ�ȭ
        if (selectedSkillID == 323)
        {
            var sandCastleEffect = PlaySystemRefStorage.playerStatManager.GetComponent<AuraRangeSandCastleEffect>();
            if (sandCastleEffect != null)
                sandCastleEffect.ResetBuff();
        }
        
    }

    // �÷��̾� ��� �� TryReviveOnDeath�� ���� ȣ��
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
    
    // �ʿ�� ExecuteEffect ���µ� �Ʒ�ó��!
    public void ResetReviveEffect(PlayerStatManager player)
    {
        var reviveEffect = player.GetComponent<DogHouseReviveEffect>();
        if (reviveEffect != null)
            reviveEffect.ExecuteEffect();
    }
    
    /// <summary>
    /// ���� ���õ� ���� ��ų �ߵ�
    /// </summary>
    private void TryActivateSkill(PlayerStatManager target)
    {
        int selectedEngineID = GameManager.Instance.SelectedEngineID;
        var engineData = BoomBoomEngineDatabase.GetEngineData(selectedEngineID);

        if (engineData != null && effectController != null)
        {
            effectController.ActivateEffect(engineData.SkillID, target); //  ����
        }
    }
}
