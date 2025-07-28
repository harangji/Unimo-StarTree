using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_UpgradePopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI unitNameText;
    [SerializeField] private Image unitImage;
    [SerializeField] private GameObject panel;
    [SerializeField] private Sprite[] unitSprites;
    
    
    [Header("스탯 레벨 텍스트")]
    [SerializeField] private TextMeshProUGUI[] statLevelTexts;  // 인덱스: 스탯 순서 동일하게 연결

    [Header("업그레이드 버튼")]
    [SerializeField] private Button[] upgradeButtons;  // 인덱스: 스탯 순서 동일하게 연결

    private int currentUnitID;

    private void Awake()
    {
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            int statIndex = i;  // 클로저 방지
            upgradeButtons[i].onClick.AddListener(() => LevelUp((UnimoLevelSystem.StatType)statIndex));
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
        int increaseAmount = 10;   // 한 번 클릭 시 10레벨씩 상승 (디버그용) 나중에 1로 수정

        bool success = UnimoLevelSystem.LevelUp(currentUnitID, stat, increaseAmount);

        if (success)
        {
            Debug.Log($"[{currentUnitID}] {stat} {increaseAmount} 레벨 상승 완료!");
            UpdateAllLevelUI();
            // 여기다가 강화 버튼 눌렀다는 데이터 추가하면 될 듯
            Base_Manager.Data.UserData.Touch++;
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
    }

    public void CloseUI()
    {
        Destroy(gameObject);
    }
}