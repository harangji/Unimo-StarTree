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
    public LevelUp_Button levelUp_Button;
    
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
        // double expNow = Base_Manager.Data.UserData.EXP;
        // double expAdd = Base_Manager.Data.EXP_GET;
        // double expMax = Base_Manager.Data.EXP_SET;
        
        // 최대 레벨 도달 시
        if (Base_Manager.Data.UserData.Level >= Data_Manager.MaxLevel)
        {
            levelUpCostGroup.SetActive(true);
            gradeUpCostGroup.SetActive(false);

            // 게이지 100% 고정
            m_Slider.fillAmount = 1f;
            m_Slider_Text.text = "100.00 %";
            NextLevelText.text = "MAX";

            // 버튼 비활성화
            levelUp_Button.canPush = false;
            // foreach (var btn in FindObjectsOfType<LevelUp_Button>())
            // {
            //     btn.GetComponent<UnityEngine.UI.Button>().interactable = false; //버튼 없으므로 오류
            // }
            
            return; // 이하 일반 로직 실행 안 함
        }
        
        if (Base_Manager.Data.PendingGradeUp)
        {
            levelUpCostGroup.SetActive(false);
            gradeUpCostGroup.SetActive(true);

            double gradeCost = RewardCalculator.GetGradeUpCost();
            gradeUpYfCostText.text = StringMethod.ToCurrencyString(gradeCost);
            gradeUpOfCostText.text = StringMethod.ToCurrencyString(gradeCost);
        }
        else
        {
            bool expFull = Base_Manager.Data.EXP_Percentage() >= 1f;
            bool isExactGradeLevel = Array.Exists(Base_Manager.Data.AltaCount,
                lvl => lvl == Base_Manager.Data.UserData.Level);

            if (expFull && isExactGradeLevel)
            {
                levelUpCostGroup.SetActive(false);
                gradeUpCostGroup.SetActive(true);
                double gradeCost = RewardCalculator.GetGradeUpCost();
                gradeUpYfCostText.text = StringMethod.ToCurrencyString(gradeCost);
                gradeUpOfCostText.text = StringMethod.ToCurrencyString(gradeCost);
            }
            else
            {
                // 그 외는 일반 레벨업 UI
                double yellowCost = RewardCalculator.GetLevelUpCost() / 5;
                levelUpCostGroup.SetActive(true);
                gradeUpCostGroup.SetActive(false);
                NextLevelText.text = StringMethod.ToCurrencyString(yellowCost);
            }
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
        
        GetSecondText.text = StringMethod.ToCurrencyString(RewardCalculator.GetYfByAltaLevel()) + "/Sec"
            +"\n" + StringMethod.ToCurrencyString(RewardCalculator.GetOfByAltaLevel()) + "/Sec";

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
