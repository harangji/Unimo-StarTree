using UnityEngine;
using UnityEngine.UI;

public class ScoreGaugeController : MonoBehaviour
{
    [SerializeField] private Image fillImage;   // 게이지 이미지 (Filled 방식)
    // [SerializeField] private Text scoreText;    // 점수 표시 (선택사항)

    private double mCurrentScore = 0;
    private double mTargetScore;

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

        fillImage.fillAmount = ratio;

        // if (scoreText != null)
        // {
        //     scoreText.text = $"{(int)mCurrentScore} / {(int)mTargetScore}";
        // }
    }
}