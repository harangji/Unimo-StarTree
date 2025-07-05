using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterState_Appear : MonsterState
{
    public override void TransitionAction(MonsterController controller)
    {
        base.TransitionAction(controller);
        float animTime = controller.enemyAnimator.GetCurrentAnimatorStateInfo(0).length;
        StartCoroutine(CoroutineExtensions.DelayedActionCall(invokePreactState, animTime));
    }
    protected void invokePreactState()
    {
        controller.EnemyPreaction();
    }
}
