using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 게임 진행 상태 관리 
// 타이머 초기값, 일시정지/재개, 게임 오버 처리, 결과 화면 활성화 등을 제어
public class PlayProcessController : MonoBehaviour
{
    public static float InitTimeSTATIC = 1.2f;
    [SerializeField] private float initTime = 1.2f;
    [SerializeField] private GameObject gameoverText;
    [SerializeField] private List<GameObject> gameResultObjs;
    private Action gameoverAction;
    private Action pauseAction;
    private Action resumeAction;

    private void Awake()
    {
        InitTimeSTATIC = initTime;
        PlaySystemRefStorage.playProcessController = this;
    }
    public void SubscribeGameoverAction(Action action)
    {
        gameoverAction += action;
    }
    public void SubscribePauseAction(Action action)
    {
        pauseAction += action;
    }
    public void SubscribeResumeAction(Action action)
    {
        resumeAction += action;
    }

    public void GameClear()
    {
        
        Debug.Log("GameClear ::: gameoverAction 호출 전");
        if (gameoverAction != null) { gameoverAction.Invoke(); }
        Debug.Log("GameClear ::: gameoverAction 호출 후");
        gameoverText.SetActive(true);
        StartCoroutine(CoroutineExtensions.DelayedActionCall(
            () => { gameResultObjs[0].SetActive(true);
                gameResultObjs[2].SetActive(true); }, 5f / 3f + 0.5f));
    }
    public void GameOver()
    {
        if (gameoverAction != null) { gameoverAction.Invoke(); }
        gameoverText.SetActive(true);
        StartCoroutine(CoroutineExtensions.DelayedActionCall(
            () => { gameResultObjs[1].SetActive(true);
                gameResultObjs[2].SetActive(true); }, 5f / 3f + 0.5f));
    }
    public void GamePaused()
    {
        if (pauseAction != null) { pauseAction.Invoke(); }
    }
    public void GameResumed()
    {
        if (resumeAction != null) { resumeAction.Invoke(); }
    }

}
