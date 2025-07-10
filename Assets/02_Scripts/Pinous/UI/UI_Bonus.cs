using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Bonus : UI_Base
{
    [SerializeField] private TextMeshProUGUI RewardText;

    public override bool Init()
    {
        RewardText.text = StringMethod.ToCurrencyString(Base_Manager.Data.UserData.Second_Base * 7200);
        return base.Init();
    }

    public void GetReward()
    {
        Base_Manager.ADS.ShowRewardedAds(() =>
        {
            Base_Manager.Data.UserData.Yellow += Base_Manager.Data.UserData.Second_Base * 7200;
            Main_UI.instance.Text_Check();
            Base_Manager.Data.UserData.BonusRewardCount = 0.0f;
            Canvas_Holder.instance.Get_Toast("Reward");
            DisableOBJ();

            Base_Manager.Data.SaveUserData();
        });
    }
}
