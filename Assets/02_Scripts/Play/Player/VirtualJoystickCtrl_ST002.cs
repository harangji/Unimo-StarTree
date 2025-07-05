using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualJoystickCtrl_ST002 : MonoBehaviour
{
    private PlayerMover_ST002 mover;
    private VitualStickImager virtualStick;
    private Vector2 stickCenterPos;
    private float controlWidth = 130;
    private bool isStopControl = false;
    private void Start()
    {
        mover = GetComponent<PlayerMover_ST002>();
        virtualStick = FindAnyObjectByType<VitualStickImager>();
        virtualStick.setStickImgSizes(2f * controlWidth, controlWidth);
        virtualStick.gameObject.SetActive(false);

        PlaySystemRefStorage.playProcessController.SubscribeGameoverAction(stopControl);
        PlaySystemRefStorage.playProcessController.SubscribePauseAction(stopControl);
        PlaySystemRefStorage.playProcessController.SubscribeResumeAction(resumeControl);
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
            virtualStick.GetComponent<RectTransform>().position = touch.position;
            stickCenterPos = touch.position;
            virtualStick.gameObject.SetActive(true);
        }
        else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
        {
            Vector2 dir = convertToDirection(touch.position);
            mover.SetDirection(dir);
            dir.y = 0;
            virtualStick.setStickPos(dir);
        }
    }
    private void OnDisable()
    {
        if (virtualStick == null) { return; }
        if (virtualStick.gameObject.activeSelf == true) { virtualStick.gameObject.SetActive(false); }
    }
    private Vector2 convertToDirection(Vector2 inputPos)
    {
        Vector2 diff = inputPos - stickCenterPos;
        diff /= controlWidth;
        diff.x = Mathf.Clamp(diff.x, -1f, 1f);
        diff.y = Mathf.Clamp(diff.y, -1f, 1f);

        return diff;
    }
    private void stopControl()
    {
        isStopControl = true;
        mover.SetDirection(Vector2.zero);
        virtualStick.gameObject.SetActive(false);
    }
    private void resumeControl()
    {
        isStopControl = false;
    }
}