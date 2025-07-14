using UnityEngine;
using UnityEngine.SceneManagement;

// StageSelectUI 에서 스테이지 진입 버튼을 눌렀을 때 필요한 함수 정의
public static class StageLoader
{
    // PlayerPrefs 키 이름 (고정) 임시적으로 Prefs로 구조 잡아둠.
    private const string LastClearedStageKey = "LastClearedStage";
    public static int CurrentStageNumber { get; private set; }

    public static void LoadStage(int stageNumber)
    {
        CurrentStageNumber = stageNumber;

        int unitDigit = stageNumber % 10;
        
        // 보너스 스테이지인지 체크
        bool isBonus = unitDigit == 5;

        // ST002: 보너스 스테이지, ST001: 나머지 (일반/보스)
        string sceneName = isBonus ? "PlayScene_ST002_Jinsu" : "PlayScene_ST001_Jinsu";

        Debug.Log(sceneName);
        
        SceneManager.LoadScene(sceneName);
    }
    
    // 저장 함수
    public static void SaveClearedStage(int clearedStage)
    {
        int previous = PlayerPrefs.GetInt(LastClearedStageKey, 0);
        if (clearedStage > previous)
            PlayerPrefs.SetInt(LastClearedStageKey, clearedStage);
    }

    // 불러오기
    public static int GetLastClearedStage()
    {
        return PlayerPrefs.GetInt(LastClearedStageKey, 200); // 기본값 0 (아무 스테이지도 클리어 안함)
    }
    
    public static void ResetProgress()
    {
        PlayerPrefs.DeleteKey(LastClearedStageKey);
    }
}