using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ScoreGaugeController : MonoBehaviour
{
    [FormerlySerializedAs("fillImage")] [SerializeField] private Image mfillImage;   // 게이지 이미지 (Filled 방식)
    // [SerializeField] private Text scoreText;    // 점수 표시 (선택사항)
    [SerializeField] private Image[] mStarImages;
    [SerializeField] private Image mEmptyStarImage;
    [SerializeField] private Image mFilledStarImage;

    private double mCurrentScore = 0;
    private double mTargetScore;

    private void Start()
    {
        for (int i = 0; i < mStarImages.Length; i++)
        {
            mStarImages[i].sprite = mEmptyStarImage.sprite;
        }
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

        mfillImage.fillAmount = ratio;
        
        switch (ratio)
        {
            case >= 1.0f:
                mStarImages[2].sprite = mFilledStarImage.sprite;
                break;
            case >= 0.66f:
                mStarImages[1].sprite = mFilledStarImage.sprite;
                break;
            case >= 0.33f:
                mStarImages[0].sprite = mFilledStarImage.sprite;
                break;
        }

        // if (scoreText != null)
        // {
        //     scoreText.text = $"{(int)mCurrentScore} / {(int)mTargetScore}";
        // }
    }
}