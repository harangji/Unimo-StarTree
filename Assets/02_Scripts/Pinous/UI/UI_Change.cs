using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Change : UI_Base
{
    int BlueStarCount, OrangeStarCount, YellowStarCount;

    public Slider OrangeSlider, YellowSlider;
    public TextMeshProUGUI Blue, Orange, Yellow;
    int blueCount;
    float orange, yellow;
    public override void Start()
    {
        blueCount = (int)Base_Manager.Data.UserData.Blue;

        Camera_Event.instance.GetCameraEvent(CameraMoveState.TeaPot);

        CountCheck();

        OrangeSlider.onValueChanged.AddListener(ValueChangeSlider_Orange);
        YellowSlider.onValueChanged.AddListener(ValueChangeSlider_Yellow);
        TextCheck();

        base.Start();
    }

    private void CountCheck()
    {
        OrangeSlider.maxValue = (float)blueCount;
        YellowSlider.maxValue = (float)blueCount;
    }

    void TextCheck()
    {
        Blue.text = BlueStarCount.ToString();
        Blue.color = Base_Manager.Data.UserData.Blue >= BlueStarCount ? Color.green : Color.red;
        Orange.text = OrangeStarCount.ToString();
        Yellow.text = StringMethod.ToCurrencyString(YellowStarCount);
    }

    void BlueCount()
    {
        BlueStarCount = (OrangeStarCount / 500) + (int)(YellowStarCount / Base_Manager.Data.UserData.Second_Base);
    }

    // Orange
    void ValueChangeSlider_Orange(float value)
    {
        // 슬라이더 값 = 사용하려는 Blue 수
        BlueStarCount = Mathf.FloorToInt(value); // 정수 보장

        // Orange = Blue × 150 (정해진 교환 비율)
        OrangeStarCount = BlueStarCount * 150;

        // YellowSlider.value = 0;
        // YellowStarCount = 0;

        TextCheck();
        
        // var index = YellowStarCount / Base_Manager.Data.UserData.Second_Base;
        // var DetectedYellow = Base_Manager.Data.UserData.Blue - index;
        // if (value >= DetectedYellow)
        // {
        //     value = (float)DetectedYellow;
        //     OrangeSlider.value = value;
        // }
        //
        // OrangeStarCount = (int)(value * 500);
        // BlueCount();
        // TextCheck();
    }

    // Yellow
    void ValueChangeSlider_Yellow(float value)
    {
        var index = OrangeStarCount / 500;
        var DetectedYellow = Base_Manager.Data.UserData.Blue - index;
        if(value >= DetectedYellow)
        {
            value = (float)DetectedYellow;
            YellowSlider.value = value;
        }
        YellowStarCount = (int)(value * Base_Manager.Data.UserData.Second_Base);
        BlueCount();
        TextCheck();
    }
    public override void DisableOBJ()
    {
        base.DisableOBJ();
    }
    public void GetExchange()
    {
        if(BlueStarCount == 0)
        {
            Canvas_Holder.instance.Get_Toast("NoneBlue");
            return;
        }
        if(Base_Manager.Data.UserData.Blue >= BlueStarCount)
        {
            Base_Manager.Data.UserData.Blue -= BlueStarCount;
            Base_Manager.Data.UserData.Red += OrangeStarCount;
            Base_Manager.Data.UserData.Yellow += YellowStarCount;
            Main_UI.instance.Text_Check();
            Canvas_Holder.instance.Get_Toast("SuccessChange");

            DisableOBJ();
        }
        else
        {
            Canvas_Holder.instance.Get_Toast("NM");
        }
    }
}
