using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 추후 추가할 UI 예시
public class StageSelectUI : MonoBehaviour
{
    // 가운데에 적힐 Stage 번호 텍스트
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
        Debug.Log($"{Base_Manager.Data.UserData.BestStage} 입니다.");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int NextStage = mNextStage + 1;
            StageLoader.SaveClearedStage(NextStage);
            mNextStage = NextStage;
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        mNextStage = Mathf.Min(mNextStage, MaxStage);
        mStageNumberText.text = $"{mNextStage} Stage";
    }

    // 시작버튼 눌렀을 때, 스테이지 진입
    public void OnClick_StartGame()
    {
        StageLoader.LoadStage(mNextStage);
    }
}