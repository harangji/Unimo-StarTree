using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCameraMover : MonoBehaviour
{
    private Transform cameraPivot;
    private Camera targetCam;

    private float scrnToWorldSens = 0.025f;
    private Vector3 moveDir = Vector3.zero;
    private float zMoveCorrection = 0.7f / Mathf.Sin(Mathf.PI / 9);
    private float currentspeed = 0;
    private readonly float maxSpeed = 45f;
    private readonly float dragCoeff = 4.5f;
    private readonly float descelation = 0.7f;
    private float xMin = -25;
    private float xMax = 25;
    private float zMin = -45; //Boundary will be adjusted by map sizes
    private float zMax = 45;
    // Start is called before the first frame update
    void Start()
    {
        cameraPivot = transform;
        targetCam = GetComponentInChildren<Camera>();
    }
    public void PanToMoveCam(Vector2 dir, float speed, bool isTouch)
    {
        if (isTouch)
        {
            Vector3 newmoveDir = new Vector3(dir.x, 0f, dir.y);
            newmoveDir.Normalize();
            newmoveDir.z *= zMoveCorrection;
            float ratio = Mathf.Clamp01(11f * Time.deltaTime);
            currentspeed = (1-ratio) * currentspeed + ratio * scrnToWorldSens * speed;
            ratio = Mathf.Clamp01(2.8f * ratio);
            moveDir = (1 - ratio) * moveDir + ratio * newmoveDir;
            if (currentspeed > maxSpeed) { currentspeed = maxSpeed; }
        }
        moveCamera(!isTouch);
    }
    public void SetPanSpeed(float ratio)
    {
        scrnToWorldSens = 0.009f * (1f - ratio) + 0.027f * ratio;
    }
    private void moveCamera(bool isDrag)
    {
        if (isDrag)
        {
            currentspeed -= (dragCoeff * currentspeed + descelation )* Time.deltaTime;
            if (currentspeed < 0f) { currentspeed = 0f; }
        }
        cameraPivot.position += currentspeed * Time.deltaTime * moveDir;
        cameraPivot.position = calcBoundedPosition(cameraPivot.position);
    }
    private Vector3 calcBoundedPosition(Vector3 pos)
    {
        if (pos.x > xMax) { pos.x = xMax; }
        if (pos.x < xMin) { pos.x = xMin; }
        if (pos.z > zMax) { pos.z = zMax; }
        if (pos.z < zMin) { pos.z = zMin; }

        return pos;
    }
}
