using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCngFX_ManualSet : SceneChangeEffecter
{
    override protected void SceneLoadedAction(float duration)
    {
        isInit = true;
    }
    override public void SceneChangeAction(float duration)
    {

    }
    public void ManualReady()
    {
        IsReady = true;
    }
}
