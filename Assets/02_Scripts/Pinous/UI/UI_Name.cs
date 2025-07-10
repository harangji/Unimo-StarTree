using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class UI_Name : UI_Base
{
    [SerializeField] private TextMeshProUGUI NameText;
    [SerializeField] private TMP_InputField NameInputField;
    [SerializeField] private TextMeshProUGUI CountText;

    public override bool Init()
    {
        NameText.text = Base_Manager.Data.UserData.UserName;
        CountText.color = Base_Manager.Data.UserData.Blue >= 50 ? Color.green : Color.red;
        
        if (Localization_Manager.LocalAccess == "kr")
            NameInputField.characterLimit = 10;
        else NameInputField.characterLimit = 20;

        return base.Init();
    }

    public void GetNameChange()
    {
        if(Base_Manager.Data.UserData.Blue < 50)
        {
            Canvas_Holder.instance.Get_Toast("NM");
            return;
        }
        Base_Manager.Data.UserData.Blue -= 50;
        Base_Manager.Data.UserData.UserName = NameInputField.text;
        Main_UI.instance.Text_Check();
        Canvas_Holder.instance.Get_Toast("NickName");
        DisableOBJ();
    }
} 
