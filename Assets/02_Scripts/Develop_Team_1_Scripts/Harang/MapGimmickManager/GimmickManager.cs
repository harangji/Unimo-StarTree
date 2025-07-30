using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GimmickManager : MonoBehaviour
{    
    [field: SerializeField] public GameObject UnimoPrefab { get; set; }
    [field: SerializeField] private GameDataSOHolder gameDataSoHolder;
    
    //경고 연출s
    [LabelText("경고 팝업창"), SerializeField]
    private GameObject warningPopup;
    
    [LabelText("기믹 텍스트들"), SerializeField]
    private List<TextMeshProUGUI> modeText;
    
    [LabelText("기믹 아이콘들"), SerializeField]
    private List<Image> modeIcon;
    
    [LabelText("기믹 출현 BGM 실행할 소스"), SerializeField]
    private AudioSource bgmSource; 
    
    [LabelText("기믹 출현 BGM"), SerializeField]
    private AudioClip modeBGM;
    
    [LabelText("이로운 기믹 텍스트 색상"), SerializeField] 
    private Color goodModeTxtColor;
    
    [LabelText("이로운 기믹 이미지 색상"), SerializeField]
    private Color goodModeImgColor;

    [LabelText("기믹 초기화자 직접할당"), SerializeField]
    private GimmickInitializer[] gimmickInitializers;
    
    [LabelText("기믹 준비됨 bool"), ReadOnly]
    private bool bIsReadyGimmiks = false;
    
    
    //ReadOnly
    [LabelText("현재 스테이지 캐싱"), ShowInInspector, ReadOnly]
    private int mCurrentStage;
    
    [LabelText("복잡도 점수"), ShowInInspector, ReadOnly]
    private int mCurrentCost;
    
    private Queue<Gimmick> mReadyGimmickQueue = new Queue<Gimmick>();
    
    private List<eGimmickGrade> mCanSelectAbleGrades = new List<eGimmickGrade>(); //등장 가능한 기믹 등급
    
    private int gimmickCount = 0;
    
    private Dictionary<string, GimmickInitializer> mGimmickInitializersMapper;
    
    public static GimmickManager Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        mGimmickInitializersMapper = new Dictionary<string, GimmickInitializer>
        {
            { "G_TRP_002", gimmickInitializers[0] }, // 블랙홀
            { "G_ENV_004", gimmickInitializers[1] } // 레드존
        };
    }

    void Start()
    {
        mReadyGimmickQueue.Clear();
        
        InitializeGimmickManager(); //기믹 뽑기
    }

    public void ExecuteGimmick() //다른 곳에서 실행
    {
        bgmSource.clip = modeBGM;
        StartCoroutine(TriggerGimmickCoroutine());
    }
    
    //기믹 매니저 초기화
    public void InitializeGimmickManager()
    {
        mCurrentStage = Base_Manager.Data.UserData.BestStage + 100; //스테이지 int 캐싱 //최소 1 (테스트 100)

        if (SettingByStageNumber(mCurrentStage)) //스테이지 수에 따른 설정들 - false일 경우 기믹 갯수가 0이므로 실행 안함
        {
            ToTsvGimmickData[] toTsvGimmickDatas = gameDataSoHolder.GimmicParsingDataSo.GetDataAll();
            for (int i = 0; i < toTsvGimmickDatas.Length; i++)
            {
                //key로 Initializer 매핑하여 초기화
                if (mGimmickInitializersMapper.TryGetValue(toTsvGimmickDatas[i].Key, out var gimmickInitializer))
                {
                    gimmickInitializer.GimmickInitializerSetting(toTsvGimmickDatas[i]);
                }
            }

            List<GimmickSample> canSelectAbleGimmickSamples = GetCanSelectAbleGimmickSamplesByGrade(); //기믹 껍데기 반환
            PickGimmickSamples(canSelectAbleGimmickSamples, gimmickCount); //기믹 껍데기 기반으로 생성

            bIsReadyGimmiks = true;
        }
        else
        {
            bIsReadyGimmiks = false;
        }
    }
    
    //currentStage를 통한 설정
    private bool SettingByStageNumber(int stageNumber)
    {
        int[] probabilities = new int[4]; // [0]=1개 확률, [1]=2개, [2]=3개, [3]=4개
        
        switch (stageNumber)
        {
            case <= 10:
                mCurrentCost = 0; //복잡도 점수
                return false;
            
            case <= 50:
                mCurrentCost = 10;
                probabilities = new[] { 100, 0, 0, 0 }; //갯수 확률 설정
            
                mCanSelectAbleGrades.Add(eGimmickGrade.Nomal); //고를 수 있는 기믹 등급 리스트 설정
                break;
            
            case <= 100:
                mCurrentCost = 25;
                probabilities = new[] { 30, 70, 0, 0 };
            
                mCanSelectAbleGrades.Add(eGimmickGrade.Nomal);
                mCanSelectAbleGrades.Add(eGimmickGrade.Enhanced);
                break;
            
            case <= 300:
                mCurrentCost = 40;
                probabilities = new[] { 0, 30, 70, 0 };
                mCanSelectAbleGrades.Add(eGimmickGrade.Nomal);
                mCanSelectAbleGrades.Add(eGimmickGrade.Enhanced);
                mCanSelectAbleGrades.Add(eGimmickGrade.Elite);
                break;
            
            case <= 600:
                mCurrentCost = 55;
                probabilities = new[] { 0, 20, 70, 10 };
            
                mCanSelectAbleGrades.Add(eGimmickGrade.Enhanced);
                mCanSelectAbleGrades.Add(eGimmickGrade.Elite);
                break;
            
            case <= 1000:
                mCurrentCost = 70;
                probabilities = new[] { 0, 0, 40, 60 };
            
                mCanSelectAbleGrades.Add(eGimmickGrade.Elite);
                mCanSelectAbleGrades.Add(eGimmickGrade.Legendary);
                break;
        }
        
        int roll = Random.Range(1, 101); // 1~100
        int cumulative = 0; 

        for (int i = 0; i < probabilities.Length; i++) //추첨
        {
            cumulative += probabilities[i]; //확률 저장
            if (roll <= cumulative)
            {
                gimmickCount = i + 1; // 1 ~ 4
                return true;
            }
        }

        return false;
    }
    
    //기믹 껍데기 생성하기
    private List<GimmickSample> GetCanSelectAbleGimmickSamplesByGrade()
    {
        List<GimmickSample> selectedGimmickSampless = new List<GimmickSample>();
        
        foreach (var grade in mCanSelectAbleGrades) //나올 수 있는 Grade를 전부 조회
        {
            //하나의 Grade의 gimmickInitializers 모두 조회
            foreach (var gimmickInitializer in gimmickInitializers)
            {
                if (gimmickInitializer.Costs[(int)grade] > mCurrentCost) continue;
                
                GimmickSample gimmickSample = new GimmickSample()
                {
                    GimmickType = gimmickInitializer.GimmickType,
                    GimmickGrade = grade,
                    GimmickInitializer = gimmickInitializer,
                    GimmickWeight = gimmickInitializer.Weights[(int)grade],
                    GimmickCost =  gimmickInitializer.Costs[(int)grade]
                };
                
                selectedGimmickSampless.Add(gimmickSample); //껍데기만 Add
            }
        }

        return selectedGimmickSampless;
    } 
    
    //기믹 미리 준비하기
    public void PickGimmickSamples(List<GimmickSample> source, int count)
    {
        List<eGimmicks> cashGimmickTypes = new List<eGimmicks>();  //중복방지
        
        for (int i = 0; i < count; i++) //count 번 실행 // && pool.Count > 0
        {
            // 아직 남은 후보가 없으면 중단
            // 이미 뽑은 기믹 타입을 제외
            List<GimmickSample> filteredPool = source
                .Where(x => !cashGimmickTypes.Contains(x.GimmickInitializer.GimmickType))
                .ToList();

            if (filteredPool.Count == 0)
                break;
            
            float totalWeight = source.Sum(e => e.GimmickWeight);
            int rand = (int)Random.Range(0, totalWeight);

            float cumulative = 0;
            foreach (GimmickSample entry in filteredPool)
            {
                cumulative += entry.GimmickWeight; //가중치 더함
                
                if (rand <= cumulative) //추첨
                {
                    Gimmick readyGimmick = entry.GimmickInitializer.InitializeGimmick(entry.GimmickGrade); //껍데기를 통해 생성
                    mReadyGimmickQueue.Enqueue(readyGimmick); // 준비된 기믹에 추가
                    MyDebug.Log($"{readyGimmick.gimmickName} is ready");
                    
                    cashGimmickTypes.Add(entry.GimmickInitializer.GimmickType); // 뽑은 타입을 추가, 중복방지
                    
                    // 순회 중인 자료구조의 구조가 바뀔 경우 위험을 초래하므로 변경
                    // pool.Remove(entry); //순회 리스트에서 제거로 중복 방지 - foreach에서는 불가능
                    //타입이 일치하지 않는 기믹만 리스트에 새로 담기 = 중복 방지
                    // pool = pool.Where(x => x.GimmickInitializer.GimmickType != entry.GimmickInitializer.GimmickType).ToList();
                    break;
                }
            }
        }
    }
    
    private IEnumerator TriggerGimmickCoroutine() //코루틴으로 모드 진행
    {
        yield return new WaitForSeconds(PlayProcessController.InitTimeSTATIC);
        PlaySceneController.Instance.PauseGame();
        
        Gimmick gimmick = mReadyGimmickQueue.Dequeue();
        gimmick.SetModeName(modeText[0]);
        if (gimmick.eGimmickType == eGimmickType.Helpful)
        {
            modeText[0].color = goodModeTxtColor;
            modeIcon[0].color = goodModeImgColor;
        }
        modeIcon[0].gameObject.SetActive(true);
        
        warningPopup.SetActive(true);
        yield return new WaitForSecondsRealtime(2.5f);
        warningPopup.SetActive(false);
        
        PlaySceneController.Instance.ResumeGame();
        yield break;
        
        // //다수 동시
        // for (int i = 0; i < modenum; i++)
        // {
        //     mReadyGimmickQueue.Dequeue().SetModeName(modeText[i]);
        //     modeIcon[i].sprite = specialModes[idx].ModeIcon;
        //     if (specialModes[idx].ModeCode.Equals("mode7") || specialModes[idx].ModeCode.Equals("mode8"))
        //     {
        //         modeText[i].color = goodModeTxtColor;
        //         modeIcon[i].color = goodModeImgColor;
        //     }
        //     modeIcon[i].gameObject.SetActive(true);
        //     specialModes[idx].TriggerMode(); 
        //     // specialModes.RemoveAt(idx); //내보낸 모드 지우기
        // }
        // warningPopup.SetActive(true);
        // yield return new WaitForSecondsRealtime(2.5f);
        // warningPopup.SetActive(false);
        // sceneCtrl.ResumeGame();
        // yield break;
    }
}

