using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class WholeSceneController : SingletonBehaviour<WholeSceneController>
{
    static public readonly float SCENETRANSITTIME = 0.7f;
    static public readonly int ActualSceneIdxOffset = 3;
    static private readonly int initSceneIdx = 1;
    static private readonly int loadSceneIdx = 2;

    private bool isLoadingScene = false;
    public int nextSceneIdx = 3;

    private SceneChangeEffecter[] mViewCtrl;
    AsyncOperation LoadingSceneAsync;

    public TextMeshProUGUI LoadingText;
    public TextMeshProUGUI VersionText;

    public GameObject TapToStartText;
    public Slider LoadingSlider;

    new void Awake()
    {
        base.Awake();
        if(VersionText != null)
        VersionText.text = "Version " + Application.version;
        if (IsNotWantedObj) { return; }
        if (PlayerPrefs.GetInt("Tutorial") == 1)
            nextSceneIdx = 3;
        else nextSceneIdx = 6;
    }
    private void Update()
    {
        if (isLoadingScene)
        {
            if (SceneManager.GetActiveScene().buildIndex == initSceneIdx)
            {
                if (nextSceneIdx == 3 || nextSceneIdx == 6)
                {
                    if (Base_Mng.Firebase.isSetFirebase)
                    {
                        isLoadingScene = false;
                        StartCoroutine(callNextSceneLoading());
                    }
                }
                else
                {
                    isLoadingScene = false;
                    CallNextScene();
                }
            }
            else if (SceneManager.GetActiveScene().buildIndex == loadSceneIdx)
            {
                isLoadingScene = false;
                CallNextScene();
            }
        }
    }
    private void OnEnable()
    {
        if (IsNotWantedObj) { return; }
        SceneManager.sceneLoaded += ResetSceneChangeCtrlList;
    }
    private void OnDisable()
    {
        if (IsNotWantedObj) { return; }
        SceneManager.sceneLoaded -= ResetSceneChangeCtrlList;
    }
    private void ResetSceneChangeCtrlList(Scene scene, LoadSceneMode mode)
    {
        //Initialize mViewCtrl and mInputBlocker
        mViewCtrl = FindObjectsByType<SceneChangeEffecter>(FindObjectsSortMode.None);
        if (SceneManager.GetActiveScene().buildIndex == initSceneIdx || SceneManager.GetActiveScene().buildIndex == loadSceneIdx)
        {
            isLoadingScene = true;
        }
    }
    public void ReadyNextScene(int next)
    {
        nextSceneIdx = next + ActualSceneIdxOffset;
        callLoadingScene();
    }
    public void CallNextScene()
    {
        callGameScene();
    }

    private void callGameScene()
    {
        StartCoroutine(callNextSceneCoroutine());
    }
    private void callLoadingScene()
    {
        StartCoroutine(callLoadingSceneCoroutine());
    }
    private IEnumerator callLoadingSceneCoroutine()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(loadSceneIdx);
        asyncLoad.allowSceneActivation = false;
        for (int i = 0; i < mViewCtrl.Length; i++)
        {
            mViewCtrl[i].SceneChangeAction(SCENETRANSITTIME);
        }
        asyncLoad.allowSceneActivation = true;

        yield break;
    }

    private IEnumerator callNextSceneLoading()
    {
        LoadingSceneAsync = SceneManager.LoadSceneAsync(nextSceneIdx);
        LoadingSceneAsync.allowSceneActivation = false;
        while (LoadingSceneAsync.progress < 0.9f) {
            LoadingSlider.value = LoadingSceneAsync.progress;
            LoadingText.text = string.Format("{0:0.00}%", (LoadingSlider.value * 100.0f));
             yield return null; 
        }
        LoadingSlider.value = 1.0f;
        TapToStartText.gameObject.SetActive(true);
        LoadingText.text = "100%";
    }

    public void GetAsyncLoadingBar()
    {
        Debug.Log("Check");
        LoadingSceneAsync.allowSceneActivation = true;
    }

    private IEnumerator callNextSceneCoroutine()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneIdx);
        asyncLoad.allowSceneActivation = false;
        while (asyncLoad.progress < 0.89f) { yield return null; }
        for (int i = 0; i < mViewCtrl.Length; i++)
        {
            mViewCtrl[i].SceneChangeAction(SCENETRANSITTIME);
        }
        asyncLoad.allowSceneActivation = true;

        yield break;
    }

    private bool isAllEffectEnd()
    {
        bool isReady = true;
        for (int i = 0; i < mViewCtrl.Length; i++)
        {
            isReady = isReady&&mViewCtrl[i].IsReady;
        }
        return isReady;
    }
}
