using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMover_ST002 : MonoBehaviour
{
    private float followTime = 0.25f;
    private readonly float followThreshold = 0.025f;
    private float followModifier = 0.075f;
    private Transform playerTransform;
    private bool isFollow = false;
    private void Start()
    {
        playerTransform = FindAnyObjectByType<PlayerMover>().transform;
        StartCoroutine(CoroutineExtensions.DelayedActionCall(() => { isFollow = true; }, PlayProcessController.InitTimeSTATIC + 0.05f));
    }
    // Update is called once per frame
    void LateUpdate()
    {
        changeCamPos();
    }
    private void changeCamPos()
    {
        if (!isFollow) { return; }
        if (Time.deltaTime < float.Epsilon) { return; }

        float actfollowTime = followTime;
        float followratio = (actfollowTime > 10 * float.Epsilon) ?
            1 - Mathf.Clamp01(Mathf.Pow(followThreshold, Time.deltaTime / actfollowTime)) : 1;
        Vector3 diff = followratio * (calcCamPos() - transform.position);
        transform.position += diff;
    }
    private Vector3 calcCamPos()
    {
        Vector3 pos = followModifier * playerTransform.position;
        pos.y = 0;
        pos.z = 0;
        return pos;
    }
}

