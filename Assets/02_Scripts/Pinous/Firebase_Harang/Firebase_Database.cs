using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public partial class Firebase_Manager: MonoBehaviour
{
    // 데이터베이스에 데이터 쓰기
    public void WriteData(string key, string value)
    {
        if(Application.internetReachability == NetworkReachability.NotReachable)
        {
            Canvas_Holder.instance.GetUI("##Network");
        }

        DatabaseReference childReference = reference.Child("USER").Child(key);
        childReference.SetRawJsonValueAsync(value).ContinueWithOnMainThread(task => {
            if (task.Exception != null)
            {
                Debug.LogError($"데이터 쓰기 오류: {task.Exception}");
            }
            else
            {
                Debug.Log("데이터 쓰기 성공!");
            }
        });
    }

    public void ReadDataOnVersion()
    {
        DatabaseReference childReference = reference.Child("ADMIN").Child("Version");
        childReference.GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.Exception != null)
            {
                Debug.LogError($"데이터 읽기 오류: {task.Exception}");
            }
            else if (task.Result.Exists)
            {
                // TODO: 아래 주석은 임시 로그인 허용이므로 후에 변경 필요합니다. 하랑
                // string versionValue = task.Result.Value.ToString();
                // if(versionValue != Application.version)
                // {
                //     VersionCheck.instance.GetVersionPopUP();
                //     return;
                // }
                // else
                {
                    AccountInitializer.instance.GetCheckVersionAndLogin();
                }
            }
            else
            {
             
            }
        });
    }

    // 데이터베이스에서 데이터 읽기
    public void ReadData(string key, System.Action<string> onDataReceived)
    {
        DatabaseReference childReference = reference.Child("USER").Child(key);
        childReference.GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.Exception != null)
            {
                Debug.LogError($"데이터 읽기 오류: {task.Exception}");
            }
            else if (task.Result.Exists)
            {
                string value = task.Result.GetRawJsonValue();
                if(string.IsNullOrEmpty(value)) //값이 없을 때 = 유저 정보가 없음
                {
                    value = NewData(); //새로 생성
                }
                isSetFirebase = true;

                onDataReceived?.Invoke(value);
            }
            else
            {
                string data = NewData();
                
                Base_Mng.Firebase.WriteData(Base_Mng.Firebase.UserID, data);
                isSetFirebase = true;

                onDataReceived?.Invoke(data);
            }
        });
    }

    string NewData()
    {
        PlayerPrefs.SetFloat("Volume", 1.0f);
        PlayerPrefs.SetFloat("FXVolume", 1.0f);

        Sound_Manager.instance.SoundCheck();

        Server_Data data = new Server_Data();
        data.UserName = Base_Mng.Firebase.UserName;
        data.EXP = 0;
        data.Level = 0;
        data.Second_Base = 5;
        data.Yellow = 0;
        data.Red = 0;
        data.Blue = 0;
        data.NextLevel_Base = 10;
        data.GetReview = false;
        data.GetOarkTong = false;
        data.GetGameTwo = false;
        data.GetInGame = false;
        data.GetVane = false;
        data.GetStarChange = false;
        data.BuffFloating[0] = 0.0f;
        data.BuffFloating[1] = 0.0f;
        data.BuffFloating[2] = 0.0f;
        
        data.BestScoreGameOne = 0;
        data.BestScoreGameTwo = 0;

        bool[] charDatas = { true, false, false, false, false, false, false, false, false, false, false, false, false };
        bool[] EqDatas = { true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };

        data.GetCharacterData = charDatas;
        data.GetEQData = EqDatas;

        data.DailyAccount = 1;
        data.GamePlay = 0;
        data.ADS = 0;
        data.Touch = 0;
        data.TimeItem = 0;
        data.RePlay = 0;

        bool[] archive = { false, false, false, false, false, false, false, false, false };
        data.GetArchivements = archive;

        data.IAP = 0;
        data.ADSBuy = false;

        data.GetDaily = false;
        data.GetGamePlay = false;
        data.GetADS = false;
        data.GetTouch = false;
        data.GetTimeItem = false;
        data.GetRePlay = false;
        data.ADSNoneReset = 0;
        data.S_DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        data.E_DateTime = "";

        data.CharCount = 1;
        data.EQCount = 1;

        data.BonusRewardCount = 1000.0f;

        string value = JsonUtility.ToJson(data);

        return value;
    }
}
