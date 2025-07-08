using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ���� ���� ���� 
// Ÿ�̸� �ʱⰪ, �Ͻ�����/�簳, ���� ���� ó��, ��� ȭ�� Ȱ��ȭ ���� ����
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
    public void TimeUp()
    {
        if (gameoverAction != null) { gameoverAction.Invoke(); }
        gameoverText.SetActive(true);
        StartCoroutine(CoroutineExtensions.DelayedActionCall(
            () => { gameResultObjs[0].SetActive(true);
                gameResultObjs[1].SetActive(true); }, 5f / 3f + 0.5f));
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
