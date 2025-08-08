using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Offline : UI_Base
{
    public Slider slider;
    double valueCount;
    [SerializeField] private TextMeshProUGUI RewardText; 
    public override void Start()
    {
        Camera_Event.instance.GetCameraEvent(CameraMoveState.Offline);
        float timer = (float)Base_Manager.instance.TimerCheck();
        slider.value = timer / 43200;
        
        valueCount = RewardCalculator.GetYfByAltaLevel() * timer;
        RewardText.text = StringMethod.ToCurrencyString(valueCount);

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
        Canvas_Holder.CloseAllPopupUI();
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
