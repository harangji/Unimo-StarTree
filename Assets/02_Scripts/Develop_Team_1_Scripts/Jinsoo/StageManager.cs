using System;
using UnityEngine;
using UnityEngine.UI;

// �ΰ��� Stage ���̵� ����.
public class StageManager : MonoBehaviour
{
    [SerializeField] private ScoreGaugeController mScoreGauge;
    
    private StageData mStageData;
    private bool mbStageEnded = false;
    private float mTargetScore = 0f; // ��ǥ ���� ����
    
    private int mMaxDifficulty;
    private int mCurrentDifficulty;

    private void Awake()
    {
        PlaySystemRefStorage.stageManager = this; // ���� ���۷��� ����
    }

    private void Start()
    {
        // ���� ���õ� �������� ���� ��������
        mStageData = new StageData(StageLoader.CurrentStageNumber);

        Debug.Log($"[StageManager] �������� {mStageData.StageNumber} ����");
        Debug.Log($" - ���� �ð�: {mStageData.TimeLimit}s");
        Debug.Log($" - ���̵� ��ġ: {mStageData.DifficultyValue}");
        Debug.Log($" - ��ǥ ���� : {mStageData.TargetScore}");
        Debug.Log($" - �ִ� �� ���� : {mStageData.MaxFlowerCount}");
        Debug.Log($" - ���� �������� ���� : {mStageData.IsBossStage}");
        
        // ���� �ð� ���� (���� PlayTimeManager�� ����)
        PlaySystemRefStorage.playTimeManager.SetStageTimeLimit(mStageData.TimeLimit);
        
        // �� �ִ� ���� ����
        FlowerGenerator_ST001.maxFlowers = mStageData.MaxFlowerCount;
        
        // ��ǥ ���� ����
        mTargetScore = mStageData.TargetScore;
        
        // ���ھ� ������ ����
        mScoreGauge.SetTargetScore(mTargetScore);
        
        // ���� ���� ���� �̺�Ʈ ����
        PlaySystemRefStorage.scoreManager.OnScoreChanged += HandleScoreChanged;
        
        // ���� ���� �̺�Ʈ
        PlaySystemRefStorage.playProcessController.SubscribeGameoverAction(OnTimeOver);

        // ���⼭ �ʿ��� �Ŵ����鿡 ���̵� �� ���� ����
        // ��: MonsterGenerator.SetDifficulty(mStageData.DifficultyValue);
        mMaxDifficulty = mStageData.DifficultyValue;
        mCurrentDifficulty = mMaxDifficulty;
    }

    // Ǯ �Ҹ� �õ�
    public bool TryConsumeDifficulty(int cost)
    {
        if (mCurrentDifficulty >= cost)
        {
            mCurrentDifficulty -= cost;
            Debug.Log($"[StageManager] ��� ��� : {cost} �� ���� ���̵� : {mCurrentDifficulty}");
            return true;
        }
        Debug.Log($"cost�� �����մϴ�. ���� cost => {mCurrentDifficulty}, ���ƾ�� �� cost => {cost}");
        return false;
    }

    // ���� �Ҹ� �� ����
    public void RestoreDifficulty(int cost)
    {
        mCurrentDifficulty = Mathf.Min(mCurrentDifficulty + cost, mMaxDifficulty);
        Debug.Log($"[StageManager] ��� ���� : {cost} �� ���� ���̵� : {mCurrentDifficulty}");
    }
    
    // ��ǥ ���� ���� �� �ٷ� Ŭ���� ó��
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

            Debug.Log("��ǥ ���� ����! �������� Ŭ���� ó��");
            PlaySystemRefStorage.playTimeManager.StopTimer(); // Ÿ�̸� ����

            StageClear();
        }
    }

    private void BonusStageStarCheck(double currentScore)
    {
        var ratio = (float)currentScore / mStageData.TargetScore;
        
    }

    // Ÿ�� ���� �� ȣ���
    private void OnTimeOver()
    {
        if (mbStageEnded) return;

        mbStageEnded = true;

        Debug.Log("�ð� ����! �������� ���� ó��");
        StageFail();
    }

    private void StageClear()
    {
        Debug.Log("�������� Ŭ����!");
        StageLoader.SaveClearedStage(mStageData.StageNumber); // ���� �������� ���� ����
        // ��� UI ����
        PlaySystemRefStorage.playProcessController.GameClear();
    }

    private void StageFail()
    {
        Debug.Log("�������� ����!");
        // ��� UI ����
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
        // �̺�Ʈ ����
        if (PlaySystemRefStorage.scoreManager != null)
            PlaySystemRefStorage.scoreManager.OnScoreChanged -= HandleScoreChanged;
    }
}