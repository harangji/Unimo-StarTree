using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Localization_Mng
{
    public class String_Data
    {
        public string kr;
        public string en;
        public string es;

        public string Get_Data(string lan = "")
        {
            if (lan != "")
            {
                switch (lan)
                {
                    case "kr": return kr;
                    case "en": return en;
                    case "es": return es;
                }
                return "";
            }
            switch (LocalAccess)
            {
                case "kr": return kr;
                case "en": return en;
                case "es": return es;
            }
            return "";
        }
    }
    public static string LocalAccess = "kr";

    private static List<Dictionary<string, object>> Localization_Data = new List<Dictionary<string, object>>(CSVReader.Read("LO_Character"));
    private static List<Dictionary<string, object>> Localization_Data_UI = new List<Dictionary<string, object>>(CSVReader.Read("LO_UI"));
    private static List<Dictionary<string, object>> Localization_Data_Tutorial = new List<Dictionary<string, object>>(CSVReader.Read("LO_Tutorial"));

    public static Action localization;

    public static Dictionary<string, String_Data> local_Data = dictionary();
    private static Dictionary<string, String_Data> dictionary()
    {
        Dictionary<string, String_Data> value = new Dictionary<string, String_Data>();
        for (int i = 0; i < Localization_Data.Count; i++)
        {
            String_Data data = new String_Data();
            data.kr = Localization_Data[i]["kr"].ToString();
            data.en = Localization_Data[i]["en"].ToString();
            data.es = Localization_Data[i]["es"].ToString();
            string temp = Localization_Data[i]["Key"].ToString();

            value.Add(temp, data);
        }

        for(int i = 0; i < Localization_Data_UI.Count; i++)
        {
            String_Data data = new String_Data();
            data.kr = Localization_Data_UI[i]["kr"].ToString();
            data.en = Localization_Data_UI[i]["en"].ToString();
            data.es = Localization_Data_UI[i]["es"].ToString();
            string temp = Localization_Data_UI[i]["Key"].ToString();
            value.Add(temp, data);
        }

        for(int i = 0; i < Localization_Data_Tutorial.Count; i++)
        {
            String_Data data = new String_Data();
            data.kr = Localization_Data_Tutorial[i]["kr"].ToString();
            data.en = Localization_Data_Tutorial[i]["en"].ToString();
            data.es = Localization_Data_Tutorial[i]["es"].ToString();
            string temp = Localization_Data_Tutorial[i]["Key"].ToString();
            value.Add(temp, data);
        }

        string check = PlayerPrefs.GetString("LOCAL");
        if (check == "")
        {
            SystemLanguage lang = Application.systemLanguage;
            switch (lang)
            {
                case SystemLanguage.Korean: LocalAccess = "kr"; break;
                case SystemLanguage.Spanish: LocalAccess = "es"; break;
                default: LocalAccess = "en"; break;
            }
        }
        else
        {
            LocalAccess = check;
        }

        return value;
    }
    public static void Set_Localization()
    {
        localization();
    }
    public static void Set_Dictionary(string temp, GameObject go)
    {
        LocalAccess = temp;
        Set_Localization();
        go.SetActive(false);
    }
    public virtual void LocalAdd(Action ac)
    {
        localization += ac;
    }
    public virtual void LocalRemove(Action ac)
    {
        localization -= ac;
    }
}
