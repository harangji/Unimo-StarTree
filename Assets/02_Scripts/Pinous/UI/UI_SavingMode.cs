using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UI_SavingMode : UI_Base
{
    public TextMeshProUGUI BatteryText;
    public Image BatteryFill;

    public TextMeshProUGUI TimeText;
    public RectTransform SavingKingPos;
    public Slider slider;
    public TextMeshProUGUI GetYellowText;

    public static Camera camera;

    private void Start()
    {
        camera = Camera.main;
        Base_Manager.SavingMode = true;
        Base_Manager.Savingyellow = 0.0d;
        Sound_Manager.instance.ReturnSound(false);

        transform.parent = Canvas_Holder.instance.transform;
        camera.enabled = false;
    }

    private void Update()
    {
        BatteryText.text = (SystemInfo.batteryLevel * 100.0f).ToString() + "%";
        BatteryFill.fillAmount = SystemInfo.batteryLevel;
        GetYellowText.text = StringMethod.ToCurrencyString(Base_Manager.Savingyellow);
        TimeText.text = System.DateTime.Now.ToString("HH:mm");
        slider.value = SavingKingPos.anchoredPosition.x / 900.0f;
    }

    public static void GetReturn()
    {
        camera.enabled = true;
        OnDemandRendering.renderFrameInterval = 1;
        Base_Manager.SavingMode = false;

        Sound_Manager.instance.ReturnSound(true);

        Canvas_Holder.CloseAllPopupUI();
    }
}
