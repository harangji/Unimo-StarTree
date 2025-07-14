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

        // ST002: ���ʽ� ��������, ST001: ������ (�Ϲ�/����)
        string sceneName = isBonus ? "PlayScene_ST002_Jinsu" : "PlayScene_ST001_Jinsu";

        Debug.Log(sceneName);
        
        SceneManager.LoadScene(sceneName);
    }
    
    // EasySaveManager ��� ���� �Լ�
    public static void SaveClearedStage(int clearedStage)
    {
        var userData = Base_Manager.Data.UserData;
        if (clearedStage > userData.BestStage)
        {
            userData.BestStage = clearedStage;
            Base_Manager.Data.SaveUserData(); // �ְ��������� �ٲ������ ���� �ѹ� ����.
        }
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