using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Playables;

public class Base_Mng : MonoBehaviour
{
    public static Base_Mng instance = null;

    private static ADS_Mng s_ADS = new ADS_Mng();
    private static Data_Mng s_Data = new Data_Mng();
    private static Firebase_Manager s_Firebase = new Firebase_Manager();
    private static IAP_Manager s_IAP = new IAP_Manager();
    private static Analytics_Mng s_Analytics = new Analytics_Mng();
    public static Data_Mng Data { get { return s_Data; } }
    public static ADS_Mng ADS { get { return s_ADS; } }
    public static Firebase_Manager Firebase { get { return s_Firebase; } }
    public static IAP_Manager IAP { get { return s_IAP; } }
    public static Analytics_Mng m_Analytics { get { return s_Analytics; } }
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
   
    private void Update()
    {
        if (GetStartGame && Firebase.isSetFirebase)
        {
            timer += Time.deltaTime;
            if (timer >= 15.0f)
            {
                timer = 0.0f;
                Data.Save();
            }

            if (Data.data.BonusRewardCount < 1000.0f)
            {
                Data.data.BonusRewardCount += Time.deltaTime;
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

            for(int i = 0; i < Data.data.BuffFloating.Length; i++)
            {
                if (Data.data.BuffFloating[i] >= 0.0f)
                {
                    Data.data.BuffFloating[i] -= Time.deltaTime;
                }
            }
        }
    }
    private void Initalize()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
            IAP.Init();
            Firebase.Init();
            m_Analytics.Init();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public double TimerCheck()
    {
        if(Data.data.S_DateTime == "" || Data.data.E_DateTime == "")
        {
            return 0.0d;
        }

        DateTime startDate = DateTime.Parse(Data.data.S_DateTime);
        DateTime EndDate = DateTime.Parse(Data.data.E_DateTime);

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
        Data.data.E_DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        var Timer = DateTime.Parse(Data.data.E_DateTime) - DateTime.Parse(Data.data.S_DateTime);
        m_Analytics.RecordCustomEventWithParameters("Time", (int)Timer.TotalMinutes);
        Data.Save();
    }
}
