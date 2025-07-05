using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;


public enum CameraMoveState
{
    Alta,
    InGame,
    Offline,
    Vane,
    
    Alta_LevelUP,
    Character,
    TeaPot,
    BonusReward
}
[System.Serializable]
public class CameraMoveEvent
{
    public CameraMoveState State;
    public Transform Pos;
    public Vector3 Plus_Pos;
    public Vector3 rotation;
    public float fieldView;
}
public class Camera_Event : MonoBehaviour
{
    [Header("## MASTER")]
    [Range(0.0f, 5.0f)]
    public float Timer;

    public static Camera_Event instance = null;
    public static bool GetCharacter = false;
    public static Transform CharacterTransform;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        mover = GetComponent<LobbyCameraMover>();
        zoomer = GetComponent<LobbyCameraZoomer>();
        moveClient = GetComponent<LobbyCamMoveClient>();
        zoomClient = GetComponent<LobbyCamZoomClient>();
        ctrl = GetComponent<LobbyClientMasterCtrl>();
        cam = transform.GetChild(0).GetComponent<Camera>();

        savePos = transform.position;
        saveRot = transform.eulerAngles;
        saveView = cam.fieldOfView;
    }
    LobbyCameraMover mover;
    LobbyCameraZoomer zoomer;
    LobbyCamMoveClient moveClient;
    LobbyCamZoomClient zoomClient;
    LobbyClientMasterCtrl ctrl;
    Camera cam;

    public CameraMoveEvent[] events;

    Coroutine coroutine;

    Vector3 savePos, saveRot;
    float saveView;

    Dictionary<CameraMoveState, CameraMoveEvent> states = new Dictionary<CameraMoveState, CameraMoveEvent>();
    private void Start()
    {
        for (int i = 0; i < events.Length; i++)
        {
            states.Add(events[i].State, events[i]);
        }
    }

    private void Update()
    {
        if (GetCharacter == false) return;
        if (CharacterTransform == null) return;

        Vector3 pos = CharacterTransform.position;
        transform.position = new Vector3(pos.x - 4.0f, pos.y -5.0f, pos.z - 2.0f);
    }

    public void MoverChange(bool isBoolean)
    {
        mover.enabled = isBoolean;
        zoomer.enabled = isBoolean;
        moveClient.enabled = isBoolean;
        zoomClient.enabled = isBoolean;
        ctrl.enabled = isBoolean;
    }

    public void GetCameraEvent(CameraMoveState state)
    {
        MoverChange(false);

        Vector3 pos, rot;
        float view;

        var eventData = states[state];

        pos = eventData.Pos.position + eventData.Plus_Pos;
        rot = eventData.rotation;
        view = eventData.fieldView;

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(CameraMoveCoroutine(pos,rot,view));
    }

    public void ReturnCamera()
    {
        CharacterTransform = null;
        GetCharacter = false;

        transform.parent = null;
        MoverChange(true);

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(CameraMoveCoroutine(savePos, saveRot, saveView));
    }

    IEnumerator CameraMoveCoroutine(Vector3 pos, Vector3 rotation, float fieldView)
    {
        float current = 0.0f;
        float percent = 0.0f;
        Vector3 startPos = transform.position;
        Vector3 endPos = pos;
        Quaternion startRot = transform.rotation;
        Quaternion endRot = Quaternion.Euler(rotation);
        float startView = cam.fieldOfView;
        float endView = fieldView;
        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / Timer;
            Vector3 LerpPos = Vector3.Lerp(startPos, endPos, percent);
            Quaternion rot = Quaternion.Lerp(startRot, endRot, percent);
            float view = Mathf.Lerp(startView, endView, percent);

            transform.position = LerpPos;
            transform.rotation = rot;
            cam.fieldOfView = view;
            yield return null;
        }

    }
 
}
