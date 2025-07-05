using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Setting : UI_Base
{
    public Slider BGMSlider;
    public Slider FXSlider;
    string valueTemp;

    public GameObject LanguageChangePanel;

    public Button OnRestoreButton;

    public GameObject[] CheckBox;
    public override void Start()
    {
        BGMSlider.value = Sound_Manager.BGM_volume;
        FXSlider.value = Sound_Manager.FX_volume;
#if UNITY_IOS
                OnRestoreButton.onClick.AddListener(() => Base_Mng.IAP.RestoreClass());
#endif

        QualityLevelCheck();
        base.Start();
    }

    public void GetQualityLevel(int value)
    {
        QualitySettings.SetQualityLevel(value, true);

        PlayerPrefs.SetInt("QualityLevel", value);
        PlayerPrefs.Save();

        QualityLevelCheck();
    }

    private void QualityLevelCheck()
    {
        var Value = PlayerPrefs.GetInt("QualityLevel");
        for(int i = 0; i < CheckBox.Length; i++)
        {
            CheckBox[i].gameObject.SetActive(false);
        }
        CheckBox[Value].gameObject.SetActive(true);
    }

    public override void Update()
    {
        Sound_Manager.BGM_volume = BGMSlider.value;
        Sound_Manager.FX_volume = FXSlider.value;
        Sound_Manager.instance.SoundCheck();
        return;
        base.Update();
    }


    public GameObject CreditPanel;
    bool GetCreditBoolCheck = false;
    public void GetCredit(bool CreditBool)
    {
        GetCreditBoolCheck = CreditBool;
        CreditPanel.SetActive(CreditBool);

        if (CreditBool)
        {
            CreditPanel.transform.parent = Canvas_Holder.instance.transform;
        }
        else CreditPanel.transform.parent = this.transform;
    }
    public void GetTutorial()
    {
        Canvas_Holder.UI_Holder.Clear();
        Pinous_Flower_Holder.FlowerHolder.Clear();
        WholeSceneController.Instance.ReadyNextScene(3);
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetFloat("BGM", Sound_Manager.BGM_volume);
        PlayerPrefs.SetFloat("FX", Sound_Manager.FX_volume);
    }

    public void GetChangeLanguagePanel(string temp)
    {
        if(temp == PlayerPrefs.GetString("LOCAL"))
        {
            Canvas_Holder.instance.Get_Toast("AllReady");
            return;
        }
        LanguageChangePanel.SetActive(true);
        valueTemp = temp;
    }

    public void NoneChange()
    {
        LanguageChangePanel.SetActive(false);
    }

    public void YesChange()
    {
        PlayerPrefs.SetString("LOCAL", valueTemp);

        Application.Quit();
    }

    public void GetURL(string temp) => Application.OpenURL(temp);
}
