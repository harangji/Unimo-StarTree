using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mon004State_Preaction : MonsterState_Preaction
{
    public override void TransitionAction(MonsterController controller)
    {
        base.TransitionAction(controller);
        controller.indicatorCtrl.DeactivateIndicator();
        checkPreActAnimDuration();
    }
    public override void UpdateAction()
    { }
    private void checkPreActAnimDuration()
    {
        StartCoroutine(CoroutineExtensions.DelayedActionCall(startJumping, 0.05f));
    }
    private void startJumping()
    {
        float animTime = controller.enemyAnimator.GetCurrentAnimatorStateInfo(0).length;
        animTime *= 1f - controller.enemyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        StartCoroutine(CoroutineExtensions.DelayedActionCall(() =>
        {
            controller.enemyCollider.enabled = false;
            controller.EnemyAction(); }, animTime));
    }
}
