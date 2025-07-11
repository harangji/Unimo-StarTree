using UnityEngine;

// 인게임 Stage 난이도 관리.
public class StageManager : MonoBehaviour
{
    private StageData mStageData;
    private bool mbStageEnded = false;

    private void Start()
    {
        // 현재 선택된 스테이지 정보 가져오기
        mStageData = new StageData(StageLoader.CurrentStageNumber);

        Debug.Log($"[StageManager] 스테이지 {mStageData.StageNumber} 시작");
        Debug.Log($" - 제한 시간: {mStageData.TimeLimit}s");
        Debug.Log($" - 난이도 수치: {mStageData.DifficultyValue}");
        Debug.Log($" - 목표 점수 : {mStageData.TargetScore}");
        Debug.Log($" - 최대 꽃 개수 : {mStageData.MaxFlowerCount}");
        Debug.Log($" - 보스 스테이지 여부 : {mStageData.IsBossStage}");

        // 여기서 필요한 매니저들에 난이도 등 전달 가능
        // 예: MonsterGenerator.SetDifficulty(mStageData.DifficultyValue);
    }

    private void Update()
    {
        if (mbStageEnded) return;

        float elapsedTime = PlaySystemRefStorage.playTimeManager.LapseTime;
        // float fillAmount = GrowthGaugeManager.Instance.FillAmount;

        if (elapsedTime >= mStageData.TimeLimit)
        {
            mbStageEnded = true;

            // if (fillAmount >= 1f)
            //     StageClear();
            // else
            //     StageFail();
        }
    }

    private void StageClear()
    {
        Debug.Log("스테이지 클리어");
        
        // 클리어된 스테이지 저장
        StageLoader.SaveClearedStage(mStageData.StageNumber);
        
        // UI_StageResult.Instance.Open(true);
    }

    private void StageFail()
    {
        Debug.Log("스테이지 실패");
        // UI_StageResult.Instance.Open(false);
    }
}