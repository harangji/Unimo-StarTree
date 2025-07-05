using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyClientMasterCtrl : MonoBehaviour
{
    private LobbyCamMoveClient mover;
    private LobbyCamZoomClient zoomer;

    private ELobbyClientStat currentState = ELobbyClientStat.IDLE;
    private void Awake()
    {
        LobbyCamClientMember.SetMasterCtrlSTATIC(this);
    }
    private void Start()
    {
        mover = GetComponent<LobbyCamMoveClient>();
        zoomer = GetComponent<LobbyCamZoomClient>();
    }

    private void Update()
    {
        switch(currentState)
        {
            case ELobbyClientStat.IDLE:
                IDLEaction();
                break;
            case ELobbyClientStat.MOVE:
                MOVEaction();
                break;
            case ELobbyClientStat.ZOOM:
                ZOOMaction();
                break;
            default:
                IDLEaction();
                break;
        }
    }

    private void IDLEaction()
    {
        if (Input.touchCount == 1)
        {
            currentState = ELobbyClientStat.MOVE;
            Touch touch = Input.GetTouch(0);
            mover.StartMoveCam(touch.position);
        }
        if (Input.touchCount == 2)
        {
            currentState = ELobbyClientStat.ZOOM;
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);
            Vector2 diff = touch1.position - touch2.position;
            zoomer.StartZoomCam(diff.magnitude);
            mover.StopMoveCam();
        }
    }
    private void MOVEaction()
    {
        if (Input.touchCount == 0)
        {
            currentState = ELobbyClientStat.IDLE;
        }
        else if (Input.touchCount ==2)
        {
            currentState = ELobbyClientStat.ZOOM;
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);
            Vector2 diff = touch1.position - touch2.position;
            zoomer.StartZoomCam(diff.magnitude);
            mover.StopMoveCam();
        }
    }
    private void ZOOMaction()
    {
        if (Input.touchCount == 0)
        {
            currentState = ELobbyClientStat.IDLE;
            zoomer.StopZoomCam();
            mover.StartMoveCam(Vector3.zero);
        }
    }
}

public enum ELobbyClientStat
{
    IDLE = 0,
    MOVE = 1,
    ZOOM = 2,
}