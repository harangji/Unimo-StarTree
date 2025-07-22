using UnityEngine;

public class SafeIndicator : MonsterIndicatorCtrl
{
    private float minScale = 0.01f;
    private float maxScale = 1f;
    
    public override void InitIndicator()
    {
        indicatorMat.SetFloat("_InnerScale", minScale);
        indicatorMat.SetFloat("_ColorChange", 0f);
    }
    
    public override void ControlIndicator(float ratio)
    {
        ratio = Mathf.Clamp01(ratio);
        indicatorMat.SetFloat("_InnerScale", Mathf.Clamp(ratio,minScale,maxScale));
        indicatorMat.SetFloat("_ColorChange", ratio);
    }
}