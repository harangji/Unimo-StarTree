using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mon006Indicator : MonsterIndicatorCtrl
{
    [SerializeField] private LineRenderer LRender;
    private Vector3[] vP;
    public override void InitIndicator()
    {
        indicatorMat.SetFloat("_ColorChange", 0f);
        vP = new Vector3[LRender.positionCount];
        LRender.GetPositions(vP);
    }
    public override void ControlIndicator(float ratio)
    {
        ratio = Mathf.Clamp01(ratio);
        indicatorMat.SetFloat("_ColorChange", ratio);
    }
    public void SetIndicatorLength(float length)
    {
        vP[1] = new Vector3(0,0,Mathf.Abs(length));
        LRender.SetPositions(vP);
    }
    public void SetIndicatorWidth(float width)
    {
        width *= 1.2f;
        LRender.widthMultiplier = width;
    }
}
