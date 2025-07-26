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
    [LabelText("���� �������� ĳ��"), ShowInInspector, ReadOnly]
    private int mCurrentStage;
    
    [LabelText("���⵵ ����"), ShowInInspector, ReadOnly]
    private int mCurrentCost;
    
    [LabelText("����� ��͵�"), ShowInInspector, ReadOnly]
    private Queue<Gimmick> mReadyGimmicks = new Queue<Gimmick>();
    
    private List<eGimmickGrade> mCanSelectAbleGrades = new List<eGimmickGrade>(); //���� ������ ��� ���
    
    private Dictionary<eGimmickGrade, List<Gimmick>> mCanSelectedGimmicksDictionary = new Dictionary<eGimmickGrade, List<Gimmick>>(); //����

    
    private int gimmickCount = 0;
    public static GimmickManager Instance { get; private set; }

    private Dictionary<string, GimmickInitializer> mGimmickInitializersMapper;
    
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
            { "G_TRP_002", gimmickInitializers[0] }, // ��Ȧ
            { "G_ENV_004", gimmickInitializers[1] } // ������
        };
    }

    void Start()
    {
        mReadyGimmicks.Clear();
        
        InitializeGimmickManager(); //��� �̱�
    }

    public void ExcuteGimmick() //�ٸ� ������ ����
    {
        if (bIsReadyGimmiks)
        {
            PlaySceneController.Instance.PauseGameSingleton();
            //��� ���� ���� �ʿ� ToDo:;
            mReadyGimmicks.Dequeue().ActivateGimmick(); //��� Ȱ��ȭ
            PlaySceneController.Instance.ResumeGameSingleton();
        }
    }
    
    public void InitializeGimmickManager()
    {
        mCurrentStage = Base_Manager.Data.UserData.BestStage + 100; //�������� int ĳ�� //�ּ� 1 (�׽�Ʈ 100)
        
        if (SetGimmicCountByStageNumber(mCurrentStage)) //�������� ���� ���� ������ - false�� ��� ��� ������ 0�̹Ƿ� ���� ����
        {
            List<GimmickSample> canSelectAbleGimmickSamples = GetCanSelectAbleGimmickSamplesByGrade(mCanSelectAbleGrades); //��� ������ ��ȯ
            PickGimmickSamples(canSelectAbleGimmickSamples, gimmickCount); //��� ������ ������� ����

            ToTsvGimmickData[] toTsvGimmickDatas = gameDataSoHolder.GimmicParsingDataSo.GetDataAll();
            for (int i = 0; i < toTsvGimmickDatas.Length; i++)
            {
                //key�� Initializer �����Ͽ� �ʱ�ȭ
                mGimmickInitializersMapper[toTsvGimmickDatas[i].Key].GimmickInitializerSetting(toTsvGimmickDatas[i]);
            }
            
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
                mCurrentCost = 0; //���⵵ ����
                return false;
            
            case <= 50:
                mCurrentCost = 10;
                probabilities = new[] { 100, 0, 0, 0 }; //���� Ȯ�� ����
            
                mCanSelectAbleGrades.Add(eGimmickGrade.Nomal); //�� �� �ִ� ��� ��� ����Ʈ ����
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
                if (gimmickInitializer.Costs[(int)grade] > mCurrentCost) continue;
                
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
    } 
    
    //��� �̸� �غ��ϱ�
    public void PickGimmickSamples(List<GimmickSample> source, int count)
    {
        List<eGimmickType> cashGimmickTypes = new List<eGimmickType>();  //�ߺ�����
        
        for (int i = 0; i < count; i++) //count �� ���� // && pool.Count > 0
        {
            // ���� ���� �ĺ��� ������ �ߴ�
            // �̹� ���� ��� Ÿ���� ����
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
                cumulative += entry.GimmickWeight; //����ġ ����
                
                if (rand <= cumulative) //��÷
                {
                    Gimmick readyGimmick = entry.GimmickInitializer.InitializeGimmick(entry.GimmickGrade); //�����⸦ ���� ����
                    mReadyGimmicks.Enqueue(readyGimmick); // �غ�� ��Ϳ� �߰�
                    
                    cashGimmickTypes.Add(entry.GimmickInitializer.GimmickType); // ���� Ÿ���� �߰�, �ߺ�����
                    
                    // ��ȸ ���� �ڷᱸ���� ������ �ٲ� ��� ������ �ʷ��ϹǷ� ����
                    // pool.Remove(entry); //��ȸ ����Ʈ���� ���ŷ� �ߺ� ���� - foreach������ �Ұ���
                    //Ÿ���� ��ġ���� �ʴ� ��͸� ����Ʈ�� ���� ��� = �ߺ� ����
                    // pool = pool.Where(x => x.GimmickInitializer.GimmickType != entry.GimmickInitializer.GimmickType).ToList();
                    break;
                }
            }
        }
    }
}

