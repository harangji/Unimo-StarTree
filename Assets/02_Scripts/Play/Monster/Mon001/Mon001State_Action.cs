using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mon001State_Action : MonsterState_Action
{
    [SerializeField] private AudioSource audioSource;
    protected float dashSpeed = 18f;
    protected float dashTime = 1f;
    private bool hasTriggerAction = false;
    private bool hasStartDash = false;
    private bool isDashing = false;
    private float timeAfterDash = 0f;

    public override void TransitionAction(MonsterController controller)
    {
        base.TransitionAction(controller);
        controller.enemyAnimator.SetTrigger("action");
        StartCoroutine(CoroutineExtensions.DelayedActionCall(() =>
        { hasTriggerAction = true; }, 0.26f));
    }
    public override void UpdateAction()
    {
        if (hasTriggerAction)
        {
            hasTriggerAction = false;
            hasStartDash = true;
            float animTime = controller.enemyAnimator.GetCurrentAnimatorStateInfo(0).length;
            animTime *= 1f-controller.enemyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            StartCoroutine(CoroutineExtensions.DelayedActionCall(() => 
            { isDashing = true; controller.indicatorCtrl.ControlIndicator(1f); audioSource.enabled = true; }, animTime));
        }
        if (hasStartDash && !isDashing)
        {
            float ratio = controller.enemyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;

            controller.indicatorCtrl.ControlIndicator(ratio);
        }
        if (isDashing) 
        {
            timeAfterDash += Time.deltaTime;
            float ratio = timeAfterDash / dashTime;
            //ratio *= ratio;
            ratio = Mathf.Clamp01(1f - 0.8f * Mathf.Pow(ratio, 0.8f));
            float speed = ratio * dashSpeed;
            controller.enemyAnimator.speed = Mathf.Sqrt(ratio);
            controller.transform.position += speed * Time.deltaTime * controller.transform.forward;
            if (timeAfterDash > dashTime) 
            {
                controller.enemyAnimator.speed = 1f;
                controller.EnemyDisappear(); 
            }
        }
    }
}
