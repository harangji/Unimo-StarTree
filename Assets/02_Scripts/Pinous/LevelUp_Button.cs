using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
public class LevelUp_Button : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    bool isPush = false;
    float timer = 0.0f;
    Coroutine coroutine;
    public void EXP_UP_Button()
    {
        InitLevelUpBUtton();
        transform.DORewind();
        transform.DOPunchScale(new Vector3(-0.2f, -0.2f, -0.2f), .25f);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        EXP_UP_Button();
        coroutine = StartCoroutine(Push_Coroutine());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPush = false;
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        timer = 0.0f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPush = false;
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        timer = 0.0f;
    }

    public void OnDisable()
    {
        isPush = false;
        if (coroutine != null) StopCoroutine(coroutine);
        timer = 0.0f;
    }

    void Update()
    {
        if (Input.touchCount == 0)
        {
            isPush = false;
            if (coroutine != null) StopCoroutine(coroutine);
            timer = 0.0f;
        }

        if (isPush)
        {
            Base_Mng.instance.saveTimer = 0.0f;
            timer += Time.deltaTime;
            if (timer >= 0.01f)
            {
                timer = 0.0f;
                EXP_UP_Button();
            }
        }
    }

    IEnumerator Push_Coroutine()
    {
        yield return new WaitForSeconds(1.0f);
        isPush = true;
    }

    private void InitLevelUpBUtton()
    {
        if (Base_Mng.Data.data.Yellow < Base_Mng.Data.data.NextLevel_Base)
        {
            Canvas_Holder.instance.Get_Toast("NM");
            return;
        }
        Base_Mng.Data.data.Yellow -= Base_Mng.Data.data.NextLevel_Base;
        Base_Mng.Data.LevelUP();
        Sound_Manager.instance.Play(Sound.Effect, "effect_00");
    }
}
