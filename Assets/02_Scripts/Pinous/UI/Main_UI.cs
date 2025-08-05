using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class Main_UI : MonoBehaviour
{
    public delegate void actionEvent();
    public static event actionEvent OnActionEvent;
    public static Main_UI instance;
    public UI_ADS_INTERESTING[] ADSBuyCheckPanel;
    public Animator StarChangeAnimation;
    public Queue<string> holderQueue = new Queue<string>();
    public Queue<Action> holderQueue_Action = new Queue<Action>();
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        Debug.Log($"Base_Manager.Data.UserData.Second_Base : {Base_Manager.Data.UserData.Second_Base}");
        Text_Check();
        RectADSCheck();

        if(Base_Manager.Data.UserData.GetOarkTong == false && Base_Manager.Data.UserData.HasEnginData[13] == false)
        {
            Base_Manager.Data.UserData.GetOarkTong = true;

            holderQueue.Enqueue("##Reward");
            holderQueue_Action.Enqueue(() =>
            {
                Canvas_Holder.UI_Holder.Peek().transform.parent = Canvas_Holder.instance.transform;
                Canvas_Holder.UI_Holder.Peek().GetComponent<UI_Reward>().GetRewardInit(RewardState.Character, CharCostumer.EQ, 0, 1, 13);
            });
        }

        if(Base_Manager.Data.UserData.GetStarChange == false)
        {
            StarChangeAnimation.SetTrigger("NoneStarChange");
        }
        else
        {
            StarChangeAnimation.SetBool("GetStarChange", true);
        }

        if(Data_Manager.SetPlayScene)
        {
            Data_Manager.SetPlayScene = false;
            if(Base_Manager.Data.UserData.GetReview == false && Base_Manager.Data.UserData.Level >= 99)
            {
                Base_Manager.Data.UserData.GetReview = true;
                holderQueue.Enqueue("##Review");
                holderQueue_Action.Enqueue(null);
            }
        }

        if(!Base_Manager.Data.UserData.GetInGame)
        {
            Base_Manager.Data.UserData.GetInGame = true;
            holderQueue.Enqueue("##ADSREMOVER");
            holderQueue_Action.Enqueue(() => Canvas_Holder.instance.Get_Toast("ADSRemovePopUp"));
        }

        Debug.Log(holderQueue.Count);
        if(holderQueue.Count > 0)
        {
            Canvas_Holder.instance.GetUI(holderQueue.Dequeue());
            holderQueue_Action.Dequeue()?.Invoke();
        }
    }

    public void GetStarChangeCheck()
    {
        Base_Manager.Data.UserData.GetStarChange = true;
        StarChangeAnimation.SetBool("GetStarChange", true);
    }

    public void RectADSCheck()
    {
        for(int i = 0; i < ADSBuyCheckPanel.Length; i++)
            ADSBuyCheckPanel[i].GetRectADS();
    }

    [SerializeField] private Image m_Slider;
    [SerializeField] private TextMeshProUGUI m_Slider_Text;
    [SerializeField] private TextMeshProUGUI LevelText;
    [SerializeField] private TextMeshProUGUI[] Assets_Text;
    [SerializeField] private TextMeshProUGUI GetSecondText;
    [SerializeField] private TextMeshProUGUI NextLevelText;
    [SerializeField] private TextMeshProUGUI NameText;
    [SerializeField] private GameObject[] objs;
    int value;
    
    [SerializeField] private GameObject levelUpCostGroup;  
    [SerializeField] private GameObject gradeUpCostGroup;
    [SerializeField] private TextMeshProUGUI gradeUpYfCostText;
    [SerializeField] private TextMeshProUGUI gradeUpOfCostText;
    [SerializeField] private Image gradeUpOfCostImg;
    
    public void Text_Check()
    {
        double expNow = Base_Manager.Data.UserData.EXP;
        double expAdd = Base_Manager.Data.EXP_GET;
        double expMax = Base_Manager.Data.EXP_SET;
        int nextLevel = Base_Manager.Data.UserData.Level + 2;

        bool isLevelUp = (expNow + expAdd >= expMax);
        bool isGradeUp = (nextLevel == 100 || nextLevel == 300 || nextLevel == 700 || nextLevel == 1000);

        if (isLevelUp && isGradeUp)
        {
            double gradeCost = RewardCalculator.GetGradeUpCost();

            levelUpCostGroup.SetActive(false);
            gradeUpCostGroup.SetActive(true);

            gradeUpYfCostText.text = StringMethod.ToCurrencyString(gradeCost);
            gradeUpOfCostText.text = StringMethod.ToCurrencyString(gradeCost);
        }
        else
        {
            double yellowCost = RewardCalculator.GetLevelUpCost();
            yellowCost /= 5;

            levelUpCostGroup.SetActive(true);
            gradeUpCostGroup.SetActive(false);

            NextLevelText.text = StringMethod.ToCurrencyString(yellowCost);
        }
        
        bool NoneDefault = false;
        for (int i = 0; i < 5; i++) objs[i].SetActive(false);

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
        if (NoneDefault == false)
        {
            value = 0;
            objs[0].SetActive(true);
        }

        m_Slider.fillAmount = Base_Manager.Data.EXP_Percentage();
        m_Slider_Text.text = $"{Base_Manager.Data.EXP_Percentage() * 100.0f:0.00} %";
        LevelText.text = "LV." + (Base_Manager.Data.UserData.Level + 1).ToString();

        Assets_Text[0].text = StringMethod.ToCurrencyString(Base_Manager.Data.UserData.Yellow);
        Assets_Text[1].text = StringMethod.ToCurrencyString(Base_Manager.Data.UserData.Red);
        Assets_Text[2].text = StringMethod.ToCurrencyString(Base_Manager.Data.UserData.Blue);
        // Assets_Text[1].text = string.Format("{0:#,###}", Base_Manager.Data.UserData.Red);
        // Assets_Text[2].text = Base_Manager.Data.UserData.Blue.ToString();
        
        GetSecondText.text = StringMethod.ToCurrencyString(RewardCalculator.GetYfByAltaLevel()) + "/Sec"
            +"\n" + StringMethod.ToCurrencyString(RewardCalculator.GetOfByAltaLevel()) + "/Sec";
        // GetSecondText.text = StringMethod.ToCurrencyString(Base_Manager.Data.UserData.Second_Base) + "/Sec";
        // NextLevelText.text = StringMethod.ToCurrencyString(Base_Manager.Data.UserData.NextLevel_Base);

        NameText.text = Base_Manager.Data.UserData.UserName;
        OnActionEvent?.Invoke();
        TextColorCheck();
    }

    public Color[] colors;
    public Image NextLevelImage;
    public void TextColorCheck()
    {
        NextLevelText.color = Base_Manager.Data.UserData.Yellow >= Base_Manager.Data.UserData.NextLevel_Base ? colors[0] : colors[1];
        NextLevelImage.color = Base_Manager.Data.UserData.Yellow >= Base_Manager.Data.UserData.NextLevel_Base ?
            new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0.5f);
        
        if (Base_Manager.Data.UserData.Yellow < RewardCalculator.GetGradeUpCost() ||
            Base_Manager.Data.UserData.Red < RewardCalculator.GetGradeUpCost())
        {
            gradeUpOfCostImg.color = new Color(0.6f, 0.6f, 0.6f, 0);
        }
        else
        {
            gradeUpOfCostImg.color = new Color(0.6f, 1, 0.3f, 0.8f);
        }
    }
}
