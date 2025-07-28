using System;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

/// <summary>
/// �� ���� �� ���������� ���� Class
/// </summary>
public class ScoreGaugeController : MonoBehaviour
{
    [SerializeField] private Image mFillTopImage;   // ������ �̹��� (Filled ���)
    [SerializeField] private Image mFillBottomImage;   // ������ �̹��� (Filled ���)
    // [SerializeField] private Text scoreText;    // ���� ǥ�� (���û���)
    [SerializeField] private Image[] mStarImages;
    [SerializeField] [CanBeNull] private Image[] mGameClearStarImages;
    [SerializeField] private Image mEmptyStarImage;
    [SerializeField] private Image mFilledStarImage;
    
    [CanBeNull] public TextMeshProUGUI mNewStarAddBlueReward;

    private double mCurrentScore = 0;
    private double mTargetScore;
    
    public double GetCurrentScore() => mCurrentScore;
    public double GetTargetScore() => mTargetScore;
    public float GetScoreRatio() => (float)(mCurrentScore / mTargetScore);

    private void Start()
    {
        int stage = StageLoader.CurrentStageNumber;
        int starFlag = 0;

        if (!Base_Manager.Data.UserData.BonusStageStars.TryGetValue(stage, out starFlag)) return;
        int starCount = 0;
        for (int i = 0; i < 3; i++)
        {
            if ((starFlag & (1 << i)) != 0)
                starCount++;
        }
        SetClearedStarCount(starCount);



        // for (int i = 0; i < mStarImages.Length; i++)
        // {
        //     mStarImages[i].sprite = mEmptyStarImage.sprite;
        //     mStarImages[i].color = new Color(0.7f, 0.7f, 0.7f);
        //     if (mGameClearStarImages != null)
        //     {
        //         mGameClearStarImages[i].sprite = mEmptyStarImage.sprite;
        //         mGameClearStarImages[i].color = new Color(1, 1, 1);
        //     }
        // }
    }

    public void SetTargetScore(double target)
    {
        mTargetScore = target;
        UpdateUI();
    }

    public void SetCurrentScore(double score)
    {
        mCurrentScore = score;
        UpdateUI();
    }

    private void UpdateUI()
    {
        float ratio = (float)(mCurrentScore / mTargetScore);
        ratio = Mathf.Clamp01(ratio);

        mFillTopImage.fillAmount = ratio;
        mFillBottomImage.fillAmount = ratio;
        
        switch (ratio)
        {
            case >= 1.0f:
                mStarImages[2].sprite = mFilledStarImage.sprite;
                if (mGameClearStarImages != null)
                {
                    mGameClearStarImages[2].sprite = mFilledStarImage.sprite;
                }
                mStarImages[2].color = new Color(1f, 1f, 1f);
                break;
            case >= 0.66f:
                mStarImages[1].sprite = mFilledStarImage.sprite;
                if (mGameClearStarImages != null)
                {
                    mGameClearStarImages[1].sprite = mFilledStarImage.sprite;
                }
                mStarImages[1].color = new Color(1f, 1f, 1f);
                break;
            case >= 0.33f:
                mStarImages[0].sprite = mFilledStarImage.sprite;
                if (mGameClearStarImages != null)
                {
                    mGameClearStarImages[0].sprite = mFilledStarImage.sprite;
                }
                mStarImages[0].color = new Color(1f, 1f, 1f);
                break;
        }
        
        //Debug.Log($"���� ���� : {mCurrentScore}, Ÿ�� ����  : {mTargetScore}");

        // if (scoreText != null)
        // {
        //     scoreText.text = $"{(int)mCurrentScore} / {(int)mTargetScore}";
        // }
    }
    
    /// <summary>
    /// �ܺο��� Ŭ������ �� ���� ������ �޾Ƽ� ���� ä���ִ� �޼���
    /// </summary>
    private void SetClearedStarCount(int starCount)
    {
        for (int i = 0; i < mStarImages.Length; i++)
        {
            bool isFilled = i < starCount;
            mStarImages[i].sprite = isFilled ? mFilledStarImage.sprite : mEmptyStarImage.sprite;
            mStarImages[i].color = isFilled ? Color.white : new Color(0.7f, 0.7f, 0.7f);

            if (mGameClearStarImages != null)
            {
                mGameClearStarImages[i].sprite = isFilled ? mFilledStarImage.sprite : mEmptyStarImage.sprite;
                mGameClearStarImages[i].color = Color.white;
            }
        }
    }
}