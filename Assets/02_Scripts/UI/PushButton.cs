using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class PushButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [SerializeField] private UnityEvent pushAction;

    private readonly float initWaitTime = 0.7f;
    private readonly float serialPushTime = 0.07f;
    private float currentTime = 0.7f;
    private bool isPush = false;

    
    private void Update()
    {
        if (isPush)
        {
            currentTime -= Time.deltaTime;
            if (currentTime < 0f)
            {
                currentTime += serialPushTime;
                if (pushAction != null) { pushAction.Invoke(); }
            }
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (pushAction != null) { pushAction.Invoke(); }
        isPush = true;
        currentTime = initWaitTime;
        transform.localScale = 1.07f * Vector3.one;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        isPush = false;
        currentTime = initWaitTime;
        transform.localScale = Vector3.one;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        isPush = false;
        currentTime = initWaitTime;
        transform.localScale = Vector3.one;
    }
}
