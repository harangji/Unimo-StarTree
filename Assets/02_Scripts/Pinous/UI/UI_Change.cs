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
        OrangeSlider.maxValue = 100000;
        YellowSlider.maxValue = 100000;
        // OrangeSlider.maxValue = (float)blueCount;
        // YellowSlider.maxValue = (float)blueCount;
    }

    void TextCheck()
    {
        Blue.text = StringMethod.ToCurrencyString(BlueStarCount);
        Blue.color = Base_Manager.Data.UserData.Blue >= BlueStarCount ? Color.green : Color.red;
        Orange.text = StringMethod.ToCurrencyString(OrangeStarCount);
        Yellow.text = StringMethod.ToCurrencyString(YellowStarCount);
    }

    void BlueCount()
    {
        BlueStarCount = (OrangeStarCount / 500) + (YellowStarCount / 2500);
    }

    // Orange
    void ValueChangeSlider_Orange(float value)
    {
        // �����̴� �� = ����Ϸ��� Blue ��
        BlueStarCount = Mathf.FloorToInt(value); // ���� ����

        // Orange = Blue �� 500 (������ ��ȯ ����)
        OrangeStarCount = BlueStarCount * 500;

        // YellowSlider.value = 0;
        // YellowStarCount = 0;

        BlueCount();
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
        // TextCheck();
    }

    // Yellow
    void ValueChangeSlider_Yellow(float value)
    {
        BlueStarCount = Mathf.FloorToInt(value);
        
        // �ӽ÷� 2500 ���� ���� �س���.
        YellowStarCount = BlueStarCount * 2500;
        
        // var index = OrangeStarCount / 500;
        // var DetectedYellow = Base_Manager.Data.UserData.Blue - index;
        // if(value >= DetectedYellow)
        // {
        //     value = (float)DetectedYellow;
        //     YellowSlider.value = value;
        // }
        // YellowStarCount = (int)(value * Base_Manager.Data.UserData.Second_Base);
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
