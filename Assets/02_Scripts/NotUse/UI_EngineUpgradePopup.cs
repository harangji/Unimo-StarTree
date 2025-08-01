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

    
    public void OpenUpgradeUI(int engineID, bool isUniqueType, EngineLevelSystem.EEngineStatType statType = EngineLevelSystem.EEngineStatType.Health)
    {
        mEngineID = engineID;
        bIsUniqueType = isUniqueType;
        mUpgradeStatType = statType;

        BoomBoomEngineData data = BoomBoomEngineDatabase.GetEngineData(engineID);
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
        bool success = bIsUniqueType
            ? EngineLevelSystem.LevelUpUnique(mEngineID, 1)
            : EngineLevelSystem.LevelUpStat(mEngineID, mUpgradeStatType, 1);

        if (success)
        {
            Debug.Log($"[{mEngineID}] 레벨업 성공!");
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

        levelText.text = $"{curLevel} / {maxLevel}";

        float growthValue = data.GrowthTable[Mathf.Clamp(curLevel, 0, data.GrowthTable.Length - 1)];
        descriptionText.text = string.Format(data.DescriptionFormat, growthValue);
    }

    public void ResetAllStats()
    {
        int engineID = GameManager.Instance.SelectedEngineID;

        // 레벨 초기화
        EngineLevelSystem.ResetEngine(engineID);  
        Debug.Log($"[UI] {engineID} 엔진 스탯 초기화 완료");

        // 해당 엔진 고유 효과 초기화 로직 호출
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
                        Debug.Log($"[ResetAllStats] Init 호출됨: {script.GetType().Name}");
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("[ResetAllStats] playerStatManager가 null입니다. 인게임에서만 호출 가능.");
        }

        RefreshUI();
    }
    
    
    private void RefreshUI()
    {
        UpdateAllLevelUI(); // 레벨 텍스트 및 설명 다시 갱신
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

    