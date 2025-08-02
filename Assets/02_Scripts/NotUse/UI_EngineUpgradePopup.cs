using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_EngineUpgradePopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI engineNameText;
    [SerializeField] private Image engineImage;
    [SerializeField] private GameObject panel;
    [SerializeField] private Sprite[] engineSprites;
    [SerializeField] private TextMeshProUGUI levelText;         // ex. "1 / 50"
    [SerializeField] private TextMeshProUGUI descriptionText;  
    [SerializeField] private Button upgradeButton;

    private int mEngineID;
    private EngineLevelSystem.EEngineStatType mUpgradeStatType;
    private bool bIsUniqueType;

    
    // 2. UI_EngineUpgradePopup.cs
// ���������� YFGainMult ���� ó��
    private EngineLevelSystem.EEngineStatType GetDefaultStatTypeForEngine(int engineID)
    {
        return engineID switch
        {
            20102 => EngineLevelSystem.EEngineStatType.YFGainMult, // ����
            21101 => EngineLevelSystem.EEngineStatType.Health, // ��ũ��
            21102 => EngineLevelSystem.EEngineStatType.AuraRange, // �����
            21103 => EngineLevelSystem.EEngineStatType.MoveSpd, // ���̽�ũ��
            _ => EngineLevelSystem.EEngineStatType.Health
        };
    }

    public void OpenUpgradeUI
        (int engineID, EngineLevelSystem.EEngineStatType statType = 
            (EngineLevelSystem.EEngineStatType)(-1))
    {
        mEngineID = engineID;
        var data = BoomBoomEngineDatabase.GetEngineData(engineID);
        
        bIsUniqueType = data.IsUniqueType;
        mUpgradeStatType = statType == (EngineLevelSystem.EEngineStatType)(-1)
            ? GetDefaultStatTypeForEngine(engineID)
            : statType;

        //  ���� ������ ��� ���� ĳ�� ����
        if (bIsUniqueType)
            EngineLevelSystem.ForceReloadUniqueLevel(engineID);

        panel.SetActive(true);

        if (data != null)
        {
            engineNameText.text = data.Name;
            SetEngineSprite(engineID);
            UpdateAllLevelUI();
        }
        else
        {
            engineNameText.text = "???";
        }
    }

    public void OnUpgradeButton()
    {
        var engineData = BoomBoomEngineDatabase.GetEngineData(mEngineID);
        bool isUnique = engineData != null && engineData.IsUniqueType;

        bool success = isUnique
            ? EngineLevelSystem.LevelUpUnique(mEngineID)
            : EngineLevelSystem.LevelUpStat(mEngineID, mUpgradeStatType, 1);

        if (success)
        {
            Debug.Log($"[{mEngineID}] ������ ����!");
            UpdateAllLevelUI();
        }
    }

    private void UpdateAllLevelUI()
    {
        const int maxLevel = 50;
        BoomBoomEngineData data = BoomBoomEngineDatabase.GetEngineData(mEngineID);

        int curLevel = bIsUniqueType
            ? EngineLevelSystem.GetUniqueLevel(mEngineID)
            : EngineLevelSystem.GetStatLevel(mEngineID, mUpgradeStatType);

        Debug.Log($"[UI] UpdateAllLevelUI ȣ��� �� CurLevel: {curLevel}");

        levelText.text = $"{curLevel} / {maxLevel}";
        float growthValue = data.GrowthTable[Mathf.Clamp(curLevel, 0, data.GrowthTable.Length - 1)];
        descriptionText.text = string.Format(data.DescriptionFormat, growthValue);
    }

    public void ResetAllStats()
    {
        var allEngines = BoomBoomEngineDatabase.GetAllEngines(); // ��ü ��ϵ� ���� ���
        foreach (var engine in allEngines)
        {
            int engineID = engine.EngineID;

            // 1. �ʱ�ȭ
            EngineLevelSystem.ResetEngine(engineID);  

            // 2. ĳ�� ���� ����
            EngineLevelSystem.ForceReloadUniqueLevel(engineID);

            // 3. ���� ȿ�� ��ũ��Ʈ Init ��ȣ�� (���� ������ ������ �����)
            var player = PlaySystemRefStorage.playerStatManager;
            if (player != null)
            {
                var effectScripts = player.GetComponents<MonoBehaviour>();
                foreach (var script in effectScripts)
                {
                    if (script is IBoomBoomEngineEffect)
                    {
                        var initMethod = script.GetType().GetMethod("Init");
                        if (initMethod != null)
                        {
                            int level = EngineLevelSystem.GetUniqueLevel(engineID);
                            initMethod.Invoke(script, new object[] { engineID, level });
                            Debug.Log($"[ResetAllStats] Init ȣ���: {script.GetType().Name} (EngineID={engineID})");
                        }
                    }
                }
            }
        }

        Debug.Log("[ResetAllStats] ��ü ���� �ʱ�ȭ �Ϸ�");

        // 4. UI ������ ���� ���õ� ������
        RefreshUI();
    }
    
    private void RefreshUI()
    {
        UpdateAllLevelUI(); // ���� �ؽ�Ʈ �� ���� �ٽ� ����
    }
    
    private void SetEngineSprite(int engineID)
    {
        int index = GetSpriteIndexByEngineID(engineID);
        engineImage.sprite = (index >= 0 && index < engineSprites.Length) ? engineSprites[index] : null;
    }

    private int GetSpriteIndexByEngineID(int engineID)
    {
        switch (engineID)
        {
            case 20101: return 0;
            case 20303: return 1;
            case 20102: return 2;
            case 20204: return 3;
            case 20301: return 4;
            case 20202: return 5;
            case 20205: return 6;
            case 20302: return 7;
            case 20203: return 8;
            case 20402: return 9;
            case 20403: return 10;
            case 20401: return 11;
            case 20201: return 12;
            case 21101: return 13;
            case 21103: return 14;
            case 21102: return 15;
            case 21202: return 16;
            case 21302: return 17;
            case 20413: return 18;
            case 20412: return 19;
            case 21301: return 20;
            case 21201: return 21;
            case 20411: return 22;
            default: return -1;
        }
    }

    public void CloseUI()
    {
        Destroy(gameObject);
    }
}

    