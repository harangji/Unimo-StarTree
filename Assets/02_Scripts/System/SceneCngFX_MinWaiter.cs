using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCngFX_MinWaiter : SceneChangeEffecter
{
    override protected void SceneLoadedAction(float duration)
    {
        StartCoroutine(CoroutineExtensions.DelayedActionCall(() => { isInit = true; }, duration));
    }
    override public void SceneChangeAction(float duration)
    {
        StartCoroutine(initWaitCoroutine(duration));
    }
    private IEnumerator initWaitCoroutine(float duration)
    {
        while (isInit == false) { yield return null; }
        yield return new WaitForSeconds(duration);
        IsReady = true;
        yield break;
    }
}
