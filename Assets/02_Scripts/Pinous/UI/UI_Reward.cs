using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum RewardState
{
    Other,
    Character
}
public class UI_Reward : UI_Base
{
    public GameObject OtherRewardPanel;
    public GameObject CharRewardPanel;
    public TextMeshProUGUI GetRewardText; 

    public override void Start()
    {
        Sound_Manager.instance.Play(Sound.Effect, "effect_16");
        GetRewardText.text = Localization_Manager.local_Data["Popup/Reward"].Get_Data();
        base.Start();
    }

    public void GetIAPReward(string name)
    {
        if(Base_Manager.Data.UserData.IAP == 0)
        {
            Base_Manager.Analytics.RecordCustomEventWithParameters("First Buy", (Base_Manager.Data.UserData.Level + 1));
        }
        else
        {
            Base_Manager.Analytics.RecordCustomEventWithParameters("Duplicate Buy", 1);
        }
        Base_Manager.Analytics.RecordSaleItemForPurchase(name);

        switch (name)
        {
            case "removeads": 
                if(Base_Manager.Data.UserData.ADSBuy == false)
                GetRewardInit(RewardState.Other, CharCostumer.Charcater, 50, 0, 0);
                Base_Manager.Data.UserData.ADSBuy = true;
                Base_Manager.ADS.BannerDestroy();
                Main_UI.instance.RectADSCheck();
                break;
            case "revemoads_all": 
                if(Base_Manager.Data.UserData.ADSBuy == false)
                GetRewardInit(RewardState.Other, CharCostumer.Charcater, 400, 0, 0);
                Base_Manager.Data.UserData.ADSBuy = true;
                Base_Manager.ADS.BannerDestroy();
                Main_UI.instance.RectADSCheck();
                break;
            case "bluehoney_1500": GetRewardInit(RewardState.Other, CharCostumer.Charcater, 40, 0, 0); break;
            case "bluehoney_3300": GetRewardInit(RewardState.Other, CharCostumer.Charcater, 95, 0, 0); break;
            case "bluehoney_6900": GetRewardInit(RewardState.Other, CharCostumer.Charcater, 200, 0, 0); break;
            case "bluehoney_9900": GetRewardInit(RewardState.Other, CharCostumer.Charcater, 300, 0, 0); break;
            case "bluehoney_29000": GetRewardInit(RewardState.Other, CharCostumer.Charcater, 900, 0, 0); break;
            case "bluehoney_49000": GetRewardInit(RewardState.Other, CharCostumer.Charcater, 1500, 0, 0); break;
            case "character_cu": GetRewardInit(RewardState.Character, CharCostumer.Charcater, 0, 5, 1); break;
            case "character_primo": GetRewardInit(RewardState.Character, CharCostumer.Charcater, 0, 9, 1); break;
            case "eq_bath": GetRewardInit(RewardState.Character, CharCostumer.EQ, 0, 1, 19); break;
        }
    }
    public void GetRewardInit(RewardState state, CharCostumer costume,int cnt = 0, int charIdx = 1, int eqIdx = 1)
    {
        switch(state)
        {
            case RewardState.Other:
                OtherRewardPanel.SetActive(true);
                TextMeshProUGUI texts = OtherRewardPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                Base_Manager.Data.UserData.Blue += cnt;
                texts.text = "x" + cnt.ToString();
                break;
            case RewardState.Character:
                CharRewardPanel.SetActive(true);
                CharCostumer costumer = costume;
                switch(costumer)
                {
                    case CharCostumer.Charcater:
                        Debug.Log(charIdx + " : " + Base_Manager.Data.UserData.GetCharacterData.Length);
                        Base_Manager.Data.UserData.GetCharacterData[charIdx] = true;
                        if(charIdx < 13)
                        {
                            Base_Manager.Data.UserData.GetEQData[charIdx] = true;
                        }
                        Land.instance.GetCharacter(charIdx);
                        break;
                    case CharCostumer.EQ:
                        Base_Manager.Data.UserData.GetEQData[eqIdx] = true;
                        break;
                }
                CurrentCharReward.instance.Init(costumer, charIdx+1, eqIdx+1);
                break;
        }

        int character = 0;
        int eq = 0;
        for(int i = 0; i < Base_Manager.Data.UserData.GetCharacterData.Length; i++)
        {
            if (Base_Manager.Data.UserData.GetCharacterData[i] == true) character++;
        }
        for(int i = 0; i < Base_Manager.Data.UserData.GetEQData.Length; i++)
        {
            if (Base_Manager.Data.UserData.GetEQData[i] == true) eq++;
        }
        Base_Manager.Analytics.RecordCustomEventWithParameters("Character", character);
        Base_Manager.Analytics.RecordCustomEventWithParameters("EQ", eq);
    }

    public override void DisableOBJ()
    {
        Canvas_Holder.instance.Get_Toast("Reward");
        DisableCheck(false);
        return;
        base.DisableOBJ();
    }
}
