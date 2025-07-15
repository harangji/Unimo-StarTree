using UnityEngine;

public class BossState_Appear : MonsterState_Appear
{
    public override void TransitionAction(MonsterController controller)
    {
        base.TransitionAction(controller);
        float animTime = controller.enemyAnimator.GetCurrentAnimatorStateInfo(0).length;
        StartCoroutine(CoroutineExtensions.DelayedActionCall(invokePreactState, animTime + 2f));
    }
}
