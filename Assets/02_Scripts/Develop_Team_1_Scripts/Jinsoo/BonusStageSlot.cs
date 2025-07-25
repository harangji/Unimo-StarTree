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
            // �� 3�� �� ������ �Ұ�
            playButton.interactable = false;

            // ��ư ���� ���� (ȸ�� ó��)
            var colors = playButton.colors;
            colors.normalColor = new Color(0.6f, 0.6f, 0.6f); // ���� ȸ��
            playButton.colors = colors;
        }
        else
        {
            // �絵�� ����
            playButton.interactable = true;
            var colors = playButton.colors;
            colors.normalColor = Color.white;
            playButton.colors = colors;

            playButton.onClick.AddListener(() =>
            {
                StageLoader.LoadStage(stageNum);
            });
        }
    }
}
