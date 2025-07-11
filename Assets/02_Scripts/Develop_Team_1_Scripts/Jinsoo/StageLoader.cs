using UnityEngine;
using UnityEngine.SceneManagement;

// StageSelectUI ���� �������� ���� ��ư�� ������ �� �ʿ��� �Լ� ����
public static class StageLoader
{
    // PlayerPrefs Ű �̸� (����) �ӽ������� Prefs�� ���� ��Ƶ�.
    private const string LastClearedStageKey = "LastClearedStage";
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
    
    // ���� �Լ�
    public static void SaveClearedStage(int clearedStage)
    {
        int previous = PlayerPrefs.GetInt(LastClearedStageKey, 0);
        if (clearedStage > previous)
            PlayerPrefs.SetInt(LastClearedStageKey, clearedStage);
    }

    // �ҷ�����
    public static int GetLastClearedStage()
    {
        return PlayerPrefs.GetInt(LastClearedStageKey, 200); // �⺻�� 0 (�ƹ� ���������� Ŭ���� ����)
    }
    
    public static void ResetProgress()
    {
        PlayerPrefs.DeleteKey(LastClearedStageKey);
    }
}