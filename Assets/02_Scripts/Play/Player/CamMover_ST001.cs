using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CamMover_ST001 : MonoBehaviour
{
    private float maxMapRadius = 17.5f;
    private float camVRangeLimit = 11.5f;
    private float camHRangeLimit = 15.5f; // will be modified to be changed by display resolution
    private float followTime = 0.25f;
    private readonly float followThreshold = 0.025f;
    private float elipseModifier = 0.8f;
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
    public void setMaximumRange(float mapradius)
    {
        maxMapRadius = mapradius;
        float ratio = Screen.width / Screen.height;
        if (ratio > 1f) { ratio = 2 - 1f/ratio; }
        ratio = Mathf.Clamp01(ratio - 0.5f);
        float vdiff = ratio * 1.5f + (1 - ratio) * 5f;
        float hdiff = (1 - ratio) * 1.5f + ratio * 5f;
        camVRangeLimit = maxMapRadius - vdiff;
        camHRangeLimit = maxMapRadius - hdiff;
        elipseModifier = Mathf.Sqrt(camVRangeLimit * camHRangeLimit) / maxMapRadius;
    }
    private void changeCamPos()
    {
        if (!isFollow) { return; }
        if (camHRangeLimit <= 0f && camVRangeLimit <= 0f) { return; }
        if (Time.deltaTime < float.Epsilon) { return; }

        float actfollowTime = followTime; //(3f - 2f * Mathf.Exp(-elipseparam / 0.03f)) (1f + 2f * Mathf.Pow(elipseparam,1.5f)) * 
        float followratio = (actfollowTime > 10 * float.Epsilon) ? 
            1-Mathf.Clamp01(Mathf.Pow(followThreshold, Time.deltaTime / actfollowTime)) : 1;
        Vector3 diff = followratio * (calcTargetPos() - transform.position);
        transform.position += diff;
        if (isRestrictedDirectionCam())
        {
            positionRestrictDirCam();
            return;
        }
    }
    private Vector3 calcTargetPos()
    {
        float elipseparam = (playerTransform.position.x / camHRangeLimit) * (playerTransform.position.x / camHRangeLimit) +
            (playerTransform.position.z / camVRangeLimit) * (playerTransform.position.z / camVRangeLimit);
        elipseparam *= elipseModifier;
        float ratio = Mathf.Clamp01(elipseparam);
        ratio = Mathf.Pow(ratio, 0.5f);
        Vector3 target = (1 - ratio) * playerTransform.position + ratio * ratio * positionElipseCam();
        return target;
    }
    private bool isRestrictedDirectionCam()
    {
        if (camHRangeLimit <= 0f) { return true; }
        if (camVRangeLimit <= 0f) { return true; }
        return false;
    }
    private Vector3 positionElipseCam()
    {
        float angle = playerTransform.position.AngleInXZ();
        Vector3 newpos = new Vector3(camHRangeLimit * Mathf.Cos(angle), 0f, camVRangeLimit * Mathf.Sin(angle));
        return newpos;
    }
    private void positionRestrictDirCam()
    {
        Vector3 newpos = Vector3.zero;
        if (camHRangeLimit <= 0f)
        {
            newpos = transform.position;
            newpos.x = 0;
            if (Mathf.Abs(newpos.z) >= camVRangeLimit)
            {
                newpos.z = Mathf.Clamp(newpos.z, -camVRangeLimit, camVRangeLimit);
            }
        }
        else if (camVRangeLimit <= 0f)
        {
            newpos = transform.position;
            newpos.z = 0;
            if (Mathf.Abs(newpos.x) >= camHRangeLimit)
            {
                newpos.x = Mathf.Clamp(newpos.x, -camHRangeLimit, camHRangeLimit);
            }
        }
        transform.position = newpos;
    }
}
