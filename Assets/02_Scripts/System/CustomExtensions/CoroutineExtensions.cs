using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public static class CoroutineExtensions
{
    public static IEnumerator DelayedActionCall(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action.Invoke();
        yield break;
    }
    public static IEnumerator ScaleInterpCoroutine(Transform targettransform, Vector3 targetscale, float duration)
    {
        float lapse = 0f;
        Vector3 oriscale = targettransform.localScale;
        while (lapse < duration)
        {
            lapse += Time.deltaTime;
            float ratio = Mathf.Clamp01(lapse / duration);
            Vector3 scale = ratio * targetscale + (1f - ratio) * oriscale;
            targettransform.localScale = scale;
            yield return null;
        }
        yield break;
    }
}
