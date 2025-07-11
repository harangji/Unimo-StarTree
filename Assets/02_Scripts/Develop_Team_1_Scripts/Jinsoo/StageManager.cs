using UnityEngine;

// �ΰ��� Stage ���̵� ����.
public class StageManager : MonoBehaviour
{
    private StageData mStageData;
    private bool mbStageEnded = false;

    private void Start()
    {
        // ���� ���õ� �������� ���� ��������
        mStageData = new StageData(StageLoader.CurrentStageNumber);

        Debug.Log($"[StageManager] �������� {mStageData.StageNumber} ����");
        Debug.Log($" - ���� �ð�: {mStageData.TimeLimit}s");
        Debug.Log($" - ���̵� ��ġ: {mStageData.DifficultyValue}");
        Debug.Log($" - ��ǥ ���� : {mStageData.TargetScore}");
        Debug.Log($" - �ִ� �� ���� : {mStageData.MaxFlowerCount}");
        Debug.Log($" - ���� �������� ���� : {mStageData.IsBossStage}");

        // ���⼭ �ʿ��� �Ŵ����鿡 ���̵� �� ���� ����
        // ��: MonsterGenerator.SetDifficulty(mStageData.DifficultyValue);
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
        Debug.Log("�������� Ŭ����");
        
        // Ŭ����� �������� ����
        StageLoader.SaveClearedStage(mStageData.StageNumber);
        
        // UI_StageResult.Instance.Open(true);
    }

    private void StageFail()
    {
        Debug.Log("�������� ����");
        // UI_StageResult.Instance.Open(false);
    }
}