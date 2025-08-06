using UnityEngine;

public class Mon005State_Appear_C : MonsterState_Appear
{
    public override void TransitionAction(MonsterController controller)
    {
        this.controller = controller;
        
        controller.canCollide = false;
        
        float animTime = controller.enemyAnimator.GetCurrentAnimatorStateInfo(0).length;
        StartCoroutine(CoroutineExtensions.DelayedActionCall(invokePreactState, animTime));
    }
}
