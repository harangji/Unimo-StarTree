using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BonusStageSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text stageText;
    [SerializeField] private Button playButton;
    [SerializeField] private Image[] starImages;
    [SerializeField] private Sprite emptyStar, filledStar;

    private int stageNum;

    public void SetSlot(int stage, int starFlag)
    {
        stageNum = stage;
        stageText.text = $"{stage} Play";

        var starCount = 0;
        for (int i = 0; i < starImages.Length; i++)
        {
            int bit = 1 << i;
            bool isFilled = (starFlag & bit) != 0;
            starImages[i].sprite = isFilled ? filledStar : emptyStar;
            if (isFilled) starCount++;
        }

        playButton.onClick.RemoveAllListeners();
        if (starCount >= 3)
        {
            // 별 3개 → 재진입 불가
            playButton.interactable = false;

            // 버튼 색상 변경 (회색 처리)
            var colors = playButton.colors;
            colors.normalColor = new Color(0.88f, 0.86f, 0.86f); // 회색
            playButton.colors = colors;

            stageText.color = new Color(0.2f, 0.2f, 0.2f, 0.4f);
        }
        else
        {
            // 재도전 가능
            playButton.interactable = true;
            var colors = playButton.colors;
            colors.normalColor = new Color(0.9f, 0.9f, 0.9f);
            playButton.colors = colors;

            playButton.onClick.AddListener(() =>
            {
                StageLoader.LoadStage(stageNum);
            });
        }
    }
}
