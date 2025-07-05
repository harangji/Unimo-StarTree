using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKStartBone : MonoBehaviour
{
    private Vector3 prevprevPosition;
    private Vector3 prevPosition;
    private float prevDeltaTime;

    private IKSource IKSourceObj;
    private bool noPlayer;
    // Start is called before the first frame update
    void Start()
    {
        IKSourceObj = GetComponentInChildren<IKSource>();
        IKSourceObj.Initialize(transform);
        prevPosition = transform.position;
        prevprevPosition = prevPosition;
        prevDeltaTime = 1f;

        //noPlayer = true;
        StartCoroutine(IKUpdateCoroutine());
        //PlayerMover mover = GetComponentInParent<PlayerMover>();
        //if (mover != null)
        //{
        //    mover.IKActions += IKUpdateAct;
        //    noPlayer = false;
        //}
        //else
        //{
        //    UnimoAutoMover lobbyMover = GetComponentInParent<UnimoAutoMover>();
        //    if (lobbyMover != null)
        //    {
        //        lobbyMover.IKActions += IKUpdateAct;
        //        noPlayer = false;
        //    }
        //    else { noPlayer = true; }
        //}
    }
    //private void LateUpdate()
    //{
    //    if (noPlayer) { IKUpdateAct(); }
    //}
    private void IKUpdateAct()
    {
        if (Time.timeScale < 0.5f || Time.deltaTime <= 4*float.Epsilon) { return; }
        IKSourceObj.ApplyInertia(calcAcceleration(), (transform.position - prevPosition) / Time.deltaTime);
        renewData();
    }
    private void renewData()
    {
        prevprevPosition = prevPosition;
        prevPosition = transform.position;
        prevDeltaTime = Time.deltaTime;
    }
    private Vector3 calcAcceleration()
    {
        float dt = Time.deltaTime;
        Vector3 currentPos = transform.position;
        Vector3 prevVelocity = (prevPosition - prevprevPosition) / prevDeltaTime;
        Vector3 currentVelocity = (currentPos - prevPosition) / dt;
        return 2 * (currentVelocity - prevVelocity) / (dt + prevDeltaTime);
    }
    private IEnumerator IKUpdateCoroutine()
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();
        while (true)
        {
            IKUpdateAct();
            yield return wait;
        }
    }
}
