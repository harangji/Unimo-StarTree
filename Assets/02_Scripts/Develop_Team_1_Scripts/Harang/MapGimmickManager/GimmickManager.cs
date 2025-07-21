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
    [LabelText("현재 스테이지 캐싱"), ReadOnly]
    private int currentStage;
    
    [LabelText("복잡도 점수"), ReadOnly]
    private int currentCost;
    
    [LabelText("골라진 기믹들"), ReadOnly]
    private List<Gimmick> readyGimmicks = new List<Gimmick>();

    private List<eGimmickGrade> canSelectAbleGrades = new List<eGimmickGrade>();
    
    private Dictionary<eGimmickGrade, List<Gimmick>> canSelectedGimmicksDictionary = new Dictionary<eGimmickGrade, List<Gimmick>>(); //예비군
    
    private int gimmickCount = 0;
    
    void Start()
    {
        InitializeGimmickManager();
    }

    public void InitializeGimmickManager()
    {
        currentStage = Base_Manager.Data.UserData.BestStage + 100; //스테이지 int 캐싱 //최소 1 (테스트 100)
        
        if (SetGimmicCountByStageNumber(currentStage)) //스테이지 수에 따른 설정들 - false일 경우 기믹 갯수가 0이므로 실행 안함
        {
            List<GimmickSample> canSelectAbleGimmickSamples = GetCanSelectAbleGimmickSamplesByGrade(canSelectAbleGrades); //선택될 수 있는 
            PickGimmickSamples(canSelectAbleGimmickSamples, gimmickCount);
            
            bIsReadyGimmiks = true;
        }
        else
        {
            bIsReadyGimmiks = false;
        }
    }
    
    
    private bool SetGimmicCountByStageNumber(int stageNumber) //currentStage를 통한 설정
    {
        int[] probabilities = new int[4]; // [0]=1개 확률, [1]=2개, [2]=3개, [3]=4개
        
        switch (stageNumber)
        {
            case <= 10:
                currentCost = 0; //복잡도 점수
                return false;
            
            case <= 50:
                currentCost = 10;
                probabilities = new[] { 100, 0, 0, 0 };
            
                canSelectAbleGrades.Add(eGimmickGrade.Nomal); //고를 수 있는 기믹 등급 리스트 설정
                break;
            
            case <= 100:
                currentCost = 25;
                probabilities = new[] { 30, 70, 0, 0 };
            
                canSelectAbleGrades.Add(eGimmickGrade.Nomal);
                canSelectAbleGrades.Add(eGimmickGrade.Enhanced);
                break;
            
            case <= 300:
                currentCost = 40;
                probabilities = new[] { 0, 30, 70, 0 };
                canSelectAbleGrades.Add(eGimmickGrade.Nomal);
                canSelectAbleGrades.Add(eGimmickGrade.Enhanced);
                canSelectAbleGrades.Add(eGimmickGrade.Elite);
                break;
            
            case <= 600:
                currentCost = 55;
                probabilities = new[] { 0, 20, 70, 10 };
            
                canSelectAbleGrades.Add(eGimmickGrade.Enhanced);
                canSelectAbleGrades.Add(eGimmickGrade.Elite);
                break;
            
            case <= 1000:
                currentCost = 70;
                probabilities = new[] { 0, 0, 40, 60 };
            
                canSelectAbleGrades.Add(eGimmickGrade.Elite);
                canSelectAbleGrades.Add(eGimmickGrade.Legendary);
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
    
    private List<GimmickSample> GetCanSelectAbleGimmickSamplesByGrade(List<eGimmickGrade> canSelectGrades)
    {
        List<GimmickSample> selectedGimmickSampless = new List<GimmickSample>();
        
        foreach (var grade in canSelectGrades) //나올 수 있는 Grade를 전부 조회
        {
            //하나의 Grade의 gimmickInitializers 모두 조회
            foreach (var gimmickInitializer in gimmickInitializers)
            {
                if (gimmickInitializer.Costs[(int)grade] > currentCost) continue;
                
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
        
        // for (int i = 0; i < readyGimmicks.Length; i++) //readyGimmicks 길이만큼
        // {
        //     float rand = Random.Range(0f, totalWeight);
        //     float cumulative = 0f;
        //     
        //     foreach (var canSelectGimmick in canSelectedGimmicksDictionary) //딕셔너리 순회
        //     {
        //         foreach (var gimmick in canSelectGimmick.Value) //기믹 리스트
        //         {
        //             cumulative += gimmick.gimmickWeights[(int)canSelectGimmick.Key];
        //             if (rand < cumulative || testBool) //추첨 성공  //
        //             {
        //                 readyGimmicks[i] = gimmick; //준비된 기믹에 추가
        //                 readyGimmicks[i].SetGrade(canSelectGimmick.Key); //grade 세팅
        //                 
        //                 MyDebug.Log($"Tlqkf : {readyGimmicks[i].gimmickName}");
        //                 //ToDo : 중복제거?
        //             }
        //         }
        //     }
        // }
    } 
    
    //기믹 미리 준비하기
    public void PickGimmickSamples(List<GimmickSample> source, int count)
    {
        List<GimmickSample> pool = new List<GimmickSample>();
        
        List<eGimmickType> cashGimmickTypes = new List<eGimmickType>();
        
        for (int i = 0; i < count; i++) //count 번 실행 // && pool.Count > 0
        {
            // 아직 남은 후보가 없으면 중단
            // 이미 뽑은 기믹 타입을 제외
            List<GimmickSample> filteredPool = pool
                .Where(x => !cashGimmickTypes.Contains(x.GimmickInitializer.GimmickType))
                .ToList();

            if (filteredPool.Count == 0)
                break;
            
            float totalWeight = pool.Sum(e => e.GimmickWeight);
            int rand = (int)Random.Range(0, totalWeight);

            float cumulative = 0;
            foreach (GimmickSample entry in filteredPool)
            {
                cumulative += entry.GimmickWeight; //가중치 더함
                
                if (rand <= cumulative) //추첨
                {
                    Gimmick readyGimmick = entry.GimmickInitializer.InitializeGimmick(entry.GimmickGrade); //껍데기를 통해 생성
                    readyGimmicks.Add(readyGimmick); // 준비된 기믹에 추가
                    
                    cashGimmickTypes.Add(entry.GimmickInitializer.GimmickType); // 뽑은 타입을 추가, 중복방지
                    
                    // 순회 중인 자료구조의 구조가 바뀔 경우 위험을 초래하므로, if 비교를 매번 하도록 변경
                    // pool.Remove(entry); //순회 리스트에서 제거로 중복 방지 - foreach에서는 불가능
                    //타입이 일치하지 않는 기믹만 리스트에 새로 담기 = 중복 방지
                    // pool = pool.Where(x => x.GimmickInitializer.GimmickType != entry.GimmickInitializer.GimmickType).ToList();
                    break;
                }
            }
        }
    }
}

