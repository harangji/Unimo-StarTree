using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class EasySaveManager : MonoBehaviour
{
    public static EasySaveManager Instance { get; private set; }
    private ES3Settings UserDataSettings { get; set; }
    private ES3File UserDataES3File { get; set; }
    private ES3File UserDataES3FileLoaded { get; set; }
    public bool bSetEasySaveUser { get; set; } = false;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        
        UserDataSettings = new ES3Settings("UserData.es3"); //ES3Settings �ν��Ͻ� ����
        UserDataES3File = new ES3File(UserDataSettings);
    }

    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.S))
    //     {
    //         CommitBuffered();
    //     }
    // }
    // ES3Settings ��� ���� //�����ؼ� �Ⱦ�
    // public void Save<T>(string key, T value)
    // {
    //     ES3.Save<T>(key, value, UserDataSettings);
    // }

    // ES3File ���ۿ� ���� ���� - CommitBuffered() �� �ѹ���
    public void SaveBuffered<T>(string key, T value)
    {
        if(!canSave) return;
        MyDebug.Log("���� ���� ����");
        UserDataES3File.Save<T>(key, value);
    }

    // ES3File ���ۿ� ���� value�� ����
    public void CommitBuffered()
    {
        if(!canSave) return;
        MyDebug.Log("���� ���� ���� �Ϸ�");
        UserDataES3File.Sync();
    }
    
    // �ҷ�����
    public bool TryLoad<T>(string key, out T tValue)
    {
        UserDataES3FileLoaded = new ES3File(UserDataSettings); //�޾ƿ� �� �ν��Ͻ��� �����ϴ°��� ����
        
        if (UserDataES3FileLoaded.KeyExists(key)) // �����ϰ� Ű�� �ִ��� �̸� �˻�
        {
            tValue = UserDataES3FileLoaded.Load<T>(key); //�ִٸ� value��
            return true; // out T tValue
        }
        
        tValue = default; //���ٸ� T default����
        return false;
    }

    // ����
    public void Delete(string key)
    {
        if (ES3.KeyExists(key, UserDataSettings.path))
            ES3.DeleteKey(key, UserDataSettings);
    }

    private bool canSave = true;
    
    // ��ü ����
    public void DeleteAll()
    {
        ES3.DeleteFile(UserDataSettings.path);
        canSave = false;
    }
    
}

[System.Serializable]
public class User_Data
{
    public string UserName = "";

    public float EXP = 0; // ���� ����ġ
    public int Level = 0; // ���� ����
    public double Second_Base = 5; // �ü� �ʴ� ���� ���귮
    public double NextLevel_Base = 5; // ������ ���
    public double Yellow, Red, Blue; //����
    
    // �������� ���� (��� : ������)
    public int BestStage = 0; //�ְ� Ŭ���� ��������
    public Dictionary<int, int> BonusStageStars = new Dictionary<int, int>();
    // ��ü �������� ���� ȹ�� ���� ����
    public HashSet<int> RewardedStages = new HashSet<int>();

    //?????
    public float[] BuffFloating = new float[3];

    //������ ĳ���� true, �̺��� false
    public bool[] HasCharacterData = { true, false, false, false, false, false, false, false, false, false, false, false, false };
    
    //������ �غؿ��� true, �̺��� false
    public bool[] HasEnginData = { 
        true, false, false, false, false, false, false, false, false, false, 
        false, false, false, false, false, false, false, false, false, false, false, false, false};

    public double BestScoreGameOne, BestScoreGameTwo;

    public int IAP = 0;
    
    // ���� Ȯ���� ���� int �������Դϴ�.
    public int ReinforceCountTotal; // ��ȭ Ƚ��
    public int StarSum; // �� �� ����
    // �ü� ���� (������� �ʾ� �ּ� ó��)
    // public int FacilityLevelSum;

    //�������� ��ǰ ���� ����
    public bool ADSBuy = false; 

    public int DailyAccount = 1;
    public int GamePlay;
    public int ADS;
    public int ADSNoneReset;
    public int Touch; // ��ġ���� ���ϸ� ��ȭ�ϱ�� �����߽��ϴ�. (�������� ���� �ٲ��� �ʾҽ��ϴ�.)
    public int TimeItem; // �ð� �������� �ƴ� ȸ�� ���������� �����߽��ϴ�. (���������� �������� �ٲ��� �ʾҽ��ϴ�.)
    public int RePlay;

    // �̼� �Ϸ� ���� (���ϸ�)
    public bool GetDaily; // �⼮
    public bool GetGamePlay; // ���� �÷����ϱ�
    public bool GetADS; // ������
    public bool GetTouch; // ���ϸ� ��ġ�ϱ�
    public bool GetTimeItem; // ���� ������ Ÿ�̸� ���� ������ �Ա�
    public bool GetRePlay; // �� �� ����

    //���� �Ϸ� ����
    public bool[] GetArchivements =
    {
        false, false, false, false, false, false, false, false, false,
        false, false, false, false, false, false, false, false, false, false, false, false, false, false,
        false, false, false, false, false, false, false
    };
    // public bool[] GetArchivements = { false, false, false, false, false, false, false, false, false };

    public string S_DateTime, E_DateTime;

    public int selectCharacter = 1;
    public int selectEngine = 1;

    public float BonusRewardCount = 1000.0f;

    public bool GetOarkTong = false;
    public bool GetStarChange = false;
    public bool GetGameTwo = false;
    public bool GetReview = false;
    public bool GetInGame = false;
    public bool GetVane = false;
}

