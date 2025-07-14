using UnityEngine;

public class StageData
{
    public int StageNumber; // �������� ��ȣ
    public bool IsBonusStage; // ���ʽ� �������� ����
    public bool IsBossStage; // ���� �������� ����
    
    public int TargetScore; // ��ǥ ����
    public int DifficultyValue; // ���̵� ��ġ (���� ���� ���̵�)
    public int MaxFlowerCount; // ���� �ִ� ��
    public float TimeLimit; // ���� �ð�
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
