using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Mission_Base : MonoBehaviour
{
    public Mission_State state;
    public TextMeshProUGUI m_Title, m_Count, m_RewardText;
    public Slider m_Slider;
    public Image m_Reward, m_TitleImage;
    public Button button;
    public GameObject InProgress; // 미션 완료 완료 표시 UI (Text)
    public CharCostumer m_CharCostumer;
    public int index;

    public void Init(string style, int count, int reward, Asset_State rewardType, int rewardValue=0)
    {
        m_Slider.value = (float)valueCount(style) / (float)count;
        m_Count.text = valueCount(style).ToString() + "/" + count.ToString();

        button.onClick.RemoveAllListeners();
        
        if (state == Mission_State.Daily)
        {
            m_RewardText.text = reward.ToString();
            Debug.Log(rewardType);
            m_Reward.sprite = Data_Manager.atlas.GetSprite(rewardType.ToString());
            GetCheck(style);

            button.onClick.AddListener(() => GetButtonCheck(style, count, reward, rewardType));
        }
        else
        {
            if (rewardType == Asset_State.Yellow || rewardType == Asset_State.Red || rewardType == Asset_State.Blue)
            {
                // 별꿀 보상 업적
                m_RewardText.text = rewardValue.ToString();
                // m_Reward.sprite = Data_Manager.atlas.GetSprite(rewardType.ToString());
                button.onClick.AddListener(() => GetButtonRewardHoney(style, count, reward, rewardType, rewardValue));
                GetCheckAchieve(reward);
            }
            else
            {
                // 기존 캐릭터/장비 보상 업적
                button.onClick.AddListener(() => GetButtonTrophy(style, count, reward));
                GetCheckAchieve(reward);
            }
        }
    }
    
    private void GetButtonRewardHoney(string style, int count, int reward, Asset_State rewardType, int rewardValue)
    {
        if (valueCount(style) < count) return;

        // Base_Manager.Analytics.RecordCustomEventWithParameters("Trophy Mission Completed", reward);

        Base_Manager.Data.UserData.GetArchivements[reward] = true;

        Canvas_Holder.instance.NoneClose = true;
        Canvas_Holder.instance.GetUI("##Reward");
        Canvas_Holder.UI_Holder.Peek().transform.parent = Canvas_Holder.instance.transform;

        Canvas_Holder.UI_Holder.Peek().GetComponent<UI_Reward>().GetRewardInit(
            RewardState.Other,
            rewardType,
            rewardValue
        );

        GetCheckAchieve(reward);
    }
    
    private void GetButtonTrophy(string style, int count, int reward)
    {
        if (valueCount(style) < count) return;

        // Base_Manager.Analytics.RecordCustomEventWithParameters("Trophy Mission Completed", reward);

        Base_Manager.Data.UserData.GetArchivements[reward] = true;
        Canvas_Holder.instance.NoneClose = true;
        Canvas_Holder.instance.GetUI("##Reward");
        Canvas_Holder.UI_Holder.Peek().transform.parent = Canvas_Holder.instance.transform;
        if (m_CharCostumer == CharCostumer.Charcater)
            Canvas_Holder.UI_Holder.Peek().GetComponent<UI_Reward>().GetRewardInit(RewardState.Character, CharCostumer.Charcater, 0, index, 1);
        else Canvas_Holder.UI_Holder.Peek().GetComponent<UI_Reward>().GetRewardInit(RewardState.Character, CharCostumer.EQ, 0, 1, index);

        GetCheckAchieve(reward);
    }
    private void GetButtonCheck(string style, int count, int reward, Asset_State rewardType)
    {
        Debug.Log("들어옴 ?");
        if (valueCount(style) < count) return;
        // Base_Manager.Analytics.RecordCustomEventWithParameters("Trophy Mission Completed", reward);
        
        Debug.Log("들어왔음 ?");
        switch (style)
        {
            case "GamePlay": Base_Manager.Data.UserData.GetGamePlay = true; break;
            case "ADS": Base_Manager.Data.UserData.GetADS = true; break;
            case "Touch": Base_Manager.Data.UserData.GetTouch = true; break;
            case "DailyAccount": Base_Manager.Data.UserData.GetDaily = true; break;
            case "TimeItem": Base_Manager.Data.UserData.GetTimeItem = true; break;
            case "RePlay": Base_Manager.Data.UserData.GetRePlay = true; break;
        }

        Canvas_Holder.instance.NoneClose = true;
        Canvas_Holder.instance.GetUI("##Reward");
        Canvas_Holder.UI_Holder.Peek().transform.parent = Canvas_Holder.instance.transform;
        Canvas_Holder.UI_Holder.Peek().GetComponent<UI_Reward>().GetRewardInit(RewardState.Other, rewardType, reward);
        // Canvas_Holder.UI_Holder.Peek().GetComponent<UI_Reward>().GetRewardInit(RewardState.Other, CharCostumer.Charcater, reward);

        GetCheck(style);

    }

    private void GetCheckAchieve(int value)
    {
        Debug.Log($"{value} ::: 몇 번째에서 끊기는거야 도대체 ?");
        var Base = Base_Manager.Data.UserData.GetArchivements[value];
        button.gameObject.SetActive(Base ? false : true);
        InProgress.SetActive(Base ? true : false);
    }

    private void GetCheck(string style)
    {
        switch(style)
        {
            case "GamePlay": 
                button.gameObject.SetActive(Base_Manager.Data.UserData.GetGamePlay ? false : true);
                InProgress.SetActive(Base_Manager.Data.UserData.GetGamePlay ? true : false);
                break;
            case "ADS":
                button.gameObject.SetActive(Base_Manager.Data.UserData.GetADS ? false : true);
                InProgress.SetActive(Base_Manager.Data.UserData.GetADS ? true : false);
                break;
            case "Touch":
                button.gameObject.SetActive(Base_Manager.Data.UserData.GetTouch ? false : true);
                InProgress.SetActive(Base_Manager.Data.UserData.GetTouch ? true : false);
                break;
            case "DailyAccount":
                button.gameObject.SetActive(Base_Manager.Data.UserData.GetDaily ? false : true);
                InProgress.SetActive(Base_Manager.Data.UserData.GetDaily ? true : false);
                break;
            case "TimeItem":
                button.gameObject.SetActive(Base_Manager.Data.UserData.GetTimeItem ? false : true);
                InProgress.SetActive(Base_Manager.Data.UserData.GetTimeItem ? true : false);
                break;
            case "RePlay":
                button.gameObject.SetActive(Base_Manager.Data.UserData.GetRePlay ? false : true);
                InProgress.SetActive(Base_Manager.Data.UserData.GetRePlay ? true : false);
                break;
        }
    }

    

    public int valueCount(string style)
    {
        switch(style)
        {
            case "GamePlay": return Base_Manager.Data.UserData.GamePlay;
            case "ADS": return Base_Manager.Data.UserData.ADS;
            case "ADSNONE": return Base_Manager.Data.UserData.ADSNoneReset;
            case "Touch": return Base_Manager.Data.UserData.Touch;
            case "Level": return Base_Manager.Data.UserData.Level + 1;
            case "DailyAccount": return Base_Manager.Data.UserData.DailyAccount;
            case "TimeItem":  return Base_Manager.Data.UserData.TimeItem;
            case "RePlay": return Base_Manager.Data.UserData.RePlay;
            case "IAP": return Base_Manager.Data.UserData.IAP;
            case "Collection":
                int a = 0;
                for(int i = 0; i < Base_Manager.Data.UserData.HasCharacterData.Length; i++)
                {
                    if (Base_Manager.Data.UserData.HasCharacterData[i] == true)
                    {
                        a++;
                    }
                }
                return a;
            case "Collection_EQ":
                int b = 0;
                for(int i = 0; i < Base_Manager.Data.UserData.HasEnginData.Length; i++)
                {
                    if (Base_Manager.Data.UserData.HasEnginData[i] == true)
                    {
                        b++;
                    }
                }
                return b;
            case "BestStage": return Base_Manager.Data.UserData.BestStage;
            case "FacilityLevelSum": return Base_Manager.Data.UserData.FacilityLevelSum;
            case "Reinforce": return Base_Manager.Data.UserData.ReinforceCountTotal;
        }
        return -1;
    }

    public void ClaimButton()
    {

    }
}
