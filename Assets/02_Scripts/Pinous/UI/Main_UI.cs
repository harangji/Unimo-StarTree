using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
        Text_Check();
        RectADSCheck();

        if(Base_Mng.Data.data.GetOarkTong == false && Base_Mng.Data.data.GetEQData[13] == false)
        {
            Base_Mng.Data.data.GetOarkTong = true;

            holderQueue.Enqueue("##Reward");
            holderQueue_Action.Enqueue(() =>
            {
                Canvas_Holder.UI_Holder.Peek().transform.parent = Canvas_Holder.instance.transform;
                Canvas_Holder.UI_Holder.Peek().GetComponent<UI_Reward>().GetRewardInit(RewardState.Character, CharCostumer.EQ, 0, 1, 13);
            });
        }

        if(Base_Mng.Data.data.GetStarChange == false)
        {
            StarChangeAnimation.SetTrigger("NoneStarChange");
        }
        else
        {
            StarChangeAnimation.SetBool("GetStarChange", true);
        }

        if(Data_Mng.SetPlayScene)
        {
            Data_Mng.SetPlayScene = false;
            if(Base_Mng.Data.data.GetReview == false && Base_Mng.Data.data.Level >= 99)
            {
                Base_Mng.Data.data.GetReview = true;
                holderQueue.Enqueue("##Review");
                holderQueue_Action.Enqueue(null);
            }
        }

        if(!Base_Mng.Data.data.GetInGame)
        {
            Base_Mng.Data.data.GetInGame = true;
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
        Base_Mng.Data.data.GetStarChange = true;
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
    public void Text_Check()
    {
        bool NoneDefault = false;
        for (int i = 0; i < 5; i++) objs[i].SetActive(false);

        for (int i = 0; i < 4; i++)
        {
            if (Base_Mng.Data.data.Level >= Base_Mng.Data.AltaCount[3 - i])
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

        m_Slider.fillAmount = Base_Mng.Data.EXP_Percentage();
        m_Slider_Text.text = string.Format("{0:0.00}", Base_Mng.Data.EXP_Percentage() * 100.0f) +"%";
        LevelText.text = "LV." + (Base_Mng.Data.data.Level + 1).ToString();

        Assets_Text[0].text = StringMethod.ToCurrencyString(Base_Mng.Data.data.Yellow);
        Assets_Text[1].text = string.Format("{0:#,###}", Base_Mng.Data.data.Red);
        Assets_Text[2].text = Base_Mng.Data.data.Blue.ToString();
     
        GetSecondText.text = StringMethod.ToCurrencyString(Base_Mng.Data.data.Second_Base) + "/Sec";
        NextLevelText.text = StringMethod.ToCurrencyString(Base_Mng.Data.data.NextLevel_Base);

        NameText.text = Base_Mng.Data.data.UserName;
        OnActionEvent?.Invoke();
        TextColorCheck();
    }

    public Color[] colors;
    public Image NextLevelImage;
    public void TextColorCheck()
    {
        NextLevelText.color = Base_Mng.Data.data.Yellow >= Base_Mng.Data.data.NextLevel_Base ? colors[0] : colors[1];
        NextLevelImage.color = Base_Mng.Data.data.Yellow >= Base_Mng.Data.data.NextLevel_Base ?
            new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0.5f);
    }
}
