using UnityEngine;
using UnityEngine.SceneManagement;

// StageSelectUI ���� �������� ���� ��ư�� ������ �� �ʿ��� �Լ� ����
public static class StageLoader
{
    public static int CurrentStageNumber { get; private set; }

    public static void LoadStage(int stageNumber)
    {
        CurrentStageNumber = stageNumber;

        int unitDigit = stageNumber % 10;
        
        // ���ʽ� ������������ üũ
        bool isBonus = unitDigit == 5;

        // 2: ���ʽ� ��������, 1: ������ (�Ϲ�/����)
        if (isBonus)
        {
            WholeSceneController.Instance.ReadyNextScene(2);
        }
        else
        {
            WholeSceneController.Instance.ReadyNextScene(1);
        }
        
        Base_Manager.Data.UserData.GamePlay++;
        Pinous_Flower_Holder.FlowerHolder.Clear();
    }
    
    // EasySaveManager ��� ���� �Լ�
    public static void SaveClearedStage(int clearedStage)
    {
        Debug.Log($"���� �ְ� �������� : {Base_Manager.Data.UserData.BestStage}");
        var userData = Base_Manager.Data.UserData;
        if (clearedStage > userData.BestStage && userData.BestStage < 1000)
        {
            userData.BestStage = clearedStage;
            Base_Manager.Data.SaveUserData(); // �ְ��������� �ٲ������ ���� �ѹ� ����.
        }
        Debug.Log($"�ٲ� �ְ� �������� : {Base_Manager.Data.UserData.BestStage}");
    }

    // EasySaveManager ��� �ҷ����� �Լ�
    public static int GetLastClearedStage()
    {
        return Base_Manager.Data.UserData.BestStage;
    }

    public static void ResetProgress()
    {
        if (EasySaveManager.Instance != null)
            // EasySaveManager.Instance.Delete("UserData"); // ��ü ������ ����
        {
            var userData = Base_Manager.Data.UserData;
            userData.BestStage = 0; // �ְ� �������� 0���� �ӽ� �ʱ�ȭ
        }
    }
}