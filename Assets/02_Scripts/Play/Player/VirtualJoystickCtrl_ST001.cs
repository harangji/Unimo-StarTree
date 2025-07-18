using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirtualJoystickCtrl_ST001 : MonoBehaviour
{
    private PlayerMover_ST001 mover;
    private VitualStickImager virtualStick;
    private Vector2 stickCenterPos;
    private RectTransform virtualStickRectTransform;
    private const float CONTROL_RADIUS = 140;
    private bool isStopControl = false;
    
    private void Start()
    {
        mover = GetComponent<PlayerMover_ST001>();
        virtualStick = FindAnyObjectByType<VitualStickImager>();
        virtualStick.setStickImgSizes(2f * CONTROL_RADIUS, 2f * CONTROL_RADIUS);
        virtualStick.gameObject.SetActive(false);

        PlaySystemRefStorage.playProcessController.SubscribeGameoverAction(StopControl);
        PlaySystemRefStorage.playProcessController.SubscribePauseAction(StopControl);
        PlaySystemRefStorage.playProcessController.SubscribeResumeAction(ResumeControl);
        
        virtualStickRectTransform = virtualStick.GetComponent<RectTransform>();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (isStopControl) { return; }
        if (Input.touchCount == 0) 
        { 
            if (virtualStick.gameObject.activeSelf == true) { mover.SetDirection(Vector2.zero); virtualStick.gameObject.SetActive(false); }
            return; 
        }
        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Ended && touch.phase == TouchPhase.Canceled)
        {
            mover.SetDirection(Vector2.zero);
            virtualStick.gameObject.SetActive(false);
        }
        else if (touch.phase == TouchPhase.Began)
        {
            virtualStickRectTransform.position= touch.position;
            stickCenterPos = touch.position;
            virtualStick.gameObject.SetActive(true);
        }
        else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) 
        {
            Vector2 dir = ConvertToDirection(touch.position);
            mover.SetDirection(ConvertToDirection(touch.position));
            virtualStick.setStickPos(dir);
        }
    }
    
    private void OnDisable()
    {
        if (virtualStick == null) { return; }
        if (virtualStick.gameObject.activeSelf == true) { virtualStick.gameObject.SetActive(false); }
    }
    
    private Vector2 ConvertToDirection(Vector2 inputPos)
    {
        Vector2 diff = inputPos - stickCenterPos;
        diff /= CONTROL_RADIUS;
        float radius = Mathf.Clamp01(diff.magnitude);
        float angle = diff.AngleInXZ();
        Vector2 dir = radius * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

        return dir;
    }
    
    private void StopControl()
    {  
        isStopControl = true;
        mover.SetDirection(Vector2.zero); 
        virtualStick.gameObject.SetActive(false);
    }
    
    private void ResumeControl()
    {
        isStopControl = false;
    }
}
