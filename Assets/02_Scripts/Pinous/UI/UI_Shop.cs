using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class UI_Shop : UI_Base
{
    public GameObject[] ADSObjects;
    public GameObject[] Characters;
    public GameObject[] EQs;
    int[] Character_valueCounts = { 5, 9, 7, 1, 4, 3, 11, 8 };
    int[] Eq_valueCounts = { 19, 17, 20, 16 };
    public TextMeshProUGUI[] texts;

    public GameObject GetPurchasePanel;
    Asset_State state;
    public int ValueCount;

    public UnityEngine.UI.Image PurchaseImage;
    public TextMeshProUGUI PurchaseText;
    Action rewardAction;
    
    public override bool Init()
    {
        CheckShop();
        texts[0].text = StringMethod.ToCurrencyString(Base_Manager.Data.UserData.Yellow);
        texts[1].text = string.Format("{0:#,###}", Base_Manager.Data.UserData.Red);
        texts[2].text = Base_Manager.Data.UserData.Blue.ToString();
        return base.Init();
    }

    public override void Update()
    {
        texts[0].text = StringMethod.ToCurrencyString(Base_Manager.Data.UserData.Yellow);
        texts[1].text = string.Format("{0:#,###}", Base_Manager.Data.UserData.Red);
        texts[2].text = Base_Manager.Data.UserData.Blue.ToString();
        return;
        base.Update();
    }

    public void GetPurchase(bool GetPurchase, Asset_State stateAsset, int value, Action reward)
    {
        GetPurchasePanel.SetActive(GetPurchase);
        if (!GetPurchase)
        {
            return;
        }
        state = stateAsset;
        ValueCount = value;

        PurchaseImage.sprite = Data_Manager.atlas.GetSprite(state.ToString());
        PurchaseText.text = ValueCount.ToString();

        rewardAction = reward;
    }

    public void GetPurchaseYes()
    {
        if(rewardAction != null)
        {
            rewardAction?.Invoke();
            rewardAction = null;
        }
        GetPurchasePanel.SetActive(false);
    }

    public void NoneButton()
    {
        if (rewardAction != null) rewardAction = null;
        GetPurchasePanel.SetActive(false);
    }


    private void CheckShop()
    {
        if(Base_Manager.Data.UserData.ADSBuy)
        {
            for(int i = 0; i< ADSObjects.Length; i++)
            {
                ADSObjects[i].transform.GetChild(4).gameObject.SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < ADSObjects.Length; i++) ADSObjects[i].transform.GetChild(4).gameObject.SetActive(false);
        }

        for (int i = 0; i < Characters.Length; i++)
        {
            if (Base_Manager.Data.UserData.HasCharacterData[Character_valueCounts[i]])
            {
                Characters[i].transform.GetChild(0).gameObject.SetActive(true);
                Characters[i].transform.GetComponent<Button>().onClick.RemoveAllListeners();
            }
            else
            {
                Characters[i].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        for(int i = 0; i < EQs.Length; i++)
        {
            if (Base_Manager.Data.UserData.HasEnginData[Eq_valueCounts[i]])
            {
                EQs[i].transform.GetChild(0).gameObject.SetActive(true);
                EQs[i].transform.GetComponent<Button>().onClick.RemoveAllListeners();
            }
            else
            {
                EQs[i].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }
    public void GetProduct(string name)
    {
        Base_Manager.Analytics.RecordCustomEventWithParameters(name, 1);
        Base_Manager.IAP.Purchase(name);
    }

    public void GetBuyAsset(int value)
    {
        Base_Manager.Analytics.RecordSaleItemForInGame("InGame");

        // 해당 캐릭터 데이터를 이미 보유하고 있다면 되돌아가는 return 문으로 추정.
        if (Base_Manager.Data.UserData.HasCharacterData[value]) return;
        // 가지고 있는 파랑재화가 300보다 낮으면
        if (Base_Manager.Data.UserData.Blue < 300)
        {
            // 재화 부족 문구 뜨면서 되돌아가기.
            Canvas_Holder.instance.Get_Toast("NM");

            return;
        }
        GetPurchase(true, Asset_State.Blue, 300, () =>
        {
            Base_Manager.Data.UserData.Blue -= 300;
            Canvas_Holder.instance.NoneClose = true;
            Canvas_Holder.instance.GetUI("##Reward");
            Canvas_Holder.UI_Holder.Peek().transform.parent = Canvas_Holder.instance.transform;
            Canvas_Holder.UI_Holder.Peek().GetComponent<UI_Reward>().GetRewardInit(RewardState.Character, CharCostumer.Charcater, 0, value, 1);
            CheckShop();
        });
        // if (Base_Manager.Data.UserData.Red < 100000)
        // {
        //     Canvas_Holder.instance.Get_Toast("NM");
        //
        //     return;
        // }
        // GetPurchase(true, Asset_State.Red, 100000, () =>
        // {
        //     Base_Manager.Data.UserData.Red -= 100000;
        //     Canvas_Holder.instance.NoneClose = true;
        //     Canvas_Holder.instance.GetUI("##Reward");
        //     Canvas_Holder.UI_Holder.Peek().transform.parent = Canvas_Holder.instance.transform;
        //     Canvas_Holder.UI_Holder.Peek().GetComponent<UI_Reward>().GetRewardInit(RewardState.Character, CharCostumer.Charcater, 0, value, 1);
        //     CheckShop();
        // });
    }
    public void GetBuyAsset02(int value)
    {
        Base_Manager.Analytics.RecordSaleItemForInGame("InGame");

        if (Base_Manager.Data.UserData.HasCharacterData[value]) return;
        if (Base_Manager.Data.UserData.Blue < 150)
        {
            Canvas_Holder.instance.Get_Toast("NM");

            return;
        }
        GetPurchase(true, Asset_State.Blue, 150, () =>
        {
            Base_Manager.Data.UserData.Blue -= 150;
            Canvas_Holder.instance.NoneClose = true;
            Canvas_Holder.instance.GetUI("##Reward");
            Canvas_Holder.UI_Holder.Peek().transform.parent = Canvas_Holder.instance.transform;
            Canvas_Holder.UI_Holder.Peek().GetComponent<UI_Reward>().GetRewardInit(RewardState.Character, CharCostumer.Charcater, 0, value, 1);
            CheckShop();
        });
        // if (Base_Manager.Data.UserData.Red < 75000)
        // {
        //     Canvas_Holder.instance.Get_Toast("NM");
        //
        //     return;
        // }
        // GetPurchase(true, Asset_State.Red, 75000, () =>
        // {
        //     Base_Manager.Data.UserData.Red -= 75000;
        //     Canvas_Holder.instance.NoneClose = true;
        //     Canvas_Holder.instance.GetUI("##Reward");
        //     Canvas_Holder.UI_Holder.Peek().transform.parent = Canvas_Holder.instance.transform;
        //     Canvas_Holder.UI_Holder.Peek().GetComponent<UI_Reward>().GetRewardInit(RewardState.Character, CharCostumer.Charcater, 0, value, 1);
        //     CheckShop();
        // });
    }
    public void GetBuyAsset03(int value)
    {
        Base_Manager.Analytics.RecordSaleItemForInGame("InGame");

        if (Base_Manager.Data.UserData.HasCharacterData[value]) return;
        if (Base_Manager.Data.UserData.Red < 150000)
        {
            Canvas_Holder.instance.Get_Toast("NM");

            return;
        }
        GetPurchase(true, Asset_State.Red, 150000, () =>
        {
            Base_Manager.Data.UserData.Red -= 150000;
            Canvas_Holder.instance.NoneClose = true;
            Canvas_Holder.instance.GetUI("##Reward");
            Canvas_Holder.UI_Holder.Peek().transform.parent = Canvas_Holder.instance.transform;
            Canvas_Holder.UI_Holder.Peek().GetComponent<UI_Reward>().GetRewardInit(RewardState.Character, CharCostumer.Charcater, 0, value, 1);
            CheckShop();
        });
    }

    public void GetBuyAssetDown(int value)
    {
        Base_Manager.Analytics.RecordSaleItemForInGame("InGame");

        if (Base_Manager.Data.UserData.HasCharacterData[value]) return;

        if (Base_Manager.Data.UserData.Red < 50000)
        {
            Canvas_Holder.instance.Get_Toast("NM");

            return;
        }
        GetPurchase(true, Asset_State.Red, 50000, () =>
        {
            Base_Manager.Data.UserData.Red -= 50000;
            Canvas_Holder.instance.NoneClose = true;
            Canvas_Holder.instance.GetUI("##Reward");
            Canvas_Holder.UI_Holder.Peek().transform.parent = Canvas_Holder.instance.transform;
            Canvas_Holder.UI_Holder.Peek().GetComponent<UI_Reward>().GetRewardInit(RewardState.Character, CharCostumer.Charcater, 0, value, 1);
            CheckShop();
        });
    }
    public void GetBuyAssetEQ(int value)
    {
        Base_Manager.Analytics.RecordSaleItemForInGame("InGame");

        if (Base_Manager.Data.UserData.HasEnginData[value]) return;

        if (Base_Manager.Data.UserData.Blue < 150)
        {
            Canvas_Holder.instance.Get_Toast("NM");

            return;
        }
        GetPurchase(true, Asset_State.Blue, 150, () =>
        {
            Base_Manager.Data.UserData.Blue -= 150;
            Canvas_Holder.instance.NoneClose = true;
            Canvas_Holder.instance.GetUI("##Reward");
            Canvas_Holder.UI_Holder.Peek().transform.parent = Canvas_Holder.instance.transform;
            Canvas_Holder.UI_Holder.Peek().GetComponent<UI_Reward>().GetRewardInit(RewardState.Character, CharCostumer.EQ, 0, 1, value);
            CheckShop();
        });
        
        // if (Base_Manager.Data.UserData.Red < 50000)
        // {
        //     Canvas_Holder.instance.Get_Toast("NM");
        //
        //     return;
        // }
        // GetPurchase(true, Asset_State.Red, 50000, () =>
        // {
        //     Base_Manager.Data.UserData.Red -= 50000;
        //     Canvas_Holder.instance.NoneClose = true;
        //     Canvas_Holder.instance.GetUI("##Reward");
        //     Canvas_Holder.UI_Holder.Peek().transform.parent = Canvas_Holder.instance.transform;
        //     Canvas_Holder.UI_Holder.Peek().GetComponent<UI_Reward>().GetRewardInit(RewardState.Character, CharCostumer.EQ, 0, 1, value);
        //     CheckShop();
        // });
    }
    public void GetBuyAssetEQ02(int value)
    {
        Base_Manager.Analytics.RecordSaleItemForInGame("InGame");

        if (Base_Manager.Data.UserData.HasEnginData[value]) return;

        if (Base_Manager.Data.UserData.Red < 25000)
        {
            Canvas_Holder.instance.Get_Toast("NM");

            return;
        }
        GetPurchase(true, Asset_State.Red, 25000, () =>
        {
            Base_Manager.Data.UserData.Red -= 25000;
            Canvas_Holder.instance.NoneClose = true;
            Canvas_Holder.instance.GetUI("##Reward");
            Canvas_Holder.UI_Holder.Peek().transform.parent = Canvas_Holder.instance.transform;
            Canvas_Holder.UI_Holder.Peek().GetComponent<UI_Reward>().GetRewardInit(RewardState.Character, CharCostumer.EQ, 0, 1, value);
            CheckShop();
        });
    }
}
