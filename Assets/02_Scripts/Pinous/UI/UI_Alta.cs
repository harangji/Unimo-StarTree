using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;
public class UI_Alta : UI_Base
{
    private static readonly string[] sBuffTexts = new string[]
    {
        "¹æ¾î·Â\n+ 5%",
        "Ã¼·Â\n+ 5%",
        "³ë¶û º°²Ü\nÈ¹µæ·® Áõ°¡\n+ 5%",
        "ÁÖÈ² º°²Ü\nÈ¹µæ·® Áõ°¡\n+ 5%",
        "Å©¸®Æ¼ÄÃ È®·ü\n+ 5%"
    };
    
    [SerializeField] private Slider m_Slider;
    [SerializeField] private TextMeshProUGUI m_Slider_Text;
    [SerializeField] private TextMeshProUGUI LevelText;
    [SerializeField] private TextMeshProUGUI GetSecondText;
    [SerializeField] private TextMeshProUGUI NextLevelText;
    [SerializeField] private GameObject[] objs;
    [SerializeField] private TextMeshProUGUI SecText;
    
    [SerializeField] private GameObject levelGroup;
    [SerializeField] private GameObject gradeGroup;
    [SerializeField] private Image gradeImg;
    [SerializeField] private TextMeshProUGUI gradeYfText;
    [SerializeField] private TextMeshProUGUI gradeOfText;

    [SerializeField] private Image[] levelBuffImgs;
    [SerializeField] private TextMeshProUGUI[] levelBuffTexts;
    [SerializeField] private LevelUp_Button mLevelUpBtn;
    
    public Color[] colors;
    int value;
    public GameObject InformationPanel;
    
    private double mYfValue, mOfValue;

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
        if (Base_Manager.Data.UserData.Level >= Data_Manager.MaxLevel)
        {
            levelGroup.SetActive(true);
            gradeGroup.SetActive(false);

            // °ÔÀÌÁö 100% °íÁ¤
            m_Slider.value = 1f;
            m_Slider_Text.text = "100.00%";
            NextLevelText.text = "MAX";
            
            mLevelUpBtn.canPush = false;
        }
        else
        {
            if (Base_Manager.Data.PendingGradeUp)
            {
                levelGroup.SetActive(false);
                gradeGroup.SetActive(true);

                double gradeCost = RewardCalculator.GetGradeUpCost();
                gradeYfText.text = StringMethod.ToCurrencyString(gradeCost);
                gradeOfText.text = StringMethod.ToCurrencyString(gradeCost);
            }
            else
            {
                double yellowCost = RewardCalculator.GetLevelUpCost() / 5;
                levelGroup.SetActive(true);
                gradeGroup.SetActive(false);
                NextLevelText.text = StringMethod.ToCurrencyString(yellowCost);
            }
            m_Slider.value = Base_Manager.Data.EXP_Percentage();
            m_Slider_Text.text = string.Format("{0:0.00}", Base_Manager.Data.EXP_Percentage() * 100.0f) + "%";
            mLevelUpBtn.canPush = true;
        }
        
        if (Base_Manager.Data.UserData.BuffFloating[0]>0.0f)
        {
            mYfValue = RewardCalculator.GetYfByAltaLevel() * 2;
            mOfValue = RewardCalculator.GetOfByAltaLevel() * 2;
        }
        else
        {
            mYfValue = RewardCalculator.GetYfByAltaLevel();
            mOfValue = RewardCalculator.GetOfByAltaLevel();
        }
        
        LevelText.text = "LV." + (Base_Manager.Data.UserData.Level + 1).ToString();
        // NextLevelText.text = StringMethod.ToCurrencyString(RewardCalculator.GetLevelUpCost());
        // NextLevelText.text = StringMethod.ToCurrencyString(Base_Manager.Data.UserData.NextLevel_Base);
        SecText.text = StringMethod.ToCurrencyString(mYfValue) + "/Sec"
            +"\n" + StringMethod.ToCurrencyString(mOfValue) + "/Sec";
        
        LevelBuffColorCheck();
        
        TextColorCheck();
    }

    private void LevelBuffColorCheck()
    {
        var level = Base_Manager.Data.UserData.Level + 1;
        int activeCount = 0;

        if (level < 100) activeCount = 1;
        else if (level < 300) activeCount = 2;
        else if (level < 700) activeCount = 3;
        else if (level < 1000) activeCount = 4;
        else activeCount = 5;

        for (int i = 0; i < levelBuffImgs.Length; i++)
        {
            levelBuffImgs[i].color = (i < activeCount) ? new Color(1, 1, 1, 1) : new Color(0, 0, 0, 1);
        }

        for (int i = 0; i < levelBuffTexts.Length; i++)
        {
            if (i < activeCount)
                levelBuffTexts[i].text = sBuffTexts[i];
            else
                levelBuffTexts[i].text = "???";
        }
    }

    [SerializeField] private Image LevelAssetImage;
    public void TextColorCheck()
    {
        // NextLevelText.color = Base_Manager.Data.UserData.Yellow >= Base_Manager.Data.UserData.NextLevel_Base ? colors[0] : colors[1];
        NextLevelText.color = Base_Manager.Data.UserData.Yellow >= RewardCalculator.GetLevelUpCost() ? colors[0] : colors[1];
        LevelAssetImage.color = Base_Manager.Data.UserData.Yellow >= RewardCalculator.GetLevelUpCost() ?
            new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0.5f);
        
        if (Base_Manager.Data.UserData.Yellow < RewardCalculator.GetGradeUpCost() ||
            Base_Manager.Data.UserData.Red < RewardCalculator.GetGradeUpCost())
        {
            gradeImg.color = new Color(0.6f, 0.6f, 0.6f, 0);
        }
        else
        {
            gradeImg.color = new Color(0.6f, 1, 0.3f, 0.8f);
        }
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
