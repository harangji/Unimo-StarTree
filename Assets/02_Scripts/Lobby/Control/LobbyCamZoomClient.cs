using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCamZoomClient : LobbyCamClientMember
{
    private LobbyCameraZoomer camZoomer;
    private float prevDist = 0f;
    private bool hasSetprevDist = false;
    private void Start()
    {
        camZoomer = GetComponent<LobbyCameraZoomer>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!isInControl) { return; }
        if (Input.touchCount < 2) { hasSetprevDist = false; return; }
        Touch touch1 = Input.GetTouch(0);
        Touch touch2 = Input.GetTouch(1);
        if ((touch1.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Stationary) &&
            (touch2.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Stationary))
        {
            Vector2 diff = touch1.position - touch2.position;
            float newdist = diff.magnitude;
            if (!hasSetprevDist)
            {
                hasSetprevDist = true;
                prevDist = newdist;
            }
            float distDiff = newdist - prevDist;
            camZoomer.PinchToZoom(distDiff);
            prevDist = newdist;
        }
    }
    public void StartZoomCam(float dist)
    {
        if (!isInControl)
        {
            isInControl = true;
            hasSetprevDist = true;
            prevDist = dist;
        }
    }
    public void StopZoomCam()
    {
        if (isInControl)
        {
            isInControl = false;
            hasSetprevDist = false;
        }
    }
}
