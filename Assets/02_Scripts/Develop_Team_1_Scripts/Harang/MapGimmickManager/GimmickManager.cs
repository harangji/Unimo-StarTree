using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class GimmickManager : MonoBehaviour
{    
    // public TextAsset csvFile;
    
    [LabelText("��͵� �Ҵ�")][SerializeField] 
    private List<Gimmick> specialGimmicks;
    
    //��� ����s
    [LabelText("��� �˾�â")][SerializeField] 
    private GameObject warningPopup;
    
    [LabelText("��� �ؽ�Ʈ��")][SerializeField] 
    private List<TextMeshProUGUI> modeText;
    
    [LabelText("��� �����ܵ�")][SerializeField] 
    private List<Image> modeIcon;
    
    [LabelText("��� ���� BGM ������ �ҽ�")][ReadOnly] 
    private AudioSource bgmSource; 
    
    [LabelText("��� ���� BGM")][SerializeField] 
    private AudioClip modeBGM;
    
    [LabelText("�̷ο� ��� �ؽ�Ʈ ����")][SerializeField] 
    private Color goodModeTxtColor;
    
    [LabelText("�̷ο� ��� �̹��� ����")][SerializeField]
    private Color goodModeImgColor;

    public bool testBool = true;
    
    //ReadOnly
    [LabelText("���� �������� ĳ��")][ReadOnly]
    private int currentStage;
    
    [LabelText("���⵵ ����")][ReadOnly]
    private int currentCost;
    
    [LabelText("����� ��͵�")][ReadOnly]
    private Gimmick[] readyGimmicks;
    
    private List<Grade> canSelectGrades = new List<Grade>();
    private Dictionary<Grade, List<Gimmick>> canSelectedGimmicksDictionary = new Dictionary<Grade, List<Gimmick>>(); //����
    
    void Start()
    {
        InitializeGimmickManager();
    }

    public void InitializeGimmickManager()
    {
        
        currentStage = Base_Manager.Data.UserData.BestStage + 100; //�������� int ĳ�� //�ּ� 1 (�׽�Ʈ 100)
        currentCost = SetComplexityScoreWithStageNumber(currentStage); //currentStage�� ���⵵ ���� ��ȯ
        readyGimmicks = new Gimmick[SetGimmicCountByStageNumber(currentStage)]; //currentStage�� ���������� ������ ��� ���� ����
        if(readyGimmicks.Length == 0) return;
        SetCanSelectedGimmicksByGrade(); //��� �̸� �غ��ϱ�
    }


    
    private int SetComplexityScoreWithStageNumber(int stageNumber) //currentStage�� ���⵵ ���� ��ȯ
    {
        // �������� ����	���⵵ ���� (����)
        // 1-10	0��
        // 11 - 50	10��
        // 51 - 100	25��
        // 101 - 300	40��
        // 301 - 600	55��
        // 601 - 1000	70��
        
        //Swtich ���� ��Ī
        int result = stageNumber switch
        {
            <= 10 => 0,
            <= 50 => 10,
            <= 100 => 25,
            <= 300 => 40,
            <= 600 => 55,
            <= 1000 => 70,
            _ => 70  // 1000 �ʰ� �� �⺻��
        };
        
        return result;
    }
    
    private int SetGimmicCountByStageNumber(int stageNumber) //currentStage�� ��� ���� ��ȯ, ���� �� �ִ� ��� ����
    {
        if (stageNumber <= 10) return 0;
        
        int[] probabilities = new int[4]; // [0]=1�� Ȯ��, [1]=2��, [2]=3��, [3]=4��

        if (stageNumber <= 50)
        {
            probabilities = new[] { 100, 0, 0, 0 };
            
            canSelectGrades.Add(Grade.Nomal);
        }
        else if (stageNumber <= 100)
        {
            probabilities = new[] { 30, 70, 0, 0 };
            
            canSelectGrades.Add(Grade.Nomal);
            canSelectGrades.Add(Grade.Enhanced);
        }
        else if (stageNumber <= 300)
        {
            probabilities = new[] { 0, 30, 70, 0 };
            canSelectGrades.Add(Grade.Nomal);
            canSelectGrades.Add(Grade.Enhanced);
            canSelectGrades.Add(Grade.Elite);
        }
        else if (stageNumber <= 600)
        {
            probabilities = new[] { 0, 20, 70, 10 };
            
            canSelectGrades.Add(Grade.Enhanced);
            canSelectGrades.Add(Grade.Elite);
        }
        else if (stageNumber <= 1000)
        {
            probabilities = new[] { 0, 0, 40, 60 };
            
            canSelectGrades.Add(Grade.Elite);
            canSelectGrades.Add(Grade.Legendary);
        }
        
        int roll = Random.Range(1, 101); // 1~100
        int cumulative = 0; 

        for (int i = 0; i < probabilities.Length; i++) //��÷
        {
            cumulative += probabilities[i]; //Ȯ�� ����
            if (roll <= cumulative)
            {
                return i + 1; // 1��~4��
            }
        }

        return 4;
    }
    
    private void SetCanSelectedGimmicksByGrade()
    {
        int totalWeight = 0;
        List<Gimmick> selectedGimmicks = new List<Gimmick>();
        
        foreach (var grade in canSelectGrades) //���� �� �ִ� Grade�� ���� ��ȸ
        {
            foreach (var gimmick in specialGimmicks)
            {
                if (gimmick.gimmickCosts[(int)grade] <= currentCost) //�ڽ�Ʈ ���ؼ� ��밡���ϴٸ�
                {
                    totalWeight += gimmick.gimmickWeights[(int)grade]; //����ġ ����
                    selectedGimmicks.Add(gimmick);
                }
            }
            
            canSelectedGimmicksDictionary.Add(grade, selectedGimmicks);
        }
        
        for (int i = 0; i < readyGimmicks.Length; i++) //readyGimmicks ���̸�ŭ
        {
            float rand = Random.Range(0f, totalWeight);
            float cumulative = 0f;
            
            foreach (var canSelectGimmick in canSelectedGimmicksDictionary) //��ųʸ� ��ȸ
            {
                foreach (var gimmick in canSelectGimmick.Value) //��� ����Ʈ
                {
                    cumulative += gimmick.gimmickWeights[(int)canSelectGimmick.Key];
                    if (rand < cumulative || testBool) //��÷ ����  //
                    {
                        readyGimmicks[i] = gimmick; //�غ�� ��Ϳ� �߰�
                        readyGimmicks[i].SetGrade(canSelectGimmick.Key); //grade ����
                        
                        MyDebug.Log($"Tlqkf : {readyGimmicks[i].gimmickName}");
                        //ToDo : �ߺ�����?
                    }
                }
            }
        }
    } //��� �̸� �غ��ϱ�
}