using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlaySceneController : MonoBehaviour
{
    public static PlaySceneController Instance;

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void ResetScene()
    {
        Sound_Manager.instance.Play(Sound.Effect, "Click_01");

        PlayerPrefs.SetInt("GetRewardCount", PlayerPrefs.GetInt("GetRewardCount") - 1);
        if(PlayerPrefs.GetInt("GetRewardCount") <= 0)
        {
            PlayerPrefs.SetInt("GetRewardCount", 2);
            Base_Manager.ADS._interstitialCallback = () =>
            {
                WholeSceneController.Instance.ReadyNextScene(SceneManager.GetActiveScene().buildIndex - WholeSceneController.ActualSceneIdxOffset);
                Time.timeScale = 1f;

            };
            Base_Manager.ADS.ShowInterstitialAds();
        }
        else
        {
            WholeSceneController.Instance.ReadyNextScene(SceneManager.GetActiveScene().buildIndex - WholeSceneController.ActualSceneIdxOffset);
            Time.timeScale = 1f;
        }

    }
    public void PauseGame()
    {
        Sound_Manager.instance.Play(Sound.Effect, "Click_01");

        PlaySystemRefStorage.playProcessController.GamePaused();
        Time.timeScale = 0f;
    }
    public void ResumeGame()
    {
        Sound_Manager.instance.Play(Sound.Effect, "Click_01");

        PlaySystemRefStorage.playProcessController.GameResumed();
        Time.timeScale = 1f;
    }
    
    public void NextGame()
    {
        int mNextStage = Base_Manager.Data.UserData.BestStage + 1;
        StageLoader.LoadStage(mNextStage);
    }
    
    
    public void PauseGameSingleton() //게임 멈추기
    {
        PlaySystemRefStorage.playProcessController.GamePaused();
        Time.timeScale = 0f;
    }

    public void ResumeGameSingleton() //게임 실행
    {
        PlaySystemRefStorage.playProcessController.GameResumed();
        Time.timeScale = 1f;
    }
    
    public void LoadLobby()
    {
        PlayerPrefs.SetInt("GetRewardCount", PlayerPrefs.GetInt("GetRewardCount") - 1);
        Data_Manager.SetPlayScene = true;
        if (PlayerPrefs.GetInt("GetRewardCount") <= 0)
        {
            PlayerPrefs.SetInt("GetRewardCount", 2);
            Base_Manager.ADS._interstitialCallback = () =>
            {
                WholeSceneController.Instance.ReadyNextScene(0);
                Time.timeScale = 1f;

            };
            Base_Manager.ADS.ShowInterstitialAds();
        }
        else
        {
            WholeSceneController.Instance.ReadyNextScene(0);
            Time.timeScale = 1f;
        }
    }
}
