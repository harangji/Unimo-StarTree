using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas_Holder : MonoBehaviour
{
    public static Canvas_Holder instance = null;

    public bool NoneClose = false;

    public static Stack<UI_Base> UI_Holder = new Stack<UI_Base>();
    public List<LevelCheck> levelCheckObjs = new List<LevelCheck>();
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        Base_Manager.ADS.Init();
    }
    private void Start()
    {
        if (Base_Manager.instance.TimerCheck() >= 30)
        {
            Main_UI.instance.holderQueue.Enqueue("##Offline_Reward");
            Main_UI.instance.holderQueue_Action.Enqueue(null);
        }
    }
    private void Update()
    {
        if(UI_Holder.Count == 0)
        {
            for (int i = 0; i < bars.Length; i++)
            {
                bars[i].SetActive(false);
            }
            bars[2].SetActive(true);
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (UI_Holder.Count > 0)
            {
                ClosePopupUI();
            }
            else GetUI("##EXIT");
        }
    }

    public void GetLevelCheck()
    {
        for(int i = 0; i < levelCheckObjs.Count; i++)
        {
            if (levelCheckObjs[i] != null)
                levelCheckObjs[i].InitCheck();
        }
    }

    public Transform LAYER_HOLDER;
    public Transform UI_HOLDER;
    public GameObject MAIN, BOTTOM;

    public void GetLock(bool isLock, bool isClose =true)
    {
        if (isClose)
        {
            CloseAllPopupUI();
        }
        MAIN.SetActive(isLock);
        BOTTOM.SetActive(isLock);
    }

    public void Get_Toast(string temp)
    {
        var go = Instantiate(Resources.Load<PopUP_Toast>("UI/PopUP_Toast"), transform);
        go.GetPopUp(temp);
    }
    public void AllReturn()
    {
        CloseAllPopupUI();
        for(int i = 0; i < bars.Length; i++)
        {
            bars[i].SetActive(false);
        }
        bars[2].SetActive(true);
        Camera_Event.instance.ReturnCamera();
    }
    public GameObject[] bars;
    public void GetUI(string temp)
    {
        bool PeekCheck = false;
        switch (temp)
        {
            case "##Shop":
                if (bars[0].activeSelf)
                {
                    CloseAllPopupUI();
                    return;
                }
                for (int i = 0; i < bars.Length; i++)
                {
                    bars[i].SetActive(false);
                }
                bars[0].SetActive(true);
                PeekCheck = false;
                break;
            case "##Character":
                if (bars[1].activeSelf)
                {
                    CloseAllPopupUI();
                    return;
                }
                for (int i = 0; i < bars.Length; i++)
                {
                    bars[i].SetActive(false);
                }
                bars[1].SetActive(true);
                PeekCheck = false;

                break;
            case "##Mission":
                if (bars[3].activeSelf)
                {
                    CloseAllPopupUI();
                    return;
                }
                for (int i = 0; i < bars.Length; i++)
                {
                    bars[i].SetActive(false);
                }
                bars[3].SetActive(true);
                PeekCheck = true;

                break;
            case "##Game":
                if (bars[4].activeSelf)
                {
                    CloseAllPopupUI();
                    return;
                }
                for (int i = 0; i < bars.Length; i++)
                {
                    bars[i].SetActive(false);
                }
                bars[4].SetActive(true);
                PeekCheck = true;

                break;
            case "##BonusReward":
                if (Base_Manager.Data.UserData.BonusRewardCount < 900.0f)
                {
                    return;
                }
                break;
            case "##Setting":
#if UNITY_ANDROID
#elif UNITY_IOS
                temp = "##Setting_iOS";
#endif
                PeekCheck = true;
                break;
        }
        if (NoneClose == false)
        {
            CloseAllPopupUI();
        }

        NoneClose = false;
        Camera_Event.instance.MoverChange(false);
        UI_Base go = null;
        go = Instantiate(Resources.Load<UI_Base>("UI/" + temp), UI_HOLDER);
        go.transform.localScale = Vector3.one;
        RectTransform rect = go.GetComponent<RectTransform>();
        rect.offsetMin = new Vector3(0, 0);
        rect.offsetMax = new Vector3(0, 0);
 
        UI_Holder.Push(go);
        
        if(PeekCheck)
            UI_Holder.Peek().transform.parent = transform;
    }

    public void ClosePopupUI(UI_Base popup)
    {
        if (UI_Holder.Count == 0)
            return;

        if (UI_Holder.Peek() != popup)
        {
            Debug.Log("Close Popup Failed!");
            return;
        }

        ClosePopupUI();
    }
    public static void CloseAllPopupUI()
    {
        while (UI_Holder.Count > 0)
            ClosePopupUI();
    }
    public static void ClosePopupUI()
    {
        if (UI_Holder.Count == 0)
            return;
        
        UI_Base popup = UI_Holder.Peek();
        popup.DisableOBJ();
    }
}
