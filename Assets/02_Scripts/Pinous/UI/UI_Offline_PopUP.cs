using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Offline_PopUP : UI_Base
{
    double valueCount;
    [SerializeField] private TextMeshProUGUI RewardText;
    [SerializeField] private TextMeshProUGUI TimerText;
    public override void Start()
    {
        Camera_Event.instance.GetCameraEvent(CameraMoveState.Offline);

        float value = (float)Base_Manager.instance.TimerCheck();
        valueCount = Base_Manager.Data.UserData.Second_Base * Base_Manager.instance.TimerCheck();
        RewardText.text = StringMethod.ToCurrencyString(valueCount);

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
        var dataValue = valueCount;
        Base_Manager.Data.UserData.Yellow += dataValue;
        Base_Manager.Data.UserData.E_DateTime = Base_Manager.Data.UserData.S_DateTime;
        Canvas_Holder.instance.Get_Toast("Reward");
        DisableOBJ();
    }

    public void RewardCollectButton()
    {
        var dataValue = valueCount * 2.0f;

        Base_Manager.ADS.ShowRewardedAds(() =>
        {
            Base_Manager.Data.UserData.Yellow += dataValue;
            Base_Manager.Data.UserData.E_DateTime = Base_Manager.Data.UserData.S_DateTime;
            Canvas_Holder.instance.Get_Toast("Reward");

            Canvas_Holder.CloseAllPopupUI();
        });
    }
}
