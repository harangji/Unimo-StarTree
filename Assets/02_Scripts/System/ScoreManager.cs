using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Random = UnityEngine.Random;

public class ScoreManager : MonoBehaviour
{
    // 점수 갱신 시 이벤트 알림
    public event Action<double> OnScoreChanged;
    
    [SerializeField] private List<TextMeshProUGUI> specialResourceTxts;
    [SerializeField] private TextMeshProUGUI scoreTxt;
    [SerializeField] private TextMeshProUGUI resultTxt;
    [SerializeField] private EngineEffectTriggerManager triggerManager;
    
    private List<double> gatheredResources;
    private double score;
    // private PlayHoneyGainCalculator honeyCalculator;
    private GameRecordManager recordManager;
    
    [SerializeField] private float criticalChance; // 크리티컬 확률 (예시값)
    [SerializeField] private float criticalMult;   // 크리티컬 배율 (예시값)
    
    private int orangeFlowerCount = 0;
    private int yellowFlowerCount = 0;
    
    private void Awake()
    {
        PlaySystemRefStorage.scoreManager = this;
        gatheredResources = new List<double> { 0, 0 };
        score = 0;
    }
    
    private void Start()
    {
        //ToDo :: recordManager 대신 Easy Save사용하기 하랑
        // recordManager = FindAnyObjectByType<GameRecordManager>();
        // honeyCalculator = GetComponent<PlayHoneyGainCalculator>();
        
        for (int i = 1; i < gatheredResources.Count; i++)
        {
            specialResourceTxts[i-1].text = StringMethod.ToCurrencyString(gatheredResources[i]);
            // specialResourceTxts[i-1].text = gatheredResources[i].ToString();
        }
        PlaySystemRefStorage.playProcessController.SubscribeGameoverAction(() => 
        {
            for (int i = 1; i < gatheredResources.Count; i++)
            {
                specialResourceTxts[i-1].text = StringMethod.ToCurrencyString(gatheredResources[i]);
                // specialResourceTxts[i-1].text = gatheredResources[i].ToString();
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

        Debug.Log($"[ScoreManager] 스탯 적용됨 ▶ 크리티컬 확률 = {criticalChance}, 배율 = {criticalMult}");
    }
    
    public void AddBloomScore(int idx, float score)
    {
        float gainMultiplier = 1f;

        if (idx == 1) // 빨간 꽃
        {
            gainMultiplier = PlaySystemRefStorage.playerStatManager.GetStat().FinalStat.OFGainMult;
            orangeFlowerCount++;
            triggerManager.OnOrangeFlowerCollected();
        }
        else if (idx == 0) // 노란 꽃
        {
            gainMultiplier = PlaySystemRefStorage.playerStatManager.GetStat().FinalStat.YFGainMult;
            yellowFlowerCount++;
            triggerManager.OnYellowFlowerCollected();
        }

        float gain = score * gainMultiplier;

        Debug.Log($"[ScoreManager] 꽃 기본 점수 = {score} → 배율({gainMultiplier}) 적용 후: {gain}");

        // 크리티컬 판정
        if (Random.value < criticalChance)
        {
            gain *= criticalMult;  // 여기서 criticalMult는 0.1f면 10배.
            Debug.Log($"[ScoreManager] 크리티컬 발동! 배율 {criticalMult} → 최종 점수: {gain}");
        }

        gatheredResources[idx] += gain;
        this.score += gain;

        if (idx != 0)
        {
            specialResourceTxts[idx - 1].text = StringMethod.ToCurrencyString(gatheredResources[idx]);
            // specialResourceTxts[idx - 1].text = gatheredResources[idx].ToString();
        }

        scoreTxt.text = StringMethod.ToCurrencyString(gatheredResources[0]);

        OnScoreChanged?.Invoke(this.score);
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
    //        Debug.Log($"[ScoreManager] 빨간 꽃 수집: 보상 = {reward}");
    //    }
    //    else
    //    {
    //        double gain = Base_Manager.Data.UserData.Second_Base *
    //                      (Base_Manager.Data.UserData.BuffFloating[1] > 0.0f ? 2.0f : 1.0f);
    //        gatheredResources[idx] += gain;
    //        addedAmount = gain;
    //
    //        Debug.Log($"[ScoreManager] 노란 꽃 수집: 수익 = {gain}");
    //    }
    //
    //    double deltaScore = Base_Manager.Data.UserData.Second_Base *
    //                        (Base_Manager.Data.UserData.BuffFloating[1] > 0.0f ? 2.0f : 1.0f);
    //    this.score += deltaScore;
    //
    //    Debug.Log($"[ScoreManager] 누적 점수(score) 증가량 = {deltaScore}");
    //
    //    if (idx != 0)
    //        specialResourceTxts[idx - 1].text = gatheredResources[idx].ToString();
    //
    //    scoreTxt.text = StringMethod.ToCurrencyString(gatheredResources[0]);
    //}
    
    // int CalculateReward(int level)
    // {
    //     // 로그 스케일 적용
    //     float normalizedLevel = Mathf.Log(level);  // 로그 적용으로 초반 급상승
    //     if (level >= 1000) normalizedLevel = Mathf.Log(1000);
    //     float maxLogLevel = Mathf.Log(1000);   // 최대 레벨 1000의 로그 값
    //
    //     // 리워드 비율 계산 후 int로 변환
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
