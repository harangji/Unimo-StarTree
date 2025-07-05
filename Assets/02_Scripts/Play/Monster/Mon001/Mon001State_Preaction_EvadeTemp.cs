using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mon001State_Preaction_EvadeTemp : MonsterState_Preaction
{
    protected float moveSpeed = 6f;
    public override void TransitionAction(MonsterController controller)
    {
        base.TransitionAction(controller);
    }
    public override void UpdateAction()
    {
        controller.transform.position += moveSpeed * Time.deltaTime * controller.transform.forward;
        if (controller.transform.position.y < 0) { invokeDisappearState(); }
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
