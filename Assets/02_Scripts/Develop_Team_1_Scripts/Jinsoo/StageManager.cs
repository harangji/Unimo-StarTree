using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 인게임 Stage 난이도 관리 Class
/// </summary>
public class StageManager : MonoBehaviour
{
    [SerializeField] private ScoreGaugeController mScoreGauge;
    [SerializeField] private TextMeshProUGUI mStageText;
    
    // 지울 변수
    [SerializeField] private TextMeshProUGUI mLevelText;
    [SerializeField] private FlowerGenerator mFlowerCount;
    
    private StageData mStageData;
    private bool mbStageEnded = false;
    private float mTargetScore = 0f; // 목표 점수 저장
    
    private int mMaxDifficulty;
    private int mCurrentDifficulty;

    public bool GetBonusStage()
    {
        return mStageData.IsBonusStage;
    }
    
    private void Awake()
    {
        PlaySystemRefStorage.stageManager = this; // 전역 레퍼런스 설정
    }

    private void Start()
    {
        // 현재 선택된 스테이지 정보 가져오기
        mStageData = new StageData(StageLoader.CurrentStageNumber);

        // Debug.Log($"[StageManager] 스테이지 {mStageData.StageNumber} 시작");
        // Debug.Log($" - 제한 시간: {mStageData.TimeLimit}s");
        // Debug.Log($" - 난이도 수치: {mStageData.DifficultyValue}");
        // Debug.Log($" - 목표 점수 : {mStageData.TargetScore}");
        // Debug.Log($" - 최대 꽃 개수 : {mStageData.MaxFlowerCount}");
        // Debug.Log($" - 보스 스테이지 여부 : {mStageData.IsBossStage}");
        
        // 제한 시간 설정 (기존 PlayTimeManager에 전달)
        PlaySystemRefStorage.playTimeManager.SetStageTimeLimit(mStageData.TimeLimit);
        
        // 꽃 최대 갯수 제한
        FlowerGenerator_ST001.maxFlowers = mStageData.MaxFlowerCount;
        
        // 목표 점수 설정
        mTargetScore = mStageData.TargetScore;
        
        // 스코어 게이지 연결
        mScoreGauge.SetTargetScore(mTargetScore);
        
        // 점수 갱신 감시 이벤트 구독
        PlaySystemRefStorage.scoreManager.OnScoreChanged += HandleScoreChanged;
        
        // 게임 오버 이벤트
        PlaySystemRefStorage.playProcessController.SubscribeGameoverAction(OnTimeOver);

        // 여기서 필요한 매니저들에 난이도 등 전달 가능
        mMaxDifficulty = mStageData.DifficultyValue;
        mCurrentDifficulty = mMaxDifficulty;
        
        // 스테이지 표시 Text
        mStageText.text = mStageData.StageNumber.ToString();
        
        DeleteText();
    }
    
    // 나중에 지울 함수 (기획팀이 밸런스 패치를 위해 화면에 띄울 난이도 Text 함수)
    public void DeleteText()
    {
        mLevelText.text = mScoreGauge.GetCurrentScore() + " / " + mScoreGauge.GetTargetScore() + "\n"
                          + mCurrentDifficulty + " / " + mMaxDifficulty + "\n"
                          + mFlowerCount.AllFlowers.Count + " / " + mStageData.MaxFlowerCount;
    }

    // 풀 소모 시도
    public bool TryConsumeDifficulty(int cost)
    {
        if (mCurrentDifficulty >= cost)
        {
            mCurrentDifficulty -= cost;
            DeleteText();
            // Debug.Log($"[StageManager] 비용 사용 : {cost} → 현재 난이도 : {mCurrentDifficulty}");
            return true;
        }
        // Debug.Log($"cost가 부족합니다. 현재 cost => {mCurrentDifficulty}, 사용됐어야 할 cost => {cost}");
        return false;
    }

    // 몬스터 소멸 시 복구
    public void RestoreDifficulty(int cost)
    {
        mCurrentDifficulty = Mathf.Min(mCurrentDifficulty + cost, mMaxDifficulty);
        DeleteText();
        // Debug.Log($"[StageManager] 비용 복구 : {cost} → 현재 난이도 : {mCurrentDifficulty}");
    }
    
