using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UI_EngineUpgradePopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI engineNameText;
    [SerializeField] private Image engineImage;
    [SerializeField] private GameObject panel;
    [SerializeField] private Sprite[] engineSprites;
    
    [Header("스탯 레벨 텍스트")]
    [SerializeField] private TextMeshProUGUI[] statLevelTexts;
    
    [Header("업그레이드 버튼")]
    [SerializeField] private Button[] upgradeButtons; 
    
    private int mEngineID;
    
    private void Awake()
    {
        // 버튼 클릭시 레벨업 (클로저 방지)
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            int statIndex = i;
            upgradeButtons[i].onClick.AddListener
                (() => LevelUp((EngineLevelSystem.EEngineStatType)statIndex));
        }
    }
    
    /// <summary>
    /// 엔진 업그레이드 팝업 오픈
    /// </summary>
    public void OpenUpgradeUI(int engineID)
    {
        mEngineID = engineID;
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
    
    /// <summary>
    /// 엔진 스탯 모두 리셋
    /// </summary>
    public void ResetAllStats()
    {
        EngineLevelSystem.ResetEngineStats(mEngineID);
        UpdateAllLevelUI();
        Debug.Log($"[{mEngineID}] 엔진 스탯 전부 리셋됨");
    }

    /// <summary>
    /// 엔진 스프라이트 세팅
    /// </summary>
    private void SetEngineSprite(int engineID)
    {
        int spriteIndex = GetSpriteIndexByEngineID(engineID);
        engineImage.sprite = 
            (spriteIndex >= 0 && spriteIndex < engineSprites.Length) ? engineSprites[spriteIndex] : null;
    }
    
    
    /// <summary>
    /// 엔진 ID로 스프라이트 인덱스 반환
    /// </summary>
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

    /// <summary>
    /// 특정 스탯 레벨업
    /// </summary>
    private void LevelUp(EngineLevelSystem.EEngineStatType stat)
    {
        int increaseAmount = 10;   // 디버그용. 실제론 1로 바꿔 사용

        bool success = EngineLevelSystem.LevelUp(mEngineID, stat, increaseAmount);

        if (success)
        {
            Debug.Log($"[{mEngineID}] {stat} {increaseAmount} 레벨 상승 완료!");
            UpdateAllLevelUI();
            // Base_Manager.Data.UserData.EngineReinforceCountTotal++; // 업적 시스템 현재는 주석 처리
        }
    }

    /// <summary>
    /// 모든 스탯 UI 갱신
    /// </summary>
    private void UpdateAllLevelUI()
    {
        const int maxLevel = 50;
        for (int i = 0; i < statLevelTexts.Length; i++)
        {
            int level = EngineLevelSystem.GetLevel(mEngineID, (EngineLevelSystem.EEngineStatType)i);
            statLevelTexts[i].text = $"{level} / {maxLevel}";
        }
    }

    public void CloseUI()
    {
        Destroy(gameObject);
    }
}
    