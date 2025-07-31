using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionFinder : MonoBehaviour
{
    public List<Dictionary<string, object>> Data = new List<Dictionary<string, object>>();

    public bool GetCompletedMission = false;

    public GameObject MissionMark;

    private void Start()
    {
        Data = CSVReader.Read("Mission");

        StartCoroutine(InitCoroutine());
    }
    public void InitCheckCompleted()
    {
        GetCompletedMission = false;
        int a = 0;
        for (int i = 0; i < Data.Count; i++)
        {
            string style = Data[i]["Style"].ToString();
            int count = int.Parse(Data[i]["Count"].ToString());
            string type = Data[i]["Type"].ToString();
            bool Checking = false;
            if(type == "Daily")
            {
                Checking = GetCheck(style);
            }
            else if(type == "Achievements")
            {
                Checking = GetCheckAchieve(a);
                a++;
            }
            if (Checking == false)
            {
                GetCompletedMission = valueCount(style) >= count;
                if (GetCompletedMission == true) break;
            }
        }
        
        MissionMark.SetActive(GetCompletedMission);
    }
    private bool GetCheckAchieve(int value)
    {
        return Base_Manager.Data.UserData.GetArchivements[value];
    }


    private bool GetCheck(string style)
    {
        switch (style)
        {
            case "GamePlay":
                return Base_Manager.Data.UserData.GetGamePlay;
            case "ADS":
                return Base_Manager.Data.UserData.GetADS;
            case "Touch":
                return Base_Manager.Data.UserData.GetTouch;
            case "DailyAccount":
                return Base_Manager.Data.UserData.GetDaily;
            case "TimeItem":
                return Base_Manager.Data.UserData.GetTimeItem;
            case "RePlay":
                return Base_Manager.Data.UserData.GetRePlay;
        }
        return false;
    }
    IEnumerator InitCoroutine()
    {
        InitCheckCompleted();
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(InitCoroutine());
    }

    public int valueCount(string style)
    {
        switch (style)
        {
            case "GamePlay": return Base_Manager.Data.UserData.GamePlay;
            case "ADS": return Base_Manager.Data.UserData.ADS;
            case "ADSNONE": return Base_Manager.Data.UserData.ADSNoneReset;
            case "Touch": return Base_Manager.Data.UserData.Touch;
            case "Level": return Base_Manager.Data.UserData.Level + 1;
            case "DailyAccount": return Base_Manager.Data.UserData.DailyAccount;
            case "TimeItem": return Base_Manager.Data.UserData.TimeItem;
            case "RePlay": return Base_Manager.Data.UserData.RePlay;
            case "IAP": return Base_Manager.Data.UserData.IAP;
            case "Collection":
                int a = 0;
                for (int i = 0; i < Base_Manager.Data.UserData.HasCharacterData.Length; i++)
                {
                    if (Base_Manager.Data.UserData.HasCharacterData[i] == true)
                    {
                        a++;
                    }
                }
                return a;
            case "Collection_EQ":
                int b = 0;
                for (int i = 0; i < Base_Manager.Data.UserData.HasEnginData.Length; i++)
                {
                    if (Base_Manager.Data.UserData.HasEnginData[i] == true)
                    {
                        b++;
                    }
                }
                return b;
            case "BestStage": return Base_Manager.Data.UserData.BestStage;
            case "FacilityLevelSum": return Base_Manager.Data.UserData.FacilityLevelSum;
            case "Reinforce": return Base_Manager.Data.UserData.ReinforceCountTotal;
        }
        return -1;
    }
}
