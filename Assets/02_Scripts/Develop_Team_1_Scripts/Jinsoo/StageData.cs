using UnityEngine;

public class StageData
{
    public int StageNumber; // 스테이지 번호
    public bool IsBonusStage; // 보너스 스테이지 여부
    public bool IsBossStage; // 보스 스테이지 여부
    
    public int TargetScore; // 목표 점수
    public int DifficultyValue; // 난이도 수치 (몬스터 생성 난이도)
    public int MaxFlowerCount; // 꽃의 최대 수
    public float TimeLimit; // 제한 시간
    public int BonusStageNumber;

    public StageData(int stageNum)
    {
        StageNumber = stageNum;
        IsBonusStage = stageNum % 10 == 5;
        IsBossStage = stageNum % 10 == 0;
        BonusStageNumber = 0;

        for (int i = 1; i < stageNum; i++)
        {
            if (i % 10 == 5) BonusStageNumber++;
        }

        if (IsBonusStage)
        {
            TimeLimit = 60;
            TargetScore = 0;
            DifficultyValue = 0;
            MaxFlowerCount = 20;
        }
        else
        {
            TimeLimit = 120;
            TargetScore = 100 + (stageNum - 1 - BonusStageNumber) * 3;

            double difficulty = 15 * Mathf.Pow(1.0164f, stageNum - 1 - BonusStageNumber);
            DifficultyValue = Mathf.Min(60, Mathf.RoundToInt((float)difficulty));
            if (IsBossStage) DifficultyValue += 5;

            MaxFlowerCount = 10 + (stageNum - 1) / 50;
        }
    }
}
