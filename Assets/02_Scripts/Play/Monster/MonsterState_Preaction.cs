using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterState_Preaction : MonsterState
{
    protected bool hasTransit = false;
    public override void TransitionAction(MonsterController controller)
    {
        base.TransitionAction(controller);
        controller.ActivateEnemy();
    }
    public override void UpdateAction()
    {
        invokeActionState();
    }
    protected void invokeActionState()
    {
        if (!hasTransit)
        {
            hasTransit = true;
            controller.EnemyAction();
        }
    }
}
