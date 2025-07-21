using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Random = UnityEngine.Random;

public class ScoreManager : MonoBehaviour
{
    // ���� ���� �� �̺�Ʈ �˸�
    public event Action<double> OnScoreChanged;
    
    [SerializeField] private List<TextMeshProUGUI> specialResourceTxts;
    [SerializeField] private TextMeshProUGUI scoreTxt;
    [SerializeField] private TextMeshProUGUI resultTxt;
    private List<double> gatheredResources;
    private double score;
    // private PlayHoneyGainCalculator honeyCalculator;
    private GameRecordManager recordManager;
    
    [SerializeField] private float criticalChance; // ũ��Ƽ�� Ȯ�� (���ð�)
    [SerializeField] private float criticalMult;   // ũ��Ƽ�� ���� (���ð�)
    
    
    private void Awake()
    {
        PlaySystemRefStorage.scoreManager = this;
        gatheredResources = new List<double> { 0, 0 };
        score = 0;
    }
    
    private void Start()
    {
        //ToDo :: recordManager ��� Easy Save����ϱ� �϶�
        // recordManager = FindAnyObjectByType<GameRecordManager>();
        // honeyCalculator = GetComponent<PlayHoneyGainCalculator>();
        
        for (int i = 1; i < gatheredResources.Count; i++)
        {
            specialResourceTxts[i-1].text = gatheredResources[i].ToString();
        }
        PlaySystemRefStorage.playProcessController.SubscribeGameoverAction(() => 
        {
            for (int i = 1; i < gatheredResources.Count; i++)
            {
                specialResourceTxts[i-1].text = gatheredResources[i].ToString();
            }
    
            scoreTxt.text = StringMethod.ToCurrencyString(gatheredResources[0]);
            resultTxt.text = StringMethod.ToCurrencyString(gatheredResources[0]);
            // checkBest();
        });
        Debug.Log("??");
    }
    
    public void ApplyStatFromCharacter(UnimoRuntimeStat stat)
    {
        criticalChance = stat.FinalStat.CriticalChance;
        criticalMult = stat.FinalStat.CriticalMult;

        Debug.Log($"[ScoreManager] ���� ����� �� ũ��Ƽ�� Ȯ�� = {criticalChance}, ���� = {criticalMult}");
    }
    
    public void AddBloomScore(int idx, float score)
    {
        if (idx == 1) // ���� ��
        {
            // int baseReward = CalculateReward(Base_Manager.Data.UserData.Level);

            if (Random.value < criticalChance)
            {
                score = Mathf.FloorToInt(score * criticalMult);
                Debug.Log($"[ScoreManager]  ���� �� ũ��Ƽ�� �ߵ�! x{criticalMult} �� {score}");
            }

            gatheredResources[idx] += score;
            this.score += score;
        }
        else if (idx == 0) // ��� ��
        {
            // double gain = score * (Base_Manager.Data.UserData.BuffFloating[1] > 0.0f ? 2.0f : 1.0f);
            float gain = score;

            if (Random.value < criticalChance)
            {
                gain *= criticalMult;
                Debug.Log($"[ScoreManager]  ��� �� ũ��Ƽ�� �ߵ�! x{criticalMult} �� {gain}");
            }

            gatheredResources[idx] += gain;
            this.score += gain;
        }

        if (idx != 0)
        {
            specialResourceTxts[idx - 1].text = gatheredResources[idx].ToString();
        }

        scoreTxt.text = StringMethod.ToCurrencyString(gatheredResources[0]);
        
        // ���� ���� �̺�Ʈ ȣ��
        OnScoreChanged?.Invoke(this.score);
        Debug.Log(this.score);
    }
    
    
    
    //public void AddBloomScore(int idx, float score)
    //{
    //    double addedAmount = 0;
    //
    //    if (idx == 1)
    //    {
    //        int reward = CalculateReward(Base_Manager.Data.UserData.Level);
    //        gatheredResources[idx] += reward;
    //        addedAmount = reward;
    //
    //        Debug.Log($"[ScoreManager] ���� �� ����: ���� = {reward}");
    //    }
    //    else
    //    {
    //        double gain = Base_Manager.Data.UserData.Second_Base *
    //                      (Base_Manager.Data.UserData.BuffFloating[1] > 0.0f ? 2.0f : 1.0f);
    //        gatheredResources[idx] += gain;
    //        addedAmount = gain;
    //
    //        Debug.Log($"[ScoreManager] ��� �� ����: ���� = {gain}");
    //    }
    //
    //    double deltaScore = Base_Manager.Data.UserData.Second_Base *
    //                        (Base_Manager.Data.UserData.BuffFloating[1] > 0.0f ? 2.0f : 1.0f);
    //    this.score += deltaScore;
    //
    //    Debug.Log($"[ScoreManager] ���� ����(score) ������ = {deltaScore}");
    //
    //    if (idx != 0)
    //        specialResourceTxts[idx - 1].text = gatheredResources[idx].ToString();
    //
    //    scoreTxt.text = StringMethod.ToCurrencyString(gatheredResources[0]);
    //}
    
    // int CalculateReward(int level)
    // {
    //     // �α� ������ ����
    //     float normalizedLevel = Mathf.Log(level);  // �α� �������� �ʹ� �޻��
    //     if (level >= 1000) normalizedLevel = Mathf.Log(1000);
    //     float maxLogLevel = Mathf.Log(1000);   // �ִ� ���� 1000�� �α� ��
    //
    //     // ������ ���� ��� �� int�� ��ȯ
    //     int reward = Mathf.FloorToInt(Mathf.Lerp(1, 50, normalizedLevel / maxLogLevel));
    //
    //     return reward;
    // }
    // private void checkBest()
    // {
    //     double best = recordManager.GetBestRecord(getGameIndex());
    //     switch(getGameIndex())
    //     {
    //         case 0:
    //             if(Base_Manager.Data.UserData.BestScoreGameOne <= score)
    //             {
    //                 Base_Manager.Data.UserData.BestScoreGameOne = best;
    //             }
    //             break;
    //         case 1:
    //             if (Base_Manager.Data.UserData.BestScoreGameTwo <= score)
    //             {
    //                 Base_Manager.Data.UserData.BestScoreGameTwo = best;
    //             }
    //             break;
    //     }
    //     if (score > best)
    //     {
    //         recordManager.SetBestRecord(score, getGameIndex());
    //     }
    // }
    private int getGameIndex()
    {
        string scnName = SceneManager.GetActiveScene().name;
        string num = scnName.Substring(scnName.Length - 3);
        int.TryParse(num, out int idx);
        idx--;
        return  idx;
    }
}
