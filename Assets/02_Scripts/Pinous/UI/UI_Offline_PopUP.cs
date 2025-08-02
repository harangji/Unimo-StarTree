using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Offline_PopUP : UI_Base
{
    double yfValueCount, ofValueCount;
    [SerializeField] private TextMeshProUGUI[] RewardText;
    [SerializeField] private TextMeshProUGUI TimerText;
    public override void Start()
    {
        Camera_Event.instance.GetCameraEvent(CameraMoveState.Offline);

        float value = (float)Base_Manager.instance.TimerCheck();
        // valueCount = Base_Manager.Data.UserData.Second_Base * Base_Manager.instance.TimerCheck();
        yfValueCount = RewardCalculator.GetYfByAltaLevel() * Base_Manager.instance.TimerCheck();
        ofValueCount = RewardCalculator.GetOfByAltaLevel() * Base_Manager.instance.TimerCheck();
        RewardText[0].text = StringMethod.ToCurrencyString(yfValueCount);
        RewardText[1].text = StringMethod.ToCurrencyString(ofValueCount);

        TimeSpan span = TimeSpan.FromSeconds(value);
        TimerText.text = span.Hours + " h " + span.Minutes + " m " + "(Max 12H)";

        base.Start();
    }

    public override void DisableOBJ()
    {
        base.DisableOBJ();
    }

    public void CollectButton()
    {
        // var dataValue = yfValueCount;
        Base_Manager.Data.UserData.Yellow += yfValueCount;
        Base_Manager.Data.UserData.Red += ofValueCount;
        Base_Manager.Data.UserData.E_DateTime = Base_Manager.Data.UserData.S_DateTime;
        Canvas_Holder.instance.Get_Toast("Reward");
        DisableOBJ();
    }

    public void RewardCollectButton()
    {
        // var dataValue = yfValueCount * 2.0f;

        Base_Manager.ADS.ShowRewardedAds(() =>
        {
            Base_Manager.Data.UserData.Yellow += (yfValueCount * 2);
            Base_Manager.Data.UserData.Red += (ofValueCount * 2);
            Base_Manager.Data.UserData.E_DateTime = Base_Manager.Data.UserData.S_DateTime;
            Canvas_Holder.instance.Get_Toast("Reward");

            Canvas_Holder.CloseAllPopupUI();
        });
    }
}
