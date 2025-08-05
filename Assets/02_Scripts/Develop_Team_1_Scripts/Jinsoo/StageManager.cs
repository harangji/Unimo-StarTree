using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �ΰ��� Stage ���̵� ���� Class
/// </summary>
public class StageManager : MonoBehaviour
{
    [SerializeField] private ScoreGaugeController mScoreGauge;
    [SerializeField] private TextMeshProUGUI mStageText;
    
    // ���� ����
    [SerializeField] private TextMeshProUGUI mLevelText;
    [SerializeField] private FlowerGenerator mFlowerCount;
    
    private StageData mStageData;
    private bool mbStageEnded = false;
    private float mTargetScore = 0f; // ��ǥ ���� ����
    
    private int mMaxDifficulty;
    private int mCurrentDifficulty;

    public bool GetBonusStage()
    {
        return mStageData.IsBonusStage;
    }
    
    private void Awake()
    {
        PlaySystemRefStorage.stageManager = this; // ���� ���۷��� ����
    }

    private void Start()
    {
        // ���� ���õ� �������� ���� ��������
        mStageData = new StageData(StageLoader.CurrentStageNumber);

        // Debug.Log($"[StageManager] �������� {mStageData.StageNumber} ����");
        // Debug.Log($" - ���� �ð�: {mStageData.TimeLimit}s");
        // Debug.Log($" - ���̵� ��ġ: {mStageData.DifficultyValue}");
        // Debug.Log($" - ��ǥ ���� : {mStageData.TargetScore}");
        // Debug.Log($" - �ִ� �� ���� : {mStageData.MaxFlowerCount}");
        // Debug.Log($" - ���� �������� ���� : {mStageData.IsBossStage}");
        
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
        mMaxDifficulty = mStageData.DifficultyValue;
        mCurrentDifficulty = mMaxDifficulty;
        
        // �������� ǥ�� Text
        mStageText.text = mStageData.StageNumber.ToString();
        
        DeleteText();
    }
    
    // ���߿� ���� �Լ� (��ȹ���� �뷱�� ��ġ�� ���� ȭ�鿡 ��� ���̵� Text �Լ�)
    public void DeleteText()
    {
        mLevelText.text = mScoreGauge.GetCurrentScore() + " / " + mScoreGauge.GetTargetScore() + "\n"
                          + mCurrentDifficulty + " / " + mMaxDifficulty + "\n"
                          + mFlowerCount.AllFlowers.Count + " / " + mStageData.MaxFlowerCount;
    }

    // Ǯ �Ҹ� �õ�
    public bool TryConsumeDifficulty(int cost)
    {
        if (mCurrentDifficulty >= cost)
        {
            mCurrentDifficulty -= cost;
            DeleteText();
            // Debug.Log($"[StageManager] ��� ��� : {cost} �� ���� ���̵� : {mCurrentDifficulty}");
            return true;
        }
        // Debug.Log($"cost�� �����մϴ�. ���� cost => {mCurrentDifficulty}, ���ƾ�� �� cost => {cost}");
        return false;
    }

    // ���� �Ҹ� �� ����
    public void RestoreDifficulty(int cost)
    {
        mCurrentDifficulty = Mathf.Min(mCurrentDifficulty + cost, mMaxDifficulty);
        DeleteText();
        // Debug.Log($"[StageManager] ��� ���� : {cost} �� ���� ���̵� : {mCurrentDifficulty}");
    }
    
    // ��ǥ ���� ���� �� �ٷ� Ŭ���� ó��
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
        
        if (mStageData.IsBonusStage)
        {
            StageClear();
            return;
        }

        Debug.Log("�ð� ����! �������� ���� ó��");
        StageFail();
    }

    private void StageClear()
    {
        Debug.Log("�������� Ŭ����!");
        StageLoader.SaveClearedStage(mStageData.StageNumber); // ���� �������� ���� ����
        // ���ʽ� �������� �� ���� ó��
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
                    // ���� ����
                    switch (i)
                    {
                        case 1: 
                            Base_Manager.Data.UserData.Blue += 5;
                            blueReward += 5;
                            Debug.Log("�� �޼� �߰� ���� ���� : 5 Blue"); 
                            break;
                        case 2: 
                            Base_Manager.Data.UserData.Blue += 5; 
                            blueReward += 5;
                            Debug.Log("�� �޼� �߰� ���� ���� : 5 Blue"); 
                            break;
                        case 3: 
                            Base_Manager.Data.UserData.Blue += 10; 
                            blueReward += 10;
                            Debug.Log("�� �޼� �߰� ���� ���� : 10 Blue"); 
                            break;
                    }
                    rewardFlags |= bit;
                }
            }

            if (mScoreGauge.mNewStarAddBlueReward != null)
            {
                mScoreGauge.mNewStarAddBlueReward.SetText(blueReward.ToString());
            }
            
            // ���� ���� �� ������ StarSum�� ����
            int newStarCount = CountBits(rewardFlags);
            if (newStarCount > 0)
            {
                userData.StarSum += newStarCount;
                Debug.Log($"[����] ���� ȹ���� �� {newStarCount}�� ���� �� StarSum: {userData.StarSum}");
            }

            // ���ο� �� ȹ���� �־��� ��츸 ����
            if (rewardFlags != 0 || newStars > CountBits(oldFlags))
            {
                int updatedFlag = oldFlags | rewardFlags;
                userData.BonusStageStars[stageNum] = updatedFlag;
                Base_Manager.Data.SaveUserData();
            }
            Debug.Log($"[���ʽ� ����] Stage {stageNum} - New Stars: {newStars}, Saved Flag: {userData.BonusStageStars[stageNum]}");
            Debug.Log(userData.BonusStageStars[stageNum]);
        }
        // ��� UI ����
        PlaySystemRefStorage.playProcessController.GameClear();
    }

    private void StageFail()
    {
        Debug.Log("�������� ����!");
        PlaySystemRefStorage.playProcessController.GameOver();
    }
    
    // ��Ʈ �� ī��Ʈ ��ƿ
    private int CountBits(int value)
    {
        var count = 0;
        for (var i = 0; i < 3; i++) // �ִ� 3��Ʈ
            if ((value & (1 << i)) != 0) count++;
        return count;
    }
    
    private float GetScoreRatio()
    {
        return (float)(mScoreGauge.GetCurrentScore() / mScoreGauge.GetTargetScore());
    }
    
    private void OnDestroy()
    {
        // �̺�Ʈ ����
        if (PlaySystemRefStorage.scoreManager != null)
            PlaySystemRefStorage.scoreManager.OnScoreChanged -= HandleScoreChanged;
    }
}