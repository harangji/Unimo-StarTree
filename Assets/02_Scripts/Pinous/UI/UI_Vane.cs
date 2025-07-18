using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Vane : UI_Base
{
    public Slider[] sliders;
    public TextMeshProUGUI[] Text_Timers;
    public GameObject[] Locks;
    public override void Start()
    {
        Camera_Event.instance.GetCameraEvent(CameraMoveState.Vane);
        base.Start();
    }

    public override void Update()
    {
        for (int i = 0; i < 3; i++)
        {
            if (Base_Manager.Data.UserData.BuffFloating[i] >= 0.0f)
            {
                Base_Manager.Data.UserData.BuffFloating[i] -= Time.deltaTime;
                Text_Timers[i].text = ShowTimer(Base_Manager.Data.UserData.BuffFloating[i]);
                sliders[i].value = Base_Manager.Data.UserData.BuffFloating[i] / 1800.0f;
            }
            else
            {
                ReturnTimer(i);
            }
        }
        base.Update();
    }

    public override void DisableOBJ()
    {
        base.DisableOBJ();
    }

    public void RewardVane(int value)
    {
        Base_Manager.ADS.ShowRewardedAds(() => GetTimer(value));
    }

    public void GetTimer(int value)
    {
        Canvas_Holder.instance.Get_Toast("ADS");
        Locks[value].SetActive(true);
        Base_Manager.Data.UserData.BuffFloating[value] = 1800.0f;
        sliders[value].gameObject.SetActive(true);
        Base_Manager.Data.SaveUserData();
    }

    public void ReturnTimer(int value)
    {
        Locks[value].SetActive(false);
        Base_Manager.Data.UserData.BuffFloating[value] = 0.0f;
        sliders[value].gameObject.SetActive(false);
        Base_Manager.Data.SaveUserData();
    }

    public static string ShowTimer(double timer)
    {
        TimeSpan t = TimeSpan.FromSeconds(Convert.ToDouble(timer));

        string answer = string.Format("{0:D2}:{1:D2}",

                              t.Minutes,

                              t.Seconds);

        return answer;
    }
}
