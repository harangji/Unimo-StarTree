using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Playables;

public class Base_Manager : MonoBehaviour
{
    public static Base_Manager instance = null;
    private static ADS_Manager s_ADS = new ADS_Manager();
    private static Data_Manager s_Data = new Data_Manager();
    private static IAP_Manager s_IAP = new IAP_Manager();
    // private static Analytics_Manager s_Analytics = new Analytics_Manager();
    public static Data_Manager Data { get { return s_Data; } }
    public static ADS_Manager ADS { get { return s_ADS; } }
    public static IAP_Manager IAP { get { return s_IAP; } }
    // public static Analytics_Manager Analytics { get { return s_Analytics; } }
    float timer = 0.0f;
    public float saveTimer = 0.0f;
    public bool GetStartGame = false;
    public static bool SavingMode = false;
    public static double Savingyellow;
    private void Awake()
    {
        Initalize();

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
    
    private void Initalize()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
            IAP.Init();
            // Firebase.Init();
            // Analytics.Init();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    
    private void Update()
    {
        if (EasySaveManager.Instance.bSetEasySaveUser)
        {
            timer += Time.deltaTime;
            if (timer >= 15.0f)
            {
                timer = 0.0f;
                Data.SaveUserData(); //15초마다 저장
            }

            if (Input.GetKeyDown(KeyCode.C)) //즉시 저장 테스트용
            {
                Data.SaveUserData();
            }
            
            if (Data.UserData.BonusRewardCount < 1000.0f)
            {
                Data.UserData.BonusRewardCount += Time.deltaTime;
            }
            if (SavingMode == false)
            {
                saveTimer += Time.deltaTime;
                if (saveTimer >= 300.0f)
                {
                    Canvas_Holder.instance.GetUI("##SavingMode");
                    saveTimer = 0.0f;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    saveTimer = 0.0f;
                }
            }
            
            for(int i = 0; i < Data.UserData.BuffFloating.Length; i++)
            {
                if (Data.UserData.BuffFloating[i] >= 0.0f)
                {
                    Data.UserData.BuffFloating[i] -= Time.deltaTime;
                }
            }
        }
    }

    public double TimerCheck()
    {
        if(Data.UserData.S_DateTime == "" || Data.UserData.E_DateTime == "")
        {
            return 0.0d;
        }

        DateTime startDate = DateTime.Parse(Data.UserData.S_DateTime);
        DateTime EndDate = DateTime.Parse(Data.UserData.E_DateTime);

        TimeSpan timer = startDate - EndDate;
        double timeCount = timer.TotalSeconds;

        return timeCount;
    }
    
    public GameObject Instantiate_Path(string path)
    {
        return Instantiate(Resources.Load<GameObject>(path));
    }

    public void Coroutine_Starter(IEnumerator enumerator)
    {
        StartCoroutine(enumerator);
    }

    private void OnDestroy()
    {
        Data.UserData.E_DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        var Timer = DateTime.Parse(Data.UserData.E_DateTime) - DateTime.Parse(Data.UserData.S_DateTime);
        // Analytics.RecordCustomEventWithParameters("Time", (int)Timer.TotalMinutes);
        Data.SaveUserData();
    }
}
