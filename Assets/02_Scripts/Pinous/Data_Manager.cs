using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using static UnityEngine.Rendering.DebugUI;


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
public class Data_Manager
{
    public static SpriteAtlas atlas;

    public User_Data UserData = new User_Data();
    public readonly int[] AltaCount = { 99, 249, 599, 999 }; // 로비의 나무 '알타'의 모습을 결정하는 기준이 되는 int배열

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
        EXP_SET = CalculateExperienceForLevel(15, UserData.Level + 1, exp_data.EXP);
        EXP_GET = CalculateExperienceForLevel(5,  UserData.Level + 1, exp_data.GET_EXP);

        float timer = (float)Base_Manager.instance.TimerCheck();
        for(int i = 0; i < UserData.BuffFloating.Length; i++)
        {
            UserData.BuffFloating[i] -= timer;
            if (UserData.BuffFloating[i] <= 0.0f) UserData.BuffFloating[i] = 0.0f;
        }
        UserData.BonusRewardCount += timer;
        if (UserData.BonusRewardCount >= 900.0f) UserData.BonusRewardCount = 1000.0f;
    }
    
    public void LevelUP()
    {
        UserData.EXP += EXP_GET;
        UserData.Second_Base = CalculateExperienceForLevel(5, UserData.Level + 1, exp_data.L_GOLD_GET);
        UserData.NextLevel_Base = CalculateExperienceForLevel(10,UserData.Level + 1, exp_data.L_GOLD);

        if (UserData.Level <= 100) UserData.NextLevel_Base /= 2;

        if (UserData.EXP >= EXP_SET)
        {
            UserData.EXP = 0;
            UserData.Level++;
            if (UserData.Level >= 99)
            {
                // Base_Mng.Analytics.RecordCustomEventWithParameters("Level", UserData.Level);
                if (UserData.GetGameTwo == false)
                {
                    UserData.GetGameTwo = true;
                    Canvas_Holder.instance.GetUI("##Game");
                }
            }
            LevelCheck();
            Canvas_Holder.instance.GetLevelCheck();
            EXP_SET = CalculateExperienceForLevel(15, UserData.Level + 1, exp_data.EXP);
            EXP_GET = CalculateExperienceForLevel(5, UserData.Level + 1, exp_data.GET_EXP);
        }
        Main_UI.instance.Text_Check();
    }

    public float EXP_Percentage()
    {
        float exp = EXP_SET;
        
        double myExp = UserData.EXP;
        
        return (float)myExp / exp;
    }
    
    public float Next_EXP()
    {
        float exp = EXP_SET;
        float myExp = EXP_GET;
        if (UserData.Level >= 1)
        {
            exp -= EXP_SET;
        }
        return (myExp / exp) * 100.0f;
    }

    private void LevelCheck()
    {
        for(int i = 0; i < AltaCount.Length; i++)
        {
            if (UserData.Level == AltaCount[i])
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
                UserData.Yellow += value;
                if(Base_Manager.SavingMode)
                {
                    Base_Manager.Savingyellow += value;
                }
                Main_UI.instance.TextColorCheck();
                break;
            case Asset_State.Red: UserData.Red += value; break;
            case Asset_State.Blue: UserData.Blue += value; break;
        }

        Main_UI.instance.Text_Check();
    }

    public void SaveUserData()
    {
        if(UserData.UserName == "")
        {
            UserData.UserName = UserData.UserName;
        }

        UserData.E_DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        EasySaveManager.Instance.SaveBuffered("User_Data", UserData);
        EasySaveManager.Instance.CommitBuffered(); //버퍼에 쌓인 기록 저장
    }

    public void LoadUserData()
    {
        if (EasySaveManager.Instance.TryLoad("User_Data", out User_Data _UserData)) //키 탐색 후 out으로 반환
        {
            UserData = _UserData;
            
            UserData.S_DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Init();

            if (UserData.E_DateTime != "" && UserData.S_DateTime != "")
            {
                var endData = DateTime.Parse(UserData.E_DateTime);
                var startData = DateTime.Parse(UserData.S_DateTime);

                if (endData.Day != startData.Day)
                {
                    UserData.GamePlay = 0;
                    UserData.ADS = 0;
                    UserData.DailyAccount = 1;
                    UserData.Touch = 0;
                    UserData.TimeItem = 0;
                    UserData.RePlay = 0;

                    UserData.GetReview = false;
                    UserData.GetInGame = false;
                    UserData.GetGamePlay = false;
                    UserData.GetADS = false;
                    UserData.GetDaily = false;
                    UserData.GetTouch = false;
                    UserData.GetTimeItem = false;
                    UserData.GetRePlay = false;
                }

                if(UserData.GetCharacterData.Length <= 12)
                {
                    bool[] charDatas = { true, false, false, false, false, false, false, false, false, false, false, false, false };

                    UserData.GetCharacterData = charDatas;
                }
            }
            
            EasySaveManager.Instance.bSetEasySaveUser = true;
        }
        else
        {
            UserData = NewData();
            Init();
            
            EasySaveManager.Instance.SaveBuffered("User_Data", UserData);
            EasySaveManager.Instance.CommitBuffered(); //버퍼에 쌓인 기록 저장
            
            EasySaveManager.Instance.bSetEasySaveUser = true;
        }
    }
    
    User_Data NewData()
    {
        PlayerPrefs.SetFloat("Volume", 1.0f);
        PlayerPrefs.SetFloat("FXVolume", 1.0f);

        Sound_Manager.instance.SoundCheck();

        User_Data data = new User_Data();
        data.UserName = UserData.UserName;
        data.EXP = 0;
        data.Level = 0;
        data.Second_Base = 5;
        data.Yellow = 0;
        data.Red = 0;
        data.Blue = 0;
        data.BestStage = 0;
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

        data.selectCharacter = 1;
        data.selectEngine = 1;

        data.BonusRewardCount = 1000.0f;

        return data;
    }
}
