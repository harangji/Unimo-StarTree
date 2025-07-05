using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeEffecter : MonoBehaviour
{
    public bool IsReady { get; protected set; } = false;
    protected bool isInit = false;
    // Start is called before the first frame update
    protected void Awake()
    {
        SceneLoadedAction(WholeSceneController.SCENETRANSITTIME);
    }

    virtual protected void SceneLoadedAction(float duration)
    {

    }
    virtual public void SceneChangeAction(float duration)
    {

    }
}
