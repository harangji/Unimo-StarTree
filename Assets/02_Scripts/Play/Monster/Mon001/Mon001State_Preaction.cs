using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mon001State_Preaction : MonsterState_Preaction
{
    protected float moveSpeed = 2.5f;
    protected float existTime = 4.3f;
    [SerializeField] private float detectionRange = 3.7f;
    private float playerSize = 0f;
    private float detectionAngle = Mathf.PI / 6f;
    public override void TransitionAction(MonsterController controller)
    {
        base.TransitionAction(controller);
        //controller.indicatorCtrl.SetIndicatorScale(detectionRange);
        playerSize = controller.playerTransform.localScale.x/2f;
    }
    public override void UpdateAction()
    {
        existTime -= Time.deltaTime;
        if (existTime < 0) { invokeDisappearState(); }
        else
        {
            controller.transform.position += moveSpeed * Time.deltaTime * controller.transform.forward;
            if (isPlayerInRange()) { invokeActionState(); }
        }
    }
    private bool isPlayerInRange()
    {
        Vector3 vec = controller.playerTransform.position - controller.transform.position;
        float dist = vec.magnitude;
        if (dist < detectionRange + playerSize)
        {
            float dot = Vector3.Dot(transform.forward, vec.normalized);
            if (dot > Mathf.Cos(detectionAngle))
            {
                return true;
            }
        }
        return false;
    }
    private void invokeDisappearState()
    {
        if (!hasTransit)
        {
            hasTransit = true;
            controller.EnemyDisappear();
        }
    }
}