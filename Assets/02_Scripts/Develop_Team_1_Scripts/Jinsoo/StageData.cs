using UnityEngine;

public class StageData
{
    public int StageNumber;
    public bool IsBonusStage;
    public bool IsBossStage;
    
    public int TargetScore;
    public int DifficultyValue;
    public int MaxFlowerCount;
    public float TimeLimit = 120f;

    public StageData(int stageNum)
    {
        StageNumber = stageNum;
        IsBonusStage = stageNum % 10 == 5;
        IsBossStage = stageNum % 10 == 0;

        if (IsBonusStage)
        {
            TargetScore = 0;
            DifficultyValue = 0;
            MaxFlowerCount = 20;
        }
        else
        {
            TargetScore = 100 + (stageNum - 1) * 5;

            double difficulty = 15 * Mathf.Pow(1.0164f, stageNum - 1);
            DifficultyValue = Mathf.Min(60, Mathf.RoundToInt((float)difficulty));
            if (IsBossStage) DifficultyValue += 5;

            MaxFlowerCount = 10 + (stageNum - 1) / 50;
        }
    }
}
