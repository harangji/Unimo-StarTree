using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterState_Disappear : MonsterState
{
    protected bool hasTriggerDisappear = false;
    public override void UpdateAction()
    {
        if (hasTriggerDisappear)
        {
            hasTriggerDisappear = false;
            float animTime = controller.enemyAnimator.GetCurrentAnimatorStateInfo(0).length;
            animTime *= 1f - controller.enemyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            StartCoroutine(CoroutineExtensions.DelayedActionCall(() =>
            { CallEnemyDestroy(); }, animTime));
        }
    }
    public override void TransitionAction(MonsterController controller)
    {
        base.TransitionAction(controller);
        controller.DeactiveEnemy();
        controller.enemyAnimator.SetTrigger("disappear"); 
        StartCoroutine(CoroutineExtensions.DelayedActionCall(() =>
        { hasTriggerDisappear = true; }, 0.26f));
    }
    protected void CallEnemyDestroy()
    {
        controller.DestroyEnemy();
    }
}
