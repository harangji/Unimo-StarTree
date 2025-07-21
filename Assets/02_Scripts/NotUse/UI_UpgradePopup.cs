using TMPro;
using UnityEngine;

public class UI_UpgradePopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private GameObject panel;

    public void OpenUpgradeUI(int unitID)
    {
        UnimoData data = UnimoDatabase.GetUnimoData(unitID);
        panel.SetActive(true);

        if (data != null)
        {
            titleText.text = $"{data.Name} 업그레이드";
            // 추가적인 스탯 표시 및 업그레이드 항목 표시 구현
        }
        else
        {
            titleText.text = "알 수 없는 유닛";
        }
    }

    public void CloseUI()
    {
        panel.SetActive(false);
    }
}