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
        
        UserDataSettings = new ES3Settings("UserData.es3"); //ES3Settings 인스턴스 생성
        UserDataES3File = new ES3File(UserDataSettings);
    }

    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.S))
    //     {
    //         CommitBuffered();
    //     }
    // }
    // ES3Settings 즉시 저장 //위험해서 안씀
    // public void Save<T>(string key, T value)
    // {
    //     ES3.Save<T>(key, value, UserDataSettings);
    // }

    // ES3File 버퍼에 지연 저장 - CommitBuffered() 시 한번에
    public void SaveBuffered<T>(string key, T value)
    {
        if(!canSave) return;
        MyDebug.Log("저장 스택 쌓임");
        UserDataES3File.Save<T>(key, value);
    }

    // ES3File 버퍼에 쌓인 value를 저장
    public void CommitBuffered()
    {
        if(!canSave) return;
        MyDebug.Log("쌓인 스택 저장 완료");
        UserDataES3File.Sync();
    }
    
    // 불러오기
    public bool TryLoad<T>(string key, out T tValue)
    {
        UserDataES3FileLoaded = new ES3File(UserDataSettings); //받아올 때 인스턴스를 생성하는것이 안전
        
        if (UserDataES3FileLoaded.KeyExists(key)) // 안전하게 키가 있는지 미리 검색
        {
            tValue = UserDataES3FileLoaded.Load<T>(key); //있다면 value를
            return true; // out T tValue
        }
        
        tValue = default; //없다면 T default값을
        return false;
    }

    // 삭제
    public void Delete(string key)
    {
        if (ES3.KeyExists(key, UserDataSettings.path))
            ES3.DeleteKey(key, UserDataSettings);
    }

    private bool canSave = true;
    
    // 전체 삭제
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

    public float EXP = 0; // 현재 경험치
    public int Level = 0; // 현재 레벨
    public double Second_Base = 5; // 시설 초당 별꿀 생산량
    public double NextLevel_Base = 5; // 레벨업 비용
    public double Yellow, Red, Blue; //별꿀
    
    // 스테이지 관련 (담당 : 김진수)
    public int BestStage = 0; //최고 클리어 스테이지
    public Dictionary<int, int> BonusStageStars = new Dictionary<int, int>();
    // 전체 스테이지 보상 획득 여부 저장
    public HashSet<int> RewardedStages = new HashSet<int>();

    //?????
    public float[] BuffFloating = new float[3];

    //보유한 캐릭터 true, 미보유 false
    public bool[] HasCharacterData = { true, false, false, false, false, false, false, false, false, false, false, false, false };
    
    //보유한 붕붕엔진 true, 미보유 false
    public bool[] HasEnginData = { 
        true, false, false, false, false, false, false, false, false, false, 
        false, false, false, false, false, false, false, false, false, false, false, false, false};

    public double BestScoreGameOne, BestScoreGameTwo;

    public int IAP = 0;
    
    // 업적 확인을 위한 int 변수들입니다.
    public int ReinforceCountTotal; // 강화 횟수
    public int StarSum; // 별 합 갯수
    // 시설 레벨 (사용하지 않아 주석 처리)
    // public int FacilityLevelSum;

    //광고제거 상품 구매 여부
    public bool ADSBuy = false; 

    public int DailyAccount = 1;
    public int GamePlay;
    public int ADS;
    public int ADSNoneReset;
    public int Touch; // 터치에서 유니모 강화하기로 변경했습니다. (변수명은 아직 바꾸지 않았습니다.)
    public int TimeItem; // 시간 아이템이 아닌 회복 아이템으로 변경했습니다. (마찬가지로 변수명은 바꾸지 않았습니다.)
    public int RePlay;

    // 미션 완료 여부 (데일리)
    public bool GetDaily; // 출석
    public bool GetGamePlay; // 게임 플레이하기
    public bool GetADS; // 광고보기
    public bool GetTouch; // 유니모 터치하기
    public bool GetTimeItem; // 게임 내에서 타이머 증가 아이템 먹기
    public bool GetRePlay; // 두 배 보상

    //업적 완료 여부
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

