using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using static UnityEngine.Rendering.DebugUI;

[System.Serializable]
public class Server_Data
{
    public string UserName = "";

    public float EXP = 0;
    public int Level = 0;
    public double Second_Base = 5;
    public double NextLevel_Base = 5;
    public double Yellow, Red, Blue;

    public float[] BuffFloating = new float[3];

    public bool[] GetCharacterData = { true, false, false, false, false, false, false, false, false, false, false, false, false };
    public bool[] GetEQData = { true, false, false, false, false, false, false, false, false, false, false, false, false , false, false , false, false, false, false , false, false, false, false};

    public double BestScoreGameOne, BestScoreGameTwo;

    public int IAP = 0;

    public bool ADSBuy = false;
    public bool ADS_Inter_Buy = false;

    public int DailyAccount = 1;
    public int GamePlay;
    public int ADS;
    public int ADSNoneReset;
    public int Touch;
    public int TimeItem;
    public int RePlay;

    public bool GetDaily;
    public bool GetGamePlay;
    public bool GetADS;
    public bool GetTouch;
    public bool GetTimeItem;
    public bool GetRePlay;

    public bool[] GetArchivements = { false, false, false, false, false, false, false, false, false };

    public string S_DateTime, E_DateTime;

    public int CharCount = 1;
    public int EQCount = 1;

    public float BonusRewardCount = 1000.0f;

    public bool GetOarkTong = false;
    public bool GetStarChange = false;
    public bool GetGameTwo = false;
    public bool GetReview = false;
    public bool GetInGame = false;
    public bool GetVane = false;
}
public enum Asset_State
{
    Yellow, Red, Blue
}

public class EXP_DATA
{
    public float EXP = 1.5f;
    public float GET_EXP = 1.2f;
    public float L_GOLD= 1.5f;
    public float L_GOLD_GET = 1.2f;
}
public class Data_Mng
{
    public static SpriteAtlas atlas;

    public Server_Data data;
    public int[] AltaCount = { 99, 249, 599, 999 };

    public EXP_DATA exp_data;

    public float EXP_SET = 0;
    public float EXP_GET = 0;
    public static bool SetPlayScene = false;
    public static int QualityLevel;
    float CalculateExperienceForLevel(float baseValue, int level, float value)
    {
        return baseValue * Mathf.Pow((level+1), value);
    }
    public void Init()
    {
        if (PlayerPrefs.HasKey("QualityLevel"))
        {
            QualityLevel = PlayerPrefs.GetInt("QualityLevel");
            QualitySettings.SetQualityLevel(QualityLevel, true);
        }
        else
        {
            float currentFPS = 1.0f / Time.deltaTime;
            if(currentFPS <= 30)
            {
                QualityLevel = 0;
            }
            else QualityLevel = 2;
            PlayerPrefs.SetInt("QualityLevel", QualityLevel);
            QualitySettings.SetQualityLevel(QualityLevel, true);
        }
        exp_data = new EXP_DATA();
        exp_data.EXP = 1.5f;
        exp_data.GET_EXP = 1.2f;
        exp_data.L_GOLD = 1.5f;
        exp_data.L_GOLD_GET = 1.2f;

        atlas = Resources.Load<SpriteAtlas>("Atlas");
        EXP_SET = CalculateExperienceForLevel(15, data.Level + 1, exp_data.EXP);
        EXP_GET = CalculateExperienceForLevel(5,  data.Level + 1, exp_data.GET_EXP);


        float timer = (float)Base_Mng.instance.TimerCheck();
        for(int i = 0; i < data.BuffFloating.Length; i++)
        {
            data.BuffFloating[i] -= timer;
            if (data.BuffFloating[i] <= 0.0f) data.BuffFloating[i] = 0.0f;
        }
        data.BonusRewardCount += timer;
        if (data.BonusRewardCount >= 900.0f) data.BonusRewardCount = 1000.0f;
    }

    public void LevelUP()
    {
        data.EXP += EXP_GET;
        data.Second_Base = CalculateExperienceForLevel(5, data.Level + 1, exp_data.L_GOLD_GET);
        data.NextLevel_Base = CalculateExperienceForLevel(10,data.Level + 1, exp_data.L_GOLD);

        if (data.Level <= 100) data.NextLevel_Base /= 2;

        if (data.EXP >= EXP_SET)
        {
            data.EXP = 0;
            data.Level++;
            if (data.Level >= 99)
            {
                Base_Mng.Analytics.RecordCustomEventWithParameters("Level", data.Level);
                if (data.GetGameTwo == false)
                {
                    data.GetGameTwo = true;
                    Canvas_Holder.instance.GetUI("##Game");
                }
            }
            LevelCheck();
            Canvas_Holder.instance.GetLevelCheck();
            EXP_SET = CalculateExperienceForLevel(15, data.Level + 1, exp_data.EXP);
            EXP_GET = CalculateExperienceForLevel(5, data.Level + 1, exp_data.GET_EXP);
        }
        Main_UI.instance.Text_Check();
    }

    public float EXP_Percentage()
    {
        float exp = EXP_SET;
        double myExp = data.EXP;
   
        return (float)myExp / exp;
    }
    public float Next_EXP()
    {
        float exp = EXP_SET;
        float myExp = EXP_GET;
        if (data.Level >= 1)
        {
            exp -= EXP_SET;
        }
        return (myExp / exp) * 100.0f;
    }

    private void LevelCheck()
    {
        for(int i = 0; i < AltaCount.Length; i++)
        {
            if (data.Level == AltaCount[i])
            {
                
                Land.instance.GetLevelUpAlta(i);
            }
        }
    }


    public void AssetPlus(Asset_State state, double value)
    {
        switch(state)
        {
            case Asset_State.Yellow:
                data.Yellow += value;
                if(Base_Mng.SavingMode)
                {
                    Base_Mng.Savingyellow += value;
                }
                Main_UI.instance.TextColorCheck();
                break;
            case Asset_State.Red: data.Red += value; break;
            case Asset_State.Blue: data.Blue += value; break;
        }

        Main_UI.instance.Text_Check();
    }

    public void Save()
    {
        if(data.UserName == "")
        {
            data.UserName = Base_Mng.Firebase.UserName;
        }

        data.E_DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string jsonData = JsonUtility.ToJson(data);
        Base_Mng.Firebase.WriteData(Base_Mng.Firebase.UserID, jsonData);
    }

    public void Load()
    {
        Base_Mng.Firebase.ReadData(Base_Mng.Firebase.UserID, (value) =>
        {
            data = JsonUtility.FromJson<Server_Data>(value);
            data.S_DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Init();

            if (data.E_DateTime != "" && data.S_DateTime != "")
            {
                var endData = DateTime.Parse(data.E_DateTime);
                var startData = DateTime.Parse(data.S_DateTime);

                if (endData.Day != startData.Day)
                {
                    data.GamePlay = 0;
                    data.ADS = 0;
                    data.DailyAccount = 1;
                    data.Touch = 0;
                    data.TimeItem = 0;
                    data.RePlay = 0;

                    data.GetReview = false;
                    data.GetInGame = false;
                    data.GetGamePlay = false;
                    data.GetADS = false;
                    data.GetDaily = false;
                    data.GetTouch = false;
                    data.GetTimeItem = false;
                    data.GetRePlay = false;
                }

                if(data.GetCharacterData.Length <= 12)
                {
                    bool[] charDatas = { true, false, false, false, false, false, false, false, false, false, false, false, false };

                    data.GetCharacterData = charDatas;
                }
            }
        });
    }
}
