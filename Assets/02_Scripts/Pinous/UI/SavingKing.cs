using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SavingKing : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public RectTransform KingTransform;
    private Vector2 _startingPoint;
    private Vector2 _moveBegin;
    private Vector2 _moveOffset;
    public Animator King;
    public Sprite[] sprites;
    public Image image;

    public RectTransform[] rects;
    bool GetMove = false;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!GetMove)
        {
            OnDemandRendering.renderFrameInterval = 1;

            _startingPoint = KingTransform.anchoredPosition;
            _moveBegin = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!GetMove)
        {
            _moveOffset = (eventData.position - _moveBegin) * 1.5f;
            KingTransform.anchoredPosition = new Vector2(_startingPoint.x + _moveOffset.x, KingTransform.anchoredPosition.y);

            for(int i = 0; i < rects.Length; i++)
            {
                if (KingTransform.anchoredPosition.x >= rects[i].anchoredPosition.x)
                    rects[i].gameObject.SetActive(false);
                else rects[i].gameObject.SetActive(true);
            }

            if (KingTransform.anchoredPosition.x <= -440.0f) image.sprite = sprites[0];
            else if (KingTransform.anchoredPosition.x > -440.0f&& KingTransform.anchoredPosition.x <= 350.0f) image.sprite = sprites[1];
            else if (KingTransform.anchoredPosition.x > 350.0f) image.sprite = sprites[2];

            if (KingTransform.anchoredPosition.x >= 550.0f)
            {
                KingTransform.anchoredPosition = new Vector2(550.0f, KingTransform.anchoredPosition.y);
                GetMove = true;
                Invoke("Return", 1.0f);
            }
        }
    }

    private void Return()
    {
        Sound_Manager.BGM_volume = 1.0f;
        Sound_Manager.FX_volume = 1.0f;
        UI_SavingMode.GetReturn();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!GetMove)
        {
            OnDemandRendering.renderFrameInterval = 3;

            for (int i = 0; i < rects.Length; i++)
            {
                rects[i].gameObject.SetActive(true);
            }
            image.sprite = sprites[0];
            KingTransform.anchoredPosition = new Vector2(-450f, KingTransform.anchoredPosition.y);
        }
    }
}
