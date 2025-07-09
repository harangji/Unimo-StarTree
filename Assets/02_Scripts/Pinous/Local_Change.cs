using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Local_Change : MonoBehaviour
{
    public bool Character = false;
    public bool Motor = false;

    public string Local_Count;
    public string Semi_Data;
    [Header(" Ãß°¡ ¼ÂÆÃ ")]
    public bool Get_Format;
    public string Foramt_Count;
    TextMeshProUGUI T;
    private void Awake()
    {
        T = GetComponent<TextMeshProUGUI>();
        Set_LocalData();
        Localization_Manager.localization += Set_LocalData;
    }
    private void OnDestroy()
    {
        Localization_Manager.localization -= Set_LocalData;
    }
    public void ReturnText(string temp)
    {
        Local_Count = temp;
        Set_LocalData();
    }
    public void Set_LocalData()
    {
        if (Local_Count != "")
        {
            string temp = "";

            string Character_Local = Character ? "Character/" : "UI/";
            if(Motor)
            {
                Character_Local = "Motor/";
            }
            if (Get_Format)
                temp = string.Format(Localization_Manager.local_Data[Character_Local + Local_Count].Get_Data(), Foramt_Count);
            else temp = Localization_Manager.local_Data[Character_Local + Local_Count].Get_Data();
            T.text = temp + Semi_Data;
        }
    }
}
