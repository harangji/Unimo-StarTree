using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover_ST002 : PlayerMover
{
    private readonly float rotateThreshold = 0.05f;
    private float rotateTime = 0.3f;
    private Vector3 moveDir = Vector3.zero;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        moveBySpeed();
    }
    public void SetDirection(Vector2 direction)
    {
        visualCtrl.SetEquipSpecialAnimSpeed(direction.magnitude);
        if (IsStop || direction.magnitude < 0.01f)
        {
            moveDir = Vector3.zero;
            visualCtrl.SetMovingAnim(false);
            moveAudio.enabled = false;
            return;
        }
        visualCtrl.SetMovingAnim(true);
        moveAudio.enabled = true;
        moveAudio.volume = Sound_Manager.instance._audioSources[1].volume;
        Vector3 dir = new Vector3(direction.x, 0, direction.y);
        moveDir = dir;
    }
    private void moveBySpeed()
    {
        playerTransform.position += moveSpeed * Time.deltaTime * moveDir + pushSpeed * Time.deltaTime * pushDir;
        moveAudio.volume = moveSoundMax * Mathf.Clamp01(moveDir.magnitude);
        moveAudio.volume = Sound_Manager.instance._audioSources[1].volume;
        if (mapSetter.IsInMap(playerTransform.position) == false)
        {
            playerTransform.position = mapSetter.FindNearestPoint(playerTransform.position);
        }
        auraCtrl.transform.position = playerTransform.position + new Vector3(0f, auraOffset, 0f);
        if (pushDir.magnitude < 0.01f)
        {
            changeRotation(moveDir);
        }
        //if (IKActions != null) { IKActions.Invoke(); }
    }
    private void changeRotation(Vector3 dir)
    {
        if (Time.deltaTime < float.Epsilon) { return; }
        if (rotateTime < float.Epsilon) { transform.forward = dir; return; }
        float pow = Time.deltaTime / rotateTime;
        float followratio = 1 - Mathf.Clamp01(Mathf.Pow(rotateThreshold, pow));
        Vector3 newforward = Vector3.Slerp(transform.forward, dir, followratio);
        transform.forward = newforward;
    }
}
