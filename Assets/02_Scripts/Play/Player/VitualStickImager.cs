using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VitualStickImager : MonoBehaviour
{
    [SerializeField] private RectTransform stickTransform;
    private float moveRadius;
    private void OnDisable()
    {
        stickTransform.localPosition = Vector3.zero;
    }
    public void setStickPos(Vector2 dir)
    {
        stickTransform.localPosition = moveRadius * dir;
    }
    public void setStickImgSizes(float sizew, float sizeh)
    {
        RectTransform rect = GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2 (sizew, sizeh);
        stickTransform.sizeDelta = (3f/7f) * new Vector2(sizew, sizew);
        moveRadius = (GetComponent<RectTransform>().rect.width - stickTransform.rect.width) / 2f;
    }
}
