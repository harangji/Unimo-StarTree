using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mon001Indicator : MonsterIndicatorCtrl
{
    private float deactiveRotate = -4.6f;
    private float minRotate = 0.1f;
    private float maxRotate = 1.16f;
    private float currentRatio = 0f;
    private float range = 3.7f;
    private float indicatorScale = 3.7f;
    private float transitTime = 0.4f;
    private Coroutine transitCoroutine;
    
    public override void InitIndicator()
    {
        indicatorMat.SetFloat("_rotate", deactiveRotate);
        indicatorMat.SetFloat("_ColorChange", 0f);
        //indicatorRender.SetPropertyBlock(indicatorMat);
        indicatorScale = range/transform.localScale.x;
        GetIndicatorTransform().localScale = indicatorScale * Vector3.one;
    }
    public override void ControlIndicator(float ratio)
    {
        if (ratio < currentRatio) { return; }
        ratio = Mathf.Clamp01(ratio);
        currentRatio = ratio;
        float minV = Mathf.Min(minRotate, indicatorMat.GetFloat("_rotate"));
        float rotateValue = ratio * maxRotate + (1 - ratio) * minV;
        indicatorMat.SetFloat("_rotate", rotateValue);
        indicatorMat.SetFloat("_ColorChange", ratio);
        //indicatorRender.SetPropertyBlock(indicatorMat);
    }
    public override void ActivateIndicator()
    {
        base.ActivateIndicator();
        if (transitCoroutine != null) { StopCoroutine(transitCoroutine); }
        transitCoroutine = StartCoroutine(indicatorSetCoroutine(minRotate));
    }
    public override void DeactivateIndicator()
    {
        if (transitCoroutine != null) { StopCoroutine(transitCoroutine); }
        if (currentRatio < 0.01f) { transitCoroutine = StartCoroutine(indicatorSetCoroutine(-3f)); }
        else { transitCoroutine = StartCoroutine(afterDashShrinkCoroutine()); }
        StartCoroutine(CoroutineExtensions.DelayedActionCall(() => { base.DeactivateIndicator(); }, transitTime));
    }
    private IEnumerator indicatorSetCoroutine(float targetRotate)
    {
        float lapsetime = 0f;
        float oriRotate = indicatorMat.GetFloat("_rotate");
        while (currentRatio < 0.01f && lapsetime<=transitTime)
        {
            lapsetime += Time.deltaTime;
            float ratio = Mathf.Clamp01(lapsetime/ transitTime);

            float rotateValue = ratio * targetRotate + (1 - ratio) * oriRotate;
            indicatorMat.SetFloat("_rotate", rotateValue);
            //indicatorRender.SetPropertyBlock(indicatorMat);
            yield return null;
        }
        yield break;
    }
    private IEnumerator afterDashShrinkCoroutine()
    {
        float lapsetime = 0f;
        while (currentRatio > 0.01f && lapsetime <= transitTime)
        {
            lapsetime += Time.deltaTime;
            float ratio = lapsetime / transitTime;
            ratio = Mathf.Clamp01(1f - 2f * ratio);
            float scale = ratio * indicatorScale;

            GetIndicatorTransform().localScale = scale * Vector3.one;
            indicatorMat.SetFloat("_ColorChange", ratio);
            //indicatorRender.SetPropertyBlock(indicatorMat);
            yield return null;
        }
        yield break;
    }
}
