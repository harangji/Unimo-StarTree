using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCamMoveClient : LobbyCamClientMember
{
    private LobbyCameraMover cameraMover;
    private Vector2 prevPos = Vector2.zero;
    private bool hasSetPrev = false;
    // Start is called before the first frame update
    void Start()
    {
        cameraMover = GetComponent<LobbyCameraMover>();
        isInControl = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (!isInControl)
        {
            return;
        }
        if (Input.touchCount == 0)
        {
            cameraMover.PanToMoveCam(Vector3.zero, 0f, false);
            hasSetPrev = false;
            return;
        }
        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
        {
            Vector2 touchPos = touch.position;
            if (!hasSetPrev)
            {
                hasSetPrev = true;
                prevPos = touchPos;
            }
            Vector2 diff = prevPos - touchPos;

            float dragSpeed = diff.magnitude / Time.deltaTime;
            cameraMover.PanToMoveCam(diff, dragSpeed, true);
            prevPos = touchPos;
        }
    }
    public void StartMoveCam(Vector3 moveStartPos)
    {
        if (!isInControl)
        {
            hasSetPrev = true;
            isInControl = true;
            prevPos = moveStartPos;
        }
    }
    public void StopMoveCam()
    {
        if (isInControl)
        {
            hasSetPrev = false;
            isInControl = false;
        }
    }
}
