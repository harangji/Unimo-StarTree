using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMaskController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TempSceneCtrl scenectrl = FindAnyObjectByType<TempSceneCtrl>();
        scenectrl.SceneMask = GetComponent<Animator>();
        scenectrl.SceneMask.SetTrigger("maskin");
        StartCoroutine(CoroutineExtensions.DelayedActionCall(() => { gameObject.SetActive(false); }, 1f));
    }
}
