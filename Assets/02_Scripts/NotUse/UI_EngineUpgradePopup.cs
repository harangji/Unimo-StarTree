using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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
    
    [SerializeField] private TextMeshProUGUI mYellowCostText;
    [SerializeField] private TextMeshProUGUI mOrangeCostText;
    
    [Header("UI 관련")]
    [SerializeField] private RectTransform mBgRectTransform;

    private int mEngineID;
    private EngineLevelSystem.EEngineStatType mUpgradeStatType;
    private bool bIsUniqueType;
    
    private UI_Upgrade mUpgradeUI; // 첫 번째 팝업 참조

    public void Init(UI_Upgrade upgradeUI)
    {
        mUpgradeUI = upgradeUI;
    }

    
    // 2. UI_EngineUpgradePopup.cs
// 곰곰엔진은 YFGainMult 고정 처리
    private EngineLevelSystem.EEngineStatType GetDefaultStatTypeForEngine(int engineID)
    {
        return engineID switch
        {
            20102 => EngineLevelSystem.EEngineStatType.YFGainMult, // 곰곰
            21101 => EngineLevelSystem.EEngineStatType.Health, // 오크통
            21102 => EngineLevelSystem.EEngineStatType.AuraRange, // 마녀솥
            21103 => EngineLevelSystem.EEngineStatType.MoveSpd, // 아이스크림
            _ => EngineLevelSystem.EEngineStatType.Health
        };
    }
    
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(
                    mBgRectTransform, 
                    Input.mousePosition, 
                    null))
            {
                Destroy(gameObject);
            }
        }
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

        //  고유 엔진일 경우 강제 캐시 리셋
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

        Debug.Log($"[UI] UpdateAllLevelUI 호출됨 ▶ CurLevel: {curLevel}");

        mYellowCostText.text = StringMethod.ToCurrencyString(RewardCalculator.EngineYellowCost(curLevel));
        mOrangeCostText.text = StringMethod.ToCurrencyString(RewardCalculator.EngineOrangeCost(curLevel));

        levelText.text = $"Lv. {curLevel} / {maxLevel}";

        // GrowthTable 값 안전하게 가져오기
        float growthValue = 0f;
        if (data?.GrowthTable != null && data.GrowthTable.Length > 0)
            growthValue = data.GrowthTable[Mathf.Clamp(curLevel, 0, data.GrowthTable.Length - 1)];

        // DescriptionFormat 안전 처리
        if (!string.IsNullOrEmpty(data?.DescriptionFormat))
        {
            if (data.DescriptionFormat.Contains("{0")) // 포맷이 있는 경우
            {
                descriptionText.text = string.Format(data.DescriptionFormat, growthValue);
            }
            else
            {
                descriptionText.text = data.DescriptionFormat; // 그대로 표시
            }
        }
        else
        {
            descriptionText.text = string.Empty; // DescriptionFormat이 없으면 빈 문자열
        }

        Debug.Log($"[UI] EngineID: {mEngineID}, CurLevel: {curLevel}, GrowthValue(raw): {growthValue}, DisplayText: {descriptionText.text}");
    }

    public void ResetAllStats()
    {
        var allEngines = BoomBoomEngineDatabase.GetAllEngines(); // 전체 등록된 엔진 목록
        foreach (var engine in allEngines)
        {
            int engineID = engine.EngineID;

            // 1. 초기화
            EngineLevelSystem.ResetEngine(engineID);  

            // 2. 캐시 강제 갱신
            EngineLevelSystem.ForceReloadUniqueLevel(engineID);

            // 3. 고유 효과 스크립트 Init 재호출 (현재 장착된 엔진만 적용됨)
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
                            Debug.Log($"[ResetAllStats] Init 호출됨: {script.GetType().Name} (EngineID={engineID})");
                        }
                    }
                }
            }
        }

        Debug.Log("[ResetAllStats] 전체 엔진 초기화 완료");

        // 4. UI 갱신은 현재 선택된 엔진만
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
    
    public void OnClickUnimoTab()
    {
        CloseUI(); 

        if (mUpgradeUI != null)
        {
            mUpgradeUI.GetUnimo();
        }
    }

    public void CloseUI()
    {
        Destroy(gameObject);
    }
}

    