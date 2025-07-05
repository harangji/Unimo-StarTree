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
#if UNITY_ANDROID
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject(0) && Canvas_Holder.UI_Holder.Count > 0)
        {
            Canvas_Holder.CloseAllPopupUI();
        }
#elif UNITY_EDITOR
#endif
    }
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
