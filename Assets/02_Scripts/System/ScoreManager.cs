using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> specialResourceTxts;
    [SerializeField] private TextMeshProUGUI scoreTxt;
    [SerializeField] private TextMeshProUGUI resultTxt;
    private List<double> gatheredResources;
    private double score;
    // private PlayHoneyGainCalculator honeyCalculator;
    private GameRecordManager recordManager;
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
            checkBest();
        });
        Debug.Log("??");
    }
    public void AddBloomScore(int idx, float score)
    {
        if (idx == 1)
        {
            gatheredResources[idx] += CalculateReward(Base_Manager.Data.UserData.Level);
        }
        else
        {
            gatheredResources[idx] += (Base_Manager.Data.UserData.Second_Base * (Base_Manager.Data.UserData.BuffFloating[1] > 0.0f ? 2.0f : 1.0f));
        }
        this.score += (Base_Manager.Data.UserData.Second_Base * (Base_Manager.Data.UserData.BuffFloating[1] > 0.0f ? 2.0f : 1.0f));
        if (idx != 0) { specialResourceTxts[idx-1].text = gatheredResources[idx].ToString(); }
        scoreTxt.text = StringMethod.ToCurrencyString(gatheredResources[0]);
    }

    int CalculateReward(int level)
    {
        // 로그 스케일 적용
        float normalizedLevel = Mathf.Log(level);  // 로그 적용으로 초반 급상승
        if (level >= 1000) normalizedLevel = Mathf.Log(1000);
        float maxLogLevel = Mathf.Log(1000);   // 최대 레벨 1000의 로그 값

        // 리워드 비율 계산 후 int로 변환
        int reward = Mathf.FloorToInt(Mathf.Lerp(1, 50, normalizedLevel / maxLogLevel));

        return reward;
    }
    private void checkBest()
    {
        double best = recordManager.GetBestRecord(getGameIndex());
        switch(getGameIndex())
        {
            case 0:
                if(Base_Manager.Data.UserData.BestScoreGameOne <= score)
                {
                    Base_Manager.Data.UserData.BestScoreGameOne = best;
                }
                break;
            case 1:
                if (Base_Manager.Data.UserData.BestScoreGameTwo <= score)
                {
                    Base_Manager.Data.UserData.BestScoreGameTwo = best;
                }
                break;
        }
        if (score > best)
        {
            recordManager.SetBestRecord(score, getGameIndex());
        }
    }
    private int getGameIndex()
    {
        string scnName = SceneManager.GetActiveScene().name;
        string num = scnName.Substring(scnName.Length - 3);
        int.TryParse(num, out int idx);
        idx--;
        return  idx;
    }
}
