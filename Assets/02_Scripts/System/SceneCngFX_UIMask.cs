using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCngFX_UIMask : SceneChangeEffecter
{
    [SerializeField] private Animator SceneMask;
    private float animWait = 1f;
    override protected void SceneLoadedAction(float duration)
    {
        SceneMask.SetTrigger("maskin");
        StartCoroutine(CoroutineExtensions.DelayedActionCall(() => {
            SceneMask.gameObject.SetActive(false);
            isInit = true;
        }, Mathf.Max(animWait,duration)));
    }
    override public void SceneChangeAction(float duration)
    {
        SceneMask.gameObject.SetActive(true);
        SceneMask.SetTrigger("maskout");
        StartCoroutine(CoroutineExtensions.DelayedActionCall(() => {
            IsReady = true;
        }, Mathf.Max(animWait, duration)));
    }
}
