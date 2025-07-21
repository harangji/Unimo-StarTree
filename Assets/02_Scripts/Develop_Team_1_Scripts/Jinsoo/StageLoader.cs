using UnityEngine;
using UnityEngine.SceneManagement;

// StageSelectUI 에서 스테이지 진입 버튼을 눌렀을 때 필요한 함수 정의
public static class StageLoader
{
    public static int CurrentStageNumber { get; private set; }

    public static void LoadStage(int stageNumber)
    {
        CurrentStageNumber = stageNumber;

        int unitDigit = stageNumber % 10;
        
        // 보너스 스테이지인지 체크
        bool isBonus = unitDigit == 5;

        // 2: 보너스 스테이지, 1: 나머지 (일반/보스)
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
    
    // EasySaveManager 기반 저장 함수
    public static void SaveClearedStage(int clearedStage)
    {
        Debug.Log($"이전 최고 스테이지 : {Base_Manager.Data.UserData.BestStage}");
        var userData = Base_Manager.Data.UserData;
        if (clearedStage > userData.BestStage && userData.BestStage < 1000)
        {
            userData.BestStage = clearedStage;
            Base_Manager.Data.SaveUserData(); // 최고스테이지가 바뀌었으니 저장 한번 실행.
        }
        Debug.Log($"바뀐 최고 스테이지 : {Base_Manager.Data.UserData.BestStage}");
    }

    // EasySaveManager 기반 불러오기 함수
    public static int GetLastClearedStage()
    {
        return Base_Manager.Data.UserData.BestStage;
    }

    public static void ResetProgress()
    {
        if (EasySaveManager.Instance != null)
            // EasySaveManager.Instance.Delete("UserData"); // 전체 데이터 삭제
        {
            var userData = Base_Manager.Data.UserData;
            userData.BestStage = 0; // 최고 스테이지 0으로 임시 초기화
        }
    }
}