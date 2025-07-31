using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Lobby ���� �������� ���� ��ư Class
/// </summary>
public class StageSelectUI : MonoBehaviour
{
    // ����� ���� Stage ��ȣ �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI mStageNumberText;
    private int mNextStage;
    private int mCurrentStage = 1;
    private int mMaxClearedStage = 0;

    private const int MaxStage = 1000;

    private void Start()
    {
        int lastCleared = StageLoader.GetLastClearedStage();
        mNextStage = lastCleared + 1;
        UpdateUI();
        // Debug.Log($"{Base_Manager.Data.UserData.BestStage} �Դϴ�.");
    }

    public void StageUpgrade()
    {
        int NextStage = mNextStage + 1;
        StageLoader.SaveClearedStage(NextStage);
        mNextStage = NextStage;
        UpdateUI();
    }

    public void StageDowngrade()
    {
        if (mNextStage <= 1) return;
        StageLoader.StageDownSave();
        mNextStage--;
        UpdateUI();
    }

    private void UpdateUI()
    {
        mNextStage = Mathf.Min(mNextStage, MaxStage);
        mStageNumberText.text = $"{mNextStage}  Play";
    }

    // ���۹�ư ������ ��, �������� ����
    public void OnClick_StartGame()
    {
        StageLoader.LoadStage(mNextStage);
    }
}