// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Globalization;
// using System.IO;
// using Sirenix.OdinInspector;
// using TMPro;
// using UnityEngine;
// using UnityEngine.UI;
// using Random = UnityEngine.Random;
//
//
// public class GimmickManager : MonoBehaviour
// {    
//     // public TextAsset csvFile;
//     
//     [LabelText("기믹들 할당")][SerializeField] 
//     private List<Gimmick> specialGimmicks;
//     
//     //경고 연출s
//     [LabelText("경고 팝업창")][SerializeField] 
//     private GameObject warningPopup;
//     
//     [LabelText("기믹 텍스트들")][SerializeField] 
//     private List<TextMeshProUGUI> modeText;
//     
//     [LabelText("기믹 아이콘들")][SerializeField] 
//     private List<Image> modeIcon;
//     
//     [LabelText("기믹 출현 BGM 실행할 소스")][ReadOnly] 
//     private AudioSource bgmSource; 
//     
//     [LabelText("기믹 출현 BGM")][SerializeField] 
//     private AudioClip modeBGM;
//     
//     [LabelText("이로운 기믹 텍스트 색상")][SerializeField] 
//     private Color goodModeTxtColor;
//     
//     [LabelText("이로운 기믹 이미지 색상")][SerializeField]
//     private Color goodModeImgColor;
//
//     public bool testBool = true;
//     
//     //ReadOnly
//     [LabelText("현재 스테이지 캐싱")][ReadOnly]
//     private int currentStage;
//     
//     [LabelText("복잡도 점수")][ReadOnly]
//     private int currentCost;
//     
//     [LabelText("골라진 기믹들")][ReadOnly]
//     private Gimmick[] readyGimmicks;
//     
//     private int[] gradeWeights = new []
//     {
//         50,
//         30,
//         20,
//         10,
//     };
//     
//     private List<GimmickGrade> canSelectGrades = new List<GimmickGrade>();
//     private Dictionary<GimmickGrade, List<Gimmick>> canSelectedGimmicksDictionary = new Dictionary<GimmickGrade, List<Gimmick>>(); //예비군
//
//     private GimmickInitializer[] gimmickFactories = new GimmickInitializer[]
//     {
//         new BlackHoleGimmickFactory(),
//         new EnhancedGimmickFactory(),
//         new EliteGimmickFactory(),
//         new LegendaryGimmickFactory()
//     };
//     
//     void Start()
//     {
//         InitializeGimmickManager();
//     }
//
//     public void InitializeGimmickManager()
//     {
//         currentStage = Base_Manager.Data.UserData.BestStage + 100; //스테이지 int 캐싱 //최소 1 (테스트 100)
//         currentCost = SetComplexityScoreWithStageNumber(currentStage); //currentStage로 복잡도 점수 반환
//         
//         int gimmickCount = SetGimmicCountByStageNumber(currentStage); //기믹 갯수 반환 & canSelectGrades에 등급 설정
//         if(gimmickCount <= 0) return;
//         
//         readyGimmicks = new Gimmick[gimmickCount]; //currentStage로 스테이지에 등장할 기믹 갯수 설정
//
//         for (int i = 0; i < readyGimmicks.Length; i++)
//         {
//             SetCanSelectedGimmicksByGrade();
//         }
//         //기믹 미리 준비하기
//     }
//     
//     private int SetComplexityScoreWithStageNumber(int stageNumber) //currentStage로 복잡도 점수 반환
//     {
//         // 스테이지 구간	복잡도 점수 (예산)
//         // 1-10	0점
//         // 11 - 50	10점
//         // 51 - 100	25점
//         // 101 - 300	40점
//         // 301 - 600	55점
//         // 601 - 1000	70점
//         
//         //Swtich 패턴 매칭
//         int result = stageNumber switch
//         {
//             <= 10 => 0,
//             <= 50 => 10,
//             <= 100 => 25,
//             <= 300 => 40,
//             <= 600 => 55,
//             <= 1000 => 70,
//             _ => 70  // 1000 초과 시 기본값
//         };
//         
//         return result;
//     }
//     
//     private int SetGimmicCountByStageNumber(int stageNumber) //currentStage로 기믹 갯수 반환, 나올 수 있는 등급 설정
//     {
//         if (stageNumber <= 10) return 0;
//         
//         int[] probabilities = new int[4]; // [0]=1개 확률, [1]=2개, [2]=3개, [3]=4개
//
//         if (stageNumber <= 50)
//         {
//             probabilities = new[] { 100, 0, 0, 0 };
//             
//             canSelectGrades.Add(GimmickGrade.Nomal);
//         }
//         else if (stageNumber <= 100)
//         {
//             probabilities = new[] { 30, 70, 0, 0 };
//             
//             canSelectGrades.Add(GimmickGrade.Nomal);
//             canSelectGrades.Add(GimmickGrade.Enhanced);
//         }
//         else if (stageNumber <= 300)
//         {
//             probabilities = new[] { 0, 30, 70, 0 };
//             canSelectGrades.Add(GimmickGrade.Nomal);
//             canSelectGrades.Add(GimmickGrade.Enhanced);
//             canSelectGrades.Add(GimmickGrade.Elite);
//         }
//         else if (stageNumber <= 600)
//         {
//             probabilities = new[] { 0, 20, 70, 10 };
//             
//             canSelectGrades.Add(GimmickGrade.Enhanced);
//             canSelectGrades.Add(GimmickGrade.Elite);
//         }
//         else if (stageNumber <= 1000)
//         {
//             probabilities = new[] { 0, 0, 40, 60 };
//             
//             canSelectGrades.Add(GimmickGrade.Elite);
//             canSelectGrades.Add(GimmickGrade.Legendary);
//         }
//         
//         int roll = Random.Range(1, 101); // 1~100
//         int cumulative = 0; 
//
//         for (int i = 0; i < probabilities.Length; i++) //추첨
//         {
//             cumulative += probabilities[i]; //확률 저장
//             if (roll <= cumulative)
//             {
//                 return i + 1; // 1개~4개
//             }
//         }
//
//         return 4;
//     }
//     
//     private void SetCanSelectedGimmicksByGrade()
//     {
//         int totalWeight = 0;
//         List<Gimmick> selectedGimmicks = new List<Gimmick>();
//         
//         foreach (var grade in canSelectGrades) //나올 수 있는 Grade를 전부 조회
//         {
//             // foreach (var gimmick in specialGimmicks)
//             // {
//             //     if (gimmick.gimmickCosts[(int)grade] <= currentCost) //코스트 비교해서 사용가능하다면
//             //     {
//             //         totalWeight += gimmick.gimmickWeights[(int)grade]; //가중치 더함
//             //         selectedGimmicks.Add(gimmick);
//             //     }
//             // }
//             float angle = Mathf.PI / 4f * i;
//             Vector3 pos = center + offset * new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
//             Quaternion rot = Quaternion.LookRotation(center - pos, Vector3.up);
//             var ctrl = Instantiate(prefab, pos, rot).GetComponent<MonsterController>();
//             
//             gimmickFactories[(int)grade].CreateGimmick();
//             
//             canSelectedGimmicksDictionary.Add(grade, selectedGimmicks);
//         }
//         //
//         // for (int i = 0; i < readyGimmicks.Length; i++) //readyGimmicks 길이만큼
//         // {
//         //     float rand = Random.Range(0f, totalWeight);
//         //     float cumulative = 0f;
//         //     
//         //     foreach (var canSelectGimmick in canSelectedGimmicksDictionary) //딕셔너리 순회
//         //     {
//         //         foreach (var gimmick in canSelectGimmick.Value) //기믹 리스트
//         //         {
//         //             cumulative += gimmick.gimmickWeights[(int)canSelectGimmick.Key];
//         //             if (rand < cumulative || testBool) //추첨 성공  //
//         //             {
//         //                 readyGimmicks[i] = gimmick; //준비된 기믹에 추가
//         //                 readyGimmicks[i].SetGrade(canSelectGimmick.Key); //grade 세팅
//         //                 
//         //                 MyDebug.Log($"Tlqkf : {readyGimmicks[i].gimmickName}");
//         //                 //ToDo : 중복제거?
//         //             }
//         //         }
//         //     }
//         // }
//     } //기믹 미리 준비하기
// }
//
