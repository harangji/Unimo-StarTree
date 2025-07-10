using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;
public class UI_Alta : UI_Base
{
    [SerializeField] private Slider m_Slider;
    [SerializeField] private TextMeshProUGUI m_Slider_Text;
    [SerializeField] private TextMeshProUGUI LevelText;
    [SerializeField] private TextMeshProUGUI GetSecondText;
    [SerializeField] private TextMeshProUGUI NextLevelText;
    [SerializeField] private GameObject[] objs;
    [SerializeField] private TextMeshProUGUI SecText;
    public Color[] colors;
    int value;
    public GameObject InformationPanel;

    public override void Start()
    {
        Main_UI.OnActionEvent += Text_Check;

        bool NoneDefault = false;
        for (int i = 0; i < objs.Length; i++) objs[i].SetActive(false);
        for (int i = 0; i < 4; i++)
        {
            if (Base_Manager.Data.UserData.Level >= Base_Manager.Data.AltaCount[3 - i])
            {
                NoneDefault = true;
                value = 4-i;
                objs[value].SetActive(true);
                break;
            }
        }
        if(NoneDefault == false)
        {
            value = 0;
            objs[0].SetActive(true);
        }

        Text_Check();
        base.Start();
    }
    public void Text_Check()
    {
        m_Slider.value = Base_Manager.Data.EXP_Percentage();
        m_Slider_Text.text = string.Format("{0:0.00}", Base_Manager.Data.EXP_Percentage() * 100.0f) + "%";
        LevelText.text = "LV." + (Base_Manager.Data.UserData.Level + 1).ToString();
        NextLevelText.text = StringMethod.ToCurrencyString(Base_Manager.Data.UserData.NextLevel_Base);
        SecText.text = StringMethod.ToCurrencyString(Base_Manager.Data.UserData.Second_Base) + "/Sec";
        TextColorCheck();
    }

    [SerializeField] private Image LevelAssetImage;
    public void TextColorCheck()
    {
        NextLevelText.color = Base_Manager.Data.UserData.Yellow >= Base_Manager.Data.UserData.NextLevel_Base ? colors[0] : colors[1];
        LevelAssetImage.color = Base_Manager.Data.UserData.Yellow >= Base_Manager.Data.UserData.NextLevel_Base ?
            new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0.5f);
    }
    public override void DisableOBJ()
    {
        Main_UI.OnActionEvent -= Text_Check;
        base.DisableOBJ();
    }

    public void GetInformation(bool Check)
    {
        InformationPanel.SetActive(Check);
    }
}
