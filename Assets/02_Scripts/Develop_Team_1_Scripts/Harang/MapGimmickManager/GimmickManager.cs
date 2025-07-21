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
    
    //��� ����s
    [LabelText("��� �˾�â"), SerializeField]
    private GameObject warningPopup;
    
    [LabelText("��� �ؽ�Ʈ��"), SerializeField]
    private List<TextMeshProUGUI> modeText;
    
    [LabelText("��� �����ܵ�"), SerializeField]
    private List<Image> modeIcon;
    
    [LabelText("��� ���� BGM ������ �ҽ�"), SerializeField]
    private AudioSource bgmSource; 
    
    [LabelText("��� ���� BGM"), SerializeField]
    private AudioClip modeBGM;
    
    [LabelText("�̷ο� ��� �ؽ�Ʈ ����"), SerializeField] 
    private Color goodModeTxtColor;
    
    [LabelText("�̷ο� ��� �̹��� ����"), SerializeField]
    private Color goodModeImgColor;

    [LabelText("��� �ʱ�ȭ�� �����Ҵ�"), SerializeField]
    private GimmickInitializer[] gimmickInitializers;
    
    [LabelText("��� �غ�� bool"), ReadOnly]
    private bool bIsReadyGimmiks = false;
    
    //ReadOnly
    [LabelText("���� �������� ĳ��"), ReadOnly]
    private int currentStage;
    
    [LabelText("���⵵ ����"), ReadOnly]
    private int currentCost;
    
    [LabelText("����� ��͵�"), ReadOnly]
    private List<Gimmick> readyGimmicks = new List<Gimmick>();

    private List<eGimmickGrade> canSelectAbleGrades = new List<eGimmickGrade>();
    
    private Dictionary<eGimmickGrade, List<Gimmick>> canSelectedGimmicksDictionary = new Dictionary<eGimmickGrade, List<Gimmick>>(); //����
    
    private int gimmickCount = 0;
    
    void Start()
    {
        InitializeGimmickManager();
    }

    public void InitializeGimmickManager()
    {
        currentStage = Base_Manager.Data.UserData.BestStage + 100; //�������� int ĳ�� //�ּ� 1 (�׽�Ʈ 100)
        
        if (SetGimmicCountByStageNumber(currentStage)) //�������� ���� ���� ������ - false�� ��� ��� ������ 0�̹Ƿ� ���� ����
        {
            List<GimmickSample> canSelectAbleGimmickSamples = GetCanSelectAbleGimmickSamplesByGrade(canSelectAbleGrades); //���õ� �� �ִ� 
            PickGimmickSamples(canSelectAbleGimmickSamples, gimmickCount);
            
            bIsReadyGimmiks = true;
        }
        else
        {
            bIsReadyGimmiks = false;
        }
    }
    
    
    private bool SetGimmicCountByStageNumber(int stageNumber) //currentStage�� ���� ����
    {
        int[] probabilities = new int[4]; // [0]=1�� Ȯ��, [1]=2��, [2]=3��, [3]=4��
        
        switch (stageNumber)
        {
            case <= 10:
                currentCost = 0; //���⵵ ����
                return false;
            
            case <= 50:
                currentCost = 10;
                probabilities = new[] { 100, 0, 0, 0 };
            
                canSelectAbleGrades.Add(eGimmickGrade.Nomal); //�� �� �ִ� ��� ��� ����Ʈ ����
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

        for (int i = 0; i < probabilities.Length; i++) //��÷
        {
            cumulative += probabilities[i]; //Ȯ�� ����
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
        
        foreach (var grade in canSelectGrades) //���� �� �ִ� Grade�� ���� ��ȸ
        {
            //�ϳ��� Grade�� gimmickInitializers ��� ��ȸ
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
                
                selectedGimmickSampless.Add(gimmickSample); //�����⸸ Add
            }
        }

        return selectedGimmickSampless;
        
        // for (int i = 0; i < readyGimmicks.Length; i++) //readyGimmicks ���̸�ŭ
        // {
        //     float rand = Random.Range(0f, totalWeight);
        //     float cumulative = 0f;
        //     
        //     foreach (var canSelectGimmick in canSelectedGimmicksDictionary) //��ųʸ� ��ȸ
        //     {
        //         foreach (var gimmick in canSelectGimmick.Value) //��� ����Ʈ
        //         {
        //             cumulative += gimmick.gimmickWeights[(int)canSelectGimmick.Key];
        //             if (rand < cumulative || testBool) //��÷ ����  //
        //             {
        //                 readyGimmicks[i] = gimmick; //�غ�� ��Ϳ� �߰�
        //                 readyGimmicks[i].SetGrade(canSelectGimmick.Key); //grade ����
        //                 
        //                 MyDebug.Log($"Tlqkf : {readyGimmicks[i].gimmickName}");
        //                 //ToDo : �ߺ�����?
        //             }
        //         }
        //     }
        // }
    } 
    
    //��� �̸� �غ��ϱ�
    public void PickGimmickSamples(List<GimmickSample> source, int count)
    {
        List<GimmickSample> pool = new List<GimmickSample>();
        
        List<eGimmickType> cashGimmickTypes = new List<eGimmickType>();
        
        for (int i = 0; i < count; i++) //count �� ���� // && pool.Count > 0
        {
            // ���� ���� �ĺ��� ������ �ߴ�
            // �̹� ���� ��� Ÿ���� ����
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
                cumulative += entry.GimmickWeight; //����ġ ����
                
                if (rand <= cumulative) //��÷
                {
                    Gimmick readyGimmick = entry.GimmickInitializer.InitializeGimmick(entry.GimmickGrade); //�����⸦ ���� ����
                    readyGimmicks.Add(readyGimmick); // �غ�� ��Ϳ� �߰�
                    
                    cashGimmickTypes.Add(entry.GimmickInitializer.GimmickType); // ���� Ÿ���� �߰�, �ߺ�����
                    
                    // ��ȸ ���� �ڷᱸ���� ������ �ٲ� ��� ������ �ʷ��ϹǷ�, if �񱳸� �Ź� �ϵ��� ����
                    // pool.Remove(entry); //��ȸ ����Ʈ���� ���ŷ� �ߺ� ���� - foreach������ �Ұ���
                    //Ÿ���� ��ġ���� �ʴ� ��͸� ����Ʈ�� ���� ��� = �ߺ� ����
                    // pool = pool.Where(x => x.GimmickInitializer.GimmickType != entry.GimmickInitializer.GimmickType).ToList();
                    break;
                }
            }
        }
    }
}

