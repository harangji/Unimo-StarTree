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
            titleText.text = $"{data.Name} ���׷��̵�";
            // �߰����� ���� ǥ�� �� ���׷��̵� �׸� ǥ�� ����
        }
        else
        {
            titleText.text = "�� �� ���� ����";
        }
    }

    public void CloseUI()
    {
        panel.SetActive(false);
    }
}