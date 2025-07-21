using System;
using UnityEngine;
using UnityEngine.UI;

// 인게임 Stage 난이도 관리.
public class StageManager : MonoBehaviour
{
    [SerializeField] private ScoreGaugeController mScoreGauge;
    
    private StageData mStageData;
    private bool mbStageEnded = false;
    private float mTargetScore = 0f; // 목표 점수 저장
    
    private int mMaxDifficulty;
    private int mCurrentDifficulty;

    private void Awake()
    {
        PlaySystemRefStorage.stageManager = this; // 전역 레퍼런스 설정
    }

    private void Start()
    {
        // 현재 선택된 스테이지 정보 가져오기
        mStageData = new StageData(StageLoader.CurrentStageNumber);

        Debug.Log($"[StageManager] 스테이지 {mStageData.StageNumber} 시작");
        Debug.Log($" - 제한 시간: {mStageData.TimeLimit}s");
        Debug.Log($" - 난이도 수치: {mStageData.DifficultyValue}");
        Debug.Log($" - 목표 점수 : {mStageData.TargetScore}");
        Debug.Log($" - 최대 꽃 개수 : {mStageData.MaxFlowerCount}");
        Debug.Log($" - 보스 스테이지 여부 : {mStageData.IsBossStage}");
        
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
        // 예: MonsterGenerator.SetDifficulty(mStageData.DifficultyValue);
        mMaxDifficulty = mStageData.DifficultyValue;
        mCurrentDifficulty = mMaxDifficulty;
    }

    // 풀 소모 시도
    public bool TryConsumeDifficulty(int cost)
    {
        if (mCurrentDifficulty >= cost)
        {
            mCurrentDifficulty -= cost;
            Debug.Log($"[StageManager] 비용 사용 : {cost} → 현재 난이도 : {mCurrentDifficulty}");
            return true;
        }
        Debug.Log($"cost가 부족합니다. 현재 cost => {mCurrentDifficulty}, 사용됐어야 할 cost => {cost}");
        return false;
    }

    // 몬스터 소멸 시 복구
    public void RestoreDifficulty(int cost)
    {
        mCurrentDifficulty = Mathf.Min(mCurrentDifficulty + cost, mMaxDifficulty);
        Debug.Log($"[StageManager] 비용 복구 : {cost} → 현재 난이도 : {mCurrentDifficulty}");
    }
    
    // 목표 점수 도달 시 바로 클리어 처리
    private void HandleScoreChanged(double currentScore)
    {
        if (mbStageEnded) return;

        mScoreGauge.SetCurrentScore(currentScore);

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

        Debug.Log("시간 종료! 스테이지 실패 처리");
        StageFail();
    }

    private void StageClear()
    {
        Debug.Log("스테이지 클리어!");
        StageLoader.SaveClearedStage(mStageData.StageNumber); // 다음 스테이지 오픈 저장
        // 결과 UI 열기
        PlaySystemRefStorage.playProcessController.GameClear();
    }

    private void StageFail()
    {
        Debug.Log("스테이지 실패!");
        // 결과 UI 열기
        if (mStageData.IsBonusStage)
        {
            StageClear();
        }
        else
        {
            PlaySystemRefStorage.playProcessController.GameOver();
        }
    }
    
    private void OnDestroy()
    {
        // 이벤트 해제
        if (PlaySystemRefStorage.scoreManager != null)
            PlaySystemRefStorage.scoreManager.OnScoreChanged -= HandleScoreChanged;
    }
}