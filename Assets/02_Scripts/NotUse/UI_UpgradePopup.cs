using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UI_UpgradePopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI unitNameText;
    [SerializeField] private Image unitImage;
    [SerializeField] private GameObject panel;
    [SerializeField] private Sprite[] unitSprites;
    
    
    [Header("스탯 레벨 텍스트")]
    [SerializeField] private TextMeshProUGUI[] statLevelTexts;  // 인덱스: 스탯 순서 동일하게 연결
    
    
    [Header("강화 요구 재화 텍스트")]
    [SerializeField] private TextMeshProUGUI[] statYellowCostTexts;
    [SerializeField] private TextMeshProUGUI[] statOrangeCostTexts;
    [SerializeField] private GameObject[] maxLevelTexts;
    [SerializeField] private GameObject[] maxLevelRemoves;

    [Header("업그레이드 버튼")]
    [SerializeField] private Button[] upgradeButtons;  // 인덱스: 스탯 순서 동일하게 연결

    [Header("상세 스탯")] 
    [SerializeField] private GameObject statDetailPanel;
    [SerializeField] private TextMeshProUGUI[] statDetailText;
    
    [Header("UI 관련")]
    [SerializeField] private RectTransform mBgRectTransform;

    private int currentUnitID;
    private UI_Upgrade mUpgradeUI;
    
    public void Init(UI_Upgrade upgradeUI)
    {
        mUpgradeUI = upgradeUI;
    }
    

    private void Awake()
    {
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            int statIndex = i;  // 클로저 방지
            upgradeButtons[i].onClick.AddListener(() => LevelUp((UnimoLevelSystem.StatType)statIndex));
        }
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

    public void OpenUpgradeUI(int unitID)
    {
        currentUnitID = unitID;
        UnimoData data = UnimoDatabase.GetUnimoData(unitID);
        panel.SetActive(true);

        if (data != null)
        {
            unitNameText.text = data.Name;
            SetUnitSprite(unitID);
            UpdateAllLevelUI();
            UpdateStatDetailUI();
        }
        else
        {
            unitNameText.text = "???";
        }
    }

    public void ResetAllStats()
    {
        UnimoLevelSystem.ResetUnitStats(currentUnitID);  // UnimoLevelSystem에 정의됨
        UpdateAllLevelUI();
        Debug.Log($"[{currentUnitID}] 스탯 전부 리셋됨");
    }
    
    private void UpdateStatDetailUI()
    {
        UnimoData data = UnimoDatabase.GetUnimoData(currentUnitID);
        if (data == null)
            return;

        var unimoStat = new UnimoRuntimeStat(data.Stat);
        unimoStat.RecalculateFinalStat();

        var stat = unimoStat.FinalStat;
        
        statDetailText[0].text  = $"{stat.Health:N0}";
        statDetailText[1].text  = $"+{stat.Armor:F2}";
        statDetailText[2].text  = $"{stat.MoveSpd:F2}";
        statDetailText[3].text = $"{stat.AuraRange:F2}m";
        statDetailText[4].text = $"{stat.AuraStr:F2}";
        statDetailText[5].text  = $"{stat.CriticalChance * 100f:F0}%";
        statDetailText[6].text  = $"x{stat.CriticalMult:F2}";
        statDetailText[7].text  = $"x{stat.YFGainMult:F2}";
        statDetailText[8].text = $"x{stat.OFGainMult:F2}";
        statDetailText[9].text  = $"+{stat.HealthRegen:F1}";
        statDetailText[10].text  = $"x{stat.HealingMult:F2}";
        statDetailText[11].text  = $"{stat.StunIgnoreChance * 100f:F0}%";
        statDetailText[12].text  = $"{stat.StunResistanceRate * 100f:F0}%";
    }

    public void DetailStat()
    {
        if (statDetailPanel.activeSelf)
        {
            statDetailPanel.SetActive(false);
        }
        else
        {
            statDetailPanel.SetActive(true);
        }
    }
    
    private void SetUnitSprite(int unitID)
    {
        int spriteIndex = GetSpriteIndexByUnitID(unitID);
        unitImage.sprite = (spriteIndex >= 0 && spriteIndex < unitSprites.Length) ? unitSprites[spriteIndex] : null;
    }

    private int GetSpriteIndexByUnitID(int unitID)
    {
        switch (unitID)
        {
            case 10101: return 0;
            case 10303: return 1;
            case 10102: return 2;
            case 10204: return 3;
            case 10301: return 4;
            case 10202: return 5;
            case 10205: return 6;
            case 10302: return 7;
            case 10203: return 8;
            case 10402: return 9;
            case 10403: return 10;
            case 10401: return 11;
            case 10201: return 12;
            default: return -1;
        }
    }

    private void LevelUp(UnimoLevelSystem.StatType stat)
    {
        int increaseAmount = 1;   // 한 번 클릭 시 10레벨씩 상승 (디버그용) 나중에 1로 수정

        bool success = UnimoLevelSystem.LevelUp(currentUnitID, stat, increaseAmount);

        if (success)
        {
            Debug.Log($"[{currentUnitID}] {stat} {increaseAmount} 레벨 상승 완료!");
            UpdateAllLevelUI();
            UpdateStatDetailUI();
            // 여기다가 강화 버튼 눌렀다는 데이터 추가하면 될 듯
            Base_Manager.Data.UserData.Touch++;
            Base_Manager.Data.UserData.ReinforceCountTotal++;
        }
    }

    private void UpdateAllLevelUI()
    {
        for (int i = 0; i < statLevelTexts.Length; i++)
        {
            int level = UnimoLevelSystem.GetLevel(currentUnitID, (UnimoLevelSystem.StatType)i);
            int maxLevel = (i == (int)UnimoLevelSystem.StatType.CriticalChance) ? 100 : 250;
            statLevelTexts[i].text = $"{level} / {maxLevel}";
        }
        
        UpdateAllCostUI();
        Main_UI.instance.Text_Check();
    }
    
    private void UpdateAllCostUI()
    {
        for (int i = 0; i < statYellowCostTexts.Length; i++)
        {
            int currentLevel = UnimoLevelSystem.GetLevel(currentUnitID, (UnimoLevelSystem.StatType)i);
            int nextLevel = currentLevel + 1;

            if (UnimoLevelSystem.TryGetCost((UnimoLevelSystem.StatType)i, nextLevel, out var cost))
            {
                maxLevelRemoves[i].SetActive(true);
                maxLevelTexts[i].SetActive(false);
                statYellowCostTexts[i].text = StringMethod.ToCurrencyString(cost.RequireYF);
                statOrangeCostTexts[i].text = StringMethod.ToCurrencyString(cost.RequireOF);
            }
            else
            {
                maxLevelRemoves[i].SetActive(false);
                maxLevelTexts[i].SetActive(true);
            }
        }
    }
    
    public void OnClickBBEngineTab()
    {
        CloseUI(); 

        if (mUpgradeUI != null)
        {
            mUpgradeUI.GetEQ();
        }
    }

    public void CloseUI()
    {
        Destroy(gameObject);
    }
}