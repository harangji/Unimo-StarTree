using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
public class UI_Base : MonoBehaviour
{
    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();
    public Transform GetTransform;
    public float startTransformPos;
    protected bool _init = false;

    public Animator animator;
    public virtual bool Init()
    {
        if (_init)
            return false;
        return _init = true;
    }


    public virtual void Start()
    {
        Sound_Manager.instance.Play(Sound.Effect, "Click_01");
        Camera_Event.instance.MoverChange(false);
        Init();
    }
    Vector2 startPos;
    Vector2 endPos;
    bool GetClose = false;
    
    public virtual void Update()
    {
        //IsPointerOverGameObject 전혀 작동 안됨.
        
// #if UNITY_ANDROID
//         // 화면을 터치했는지
//         if (Input.touchCount == 0) return;
//         Touch touch = Input.GetTouch(0);
//         if (touch.phase == TouchPhase.Began)
//         {
//             // 터치했을 때, 터치 위치가 UI 위라면 ture
//             Debug.Log($" UI 위인가? {EventSystem.current.IsPointerOverGameObject(touch.fingerId)}");
//         }	
//         
//         // if(Input.touchCount == 0) return;
//         //
//         // Touch touch = Input.GetTouch(0);
//         // if (touch.phase == TouchPhase.Canceled &&
//         //     !EventSystem.current.IsPointerOverGameObject(0) &&
//         //     Canvas_Holder.UI_Holder.Count > 0)
//         // {
//         //     Canvas_Holder.CloseAllPopupUI();
//         // }
// #elif UNITY_EDITOR
// #endif
    }

//     public virtual void Update()
//     {
// #if UNITY_ANDROID
//         if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
//         {
//             if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) &&
//                 Canvas_Holder.UI_Holder.Count > 0)
//             {
//                 Canvas_Holder.CloseAllPopupUI();
//             }
//         }
// #elif UNITY_EDITOR
//     if (Input.GetMouseButtonDown(0) &&
//         !EventSystem.current.IsPointerOverGameObject() &&
//         Canvas_Holder.UI_Holder.Count > 0)
//     {
//         Canvas_Holder.CloseAllPopupUI();
//     }
// #endif
//      }


    public virtual void DisableOBJ()
    {
        DisableCheck();
        if (Main_UI.instance != null)
        {
            if (Main_UI.instance.holderQueue.Count > 0)
            {
                Canvas_Holder.instance.GetUI(Main_UI.instance.holderQueue.Dequeue());
                Main_UI.instance.holderQueue_Action.Dequeue()?.Invoke();
            }
        }
    }

    protected void DisableCheck(bool MoveChange = true)
    {
        Canvas_Holder.UI_Holder.Pop();
        Camera_Event.instance.MoverChange(MoveChange);
        Sound_Manager.instance.Play(Sound.Effect, "Click_02");
        Destroy(this.gameObject);
    }
   
}
