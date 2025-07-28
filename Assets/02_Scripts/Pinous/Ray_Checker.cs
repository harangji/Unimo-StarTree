using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.TextCore.Text;

public class Ray_Checker : MonoBehaviour
{
    Camera camera;
    bool isCharacter = false;
    private void Start()
    {
        camera = transform.GetChild(0).GetComponent<Camera>();
    }
    bool isClick =false;
    float timer = 1.0f;
    Ray_Object obj;
    GameObject unimoObj;
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject(0) && Canvas_Holder.UI_Holder.Count == 0)
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity))
            {
                if (hit.transform.gameObject.GetComponent<Ray_Object>() != null)
                {
                    obj = hit.transform.gameObject.GetComponent<Ray_Object>();
                }
                else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Unimo"))
                {
                    unimoObj = hit.transform.gameObject;
                }
            }
            isClick = true;
            timer = 1.0f;
        }
        if(Input.GetMouseButton(0))
        {
            timer -= Time.deltaTime;
            if (timer <= 0.0f) isClick = false;
        }
        if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject(0) && Canvas_Holder.UI_Holder.Count == 0)
        {
            Debug.Log(isClick);
            if(isClick == false)
            {
                return;
            }
            isClick = false;
            if (obj == null && unimoObj == null) return;
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity))
            {
                Debug.Log(hit.transform.gameObject.name);

                if (hit.transform.gameObject.GetComponent<Ray_Object>() != null)
                {
                    hit.transform.GetComponent<Ray_Object>().GetRayCheck();
                    Camera_Event.CharacterTransform = null;
                    Camera_Event.GetCharacter = false;
                }
                else if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Unimo"))
                {
                    Camera_Event.instance.events[5].Pos = hit.transform;
                    Camera_Event.instance.GetCameraEvent(CameraMoveState.Character);
                    Camera_Event.CharacterTransform = hit.transform;
                    Camera_Event.GetCharacter = true;
                    Canvas_Holder.instance.GetLock(false);
                    Canvas_Holder.instance.GetUI("##Unimo");
                    Canvas_Holder.UI_Holder.Peek().GetComponent<UI_Unimo>().Key = hit.transform.gameObject.GetComponent<AI_Move>().Name;

                    // Base_Manager.Data.UserData.Touch++; // 유니모 터치 시 예전의 퀘스트 터치 증가 제거
                }
            }
            obj = null;
            unimoObj = null;
        }
    }
}
