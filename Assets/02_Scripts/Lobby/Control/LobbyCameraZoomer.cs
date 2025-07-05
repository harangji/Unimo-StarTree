using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCameraZoomer : MonoBehaviour
{
    private Camera targetCam;

    private float pinchDisttoFOVSens = 0.022f;
    public float maxFOV = 80.0f;
    public float minFOV = 10f;

    private LobbyCameraMover mover;
    // Start is called before the first frame update
    void Start()
    {
        targetCam = GetComponentInChildren<Camera>();
        mover = GetComponent<LobbyCameraMover>();
    }

    public void PinchToZoom(float pinchDistdiff)
    {
        float FOVdiff = pinchDistdiff * pinchDisttoFOVSens;
        targetCam.fieldOfView -= FOVdiff;
        if (targetCam.fieldOfView > maxFOV) { targetCam.fieldOfView = maxFOV; }
        else if (targetCam.fieldOfView < minFOV) { targetCam.fieldOfView = minFOV; }
        setMoverSpeed();
    }
    private void setMoverSpeed()
    {
        float ratio = (targetCam.fieldOfView - minFOV) / (maxFOV - minFOV);
        mover.SetPanSpeed(ratio);
    }
}
