using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mon002Indicator : MonsterIndicatorCtrl
{
    private float deactiveTime = 0.25f;
    private void OnDisable()
    {
        if (indicatorObj != null) { Destroy(indicatorObj); }
    }
    public override void InitIndicator()
    {
        ControlIndicator(0f);
    }
    public override void ControlIndicator(float ratio)
    {
        ratio = Mathf.Clamp01(ratio);
        GetIndicatorTransform().localScale = new Vector3(ratio, 1f, 1f);
        indicatorMat.SetFloat("_Alpha", 1);
        indicatorMat.SetFloat("_ColorChange", ratio);
        //indicatorRender.SetPropertyBlock(indicatorMat);
    }
    public override void DeactivateIndicator()
    {
        StartCoroutine(deactiveCoroutine());
    }
    private IEnumerator deactiveCoroutine()
    {
        float lapseTime = 0f;
        while (lapseTime <= deactiveTime)
        {
            lapseTime += Time.deltaTime;
            float ratio = 1-Mathf.Clamp01(lapseTime/deactiveTime);
            ControlIndicator(ratio);
            yield return null;
        }
        Destroy(indicatorObj);
        yield break;
    }
}
