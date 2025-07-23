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

        for (int i = 0; i < starImages.Length; i++)
        {
            int bit = 1 << i;
            bool isFilled = (starFlag & bit) != 0;
            starImages[i].sprite = isFilled ? filledStar : emptyStar;
        }

        playButton.onClick.RemoveAllListeners();
        playButton.onClick.AddListener(() =>
        {
            // 해당 스테이지로 진입 넣기
            StageLoader.LoadStage(stageNum);
        });
    }
}
