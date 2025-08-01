using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class DamageUI : MonoBehaviour
{
    private RectTransform targetUI;
    
    private void Awake()
    {
        targetUI = GetComponent<RectTransform>();
    }

    void OnEnable()
    {
        StartCoroutine(Destroy());
        
        Vector3 targetPos = targetUI.anchoredPosition + new Vector2(0f, 50f);
        targetUI.DOAnchorPos(targetPos, 2f).SetEase(Ease.OutCubic);
    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(2f);
        
        DamageUIManager.instance.ReturnUI(gameObject);
    }
}
