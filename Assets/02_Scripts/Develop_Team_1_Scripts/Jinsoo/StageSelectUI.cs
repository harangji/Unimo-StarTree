using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 추후 추가할 UI 예시
public class StageSelectUI : MonoBehaviour
{
    // 가운데에 적힐 Stage 번호 텍스트
    [SerializeField] private TextMeshProUGUI mStageNumberText;
    // [SerializeField] private Text mLockText;
    
    private int mCurrentStage = 1;
    private int mMaxClearedStage = 0;

    private const int MaxStage = 1000;

    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            mMaxClearedStage++;
            Debug.Log($"마지막 스테이지 증가 ::: {mMaxClearedStage}");
        }
    }

    // 왼쪽 화살표 눌렀을 때, 스테이지 감소
    public void OnClick_LeftArrow()
    {
        mCurrentStage = Mathf.Max(1, mCurrentStage - 1);
        UpdateUI();
    }

    // 오른쪽 화살표 눌렀을 때, 스테이지 증가
    public void OnClick_RightArrow()
    {
        if (mCurrentStage < Mathf.Min(MaxStage, mMaxClearedStage + 1))
        {
            mCurrentStage++;
            UpdateUI();
        }
        else
        {
            // 잠긴 스테이지 안내 메시지
            // if (mLockText != null)
            //     mLockText.text = "이전 스테이지를 먼저 클리어해야 합니다";
            Debug.Log("잠겨 있어요");
        }
    }

    private void UpdateUI()
    {
        mStageNumberText.text = $"Stage {mCurrentStage}";
        
        // if (mLockText != null)
        //     mLockText.text = ""; // 메시지 초기화
    }

    // 시작버튼 눌렀을 때, 스테이지 진입
    public void OnClick_StartGame()
    {
        mMaxClearedStage = StageLoader.GetLastClearedStage();
        // 잠긴 스테이지는 시작 불가
        if (mCurrentStage > mMaxClearedStage + 1)
        {
            // if (mLockText != null)
            //     mLockText.text = "이 스테이지는 잠겨 있습니다";
            Debug.Log("잠겨 있어요");
            return;
        }
        
        StageLoader.LoadStage(mCurrentStage);
    }
}