    // 목표 점수 도달 시 바로 클리어 처리
    private void HandleScoreChanged(double currentScore)
    {
        if (mbStageEnded) return;

        mScoreGauge.SetCurrentScore(currentScore);
        DeleteText();

        if (mStageData.IsBonusStage)
        {
            BonusStageStarCheck(currentScore);
        }
        
        if (currentScore >= mTargetScore)
        {
            mbStageEnded = true;

            Debug.Log("목표 점수 도달! 스테이지 클리어 처리");
            PlaySystemRefStorage.playTimeManager.StopTimer(); // 타이머 정지

            StageClear();
        }
    }

    private void BonusStageStarCheck(double currentScore)
    {
        var ratio = (float)currentScore / mStageData.TargetScore;
        
    }

    // 타임 오버 시 호출됨
    private void OnTimeOver()
    {
        if (mbStageEnded) return;

        mbStageEnded = true;
        
        if (mStageData.IsBonusStage)
        {
            StageClear();
            return;
        }

        Debug.Log("시간 종료! 스테이지 실패 처리");
        StageFail();
    }

    private void StageClear()
    {
        Debug.Log("스테이지 클리어!");
        StageLoader.SaveClearedStage(mStageData.StageNumber); // 다음 스테이지 오픈 저장
        // 보너스 스테이지 별 저장 처리
        if (mStageData.IsBonusStage)
        {
            int stageNum = mStageData.StageNumber;
            float ratio = (float)(mScoreGauge.GetScoreRatio());
            int newStars = 0;
            if (ratio >= 1.0f) newStars = 3;
            else if (ratio >= 0.66f) newStars = 2;
            else if (ratio >= 0.33f) newStars = 1;

            var userData = Base_Manager.Data.UserData;

            int oldFlags = 0;
            userData.BonusStageStars.TryGetValue(stageNum, out oldFlags);

            int rewardFlags = 0;
            int redReward = 0;
            int blueReward = 0;

            for (int i = 1; i <= newStars; i++)
            {
                int bit = 1 << (i - 1);
                if ((oldFlags & bit) == 0)
                {
                    // 보상 지급
                    switch (i)
                    {
                        case 1: 
                            Base_Manager.Data.UserData.Blue += 5;
                            blueReward += 5;
                            Debug.Log("별 달성 추가 보상 지급 : 5 Blue"); 
                            break;
                        case 2: 
                            Base_Manager.Data.UserData.Blue += 5; 
                            blueReward += 5;
                            Debug.Log("별 달성 추가 보상 지급 : 5 Blue"); 
                            break;
                        case 3: 
                            Base_Manager.Data.UserData.Blue += 10; 
                            blueReward += 10;
                            Debug.Log("별 달성 추가 보상 지급 : 10 Blue"); 
                            break;
                    }
                    rewardFlags |= bit;
                }
            }

            if (mScoreGauge.mNewStarAddBlueReward != null)
            {
                mScoreGauge.mNewStarAddBlueReward.SetText(blueReward.ToString());
            }
            
            // 새로 얻은 별 개수를 StarSum에 누적
            int newStarCount = CountBits(rewardFlags);
            if (newStarCount > 0)
            {
                userData.StarSum += newStarCount;
                Debug.Log($"[업적] 새로 획득한 별 {newStarCount}개 누적 → StarSum: {userData.StarSum}");
            }

            // 새로운 별 획득이 있었을 경우만 저장
            if (rewardFlags != 0 || newStars > CountBits(oldFlags))
            {
                int updatedFlag = oldFlags | rewardFlags;
                userData.BonusStageStars[stageNum] = updatedFlag;
                Base_Manager.Data.SaveUserData();
            }
            Debug.Log($"[보너스 저장] Stage {stageNum} - New Stars: {newStars}, Saved Flag: {userData.BonusStageStars[stageNum]}");
            Debug.Log(userData.BonusStageStars[stageNum]);
        }
        // 결과 UI 열기
        PlaySystemRefStorage.playProcessController.GameClear();
    }

    private void StageFail()
    {
        Debug.Log("스테이지 실패!");
        PlaySystemRefStorage.playProcessController.GameOver();
    }
    
    // 비트 수 카운트 유틸
    private int CountBits(int value)
    {
        var count = 0;
        for (var i = 0; i < 3; i++) // 최대 3비트
            if ((value & (1 << i)) != 0) count++;
        return count;
    }
    
    private float GetScoreRatio()
    {
        return (float)(mScoreGauge.GetCurrentScore() / mScoreGauge.GetTargetScore());
    }
    
    private void OnDestroy()
    {
        // 이벤트 해제
        if (PlaySystemRefStorage.scoreManager != null)
            PlaySystemRefStorage.scoreManager.OnScoreChanged -= HandleScoreChanged;
    }
}