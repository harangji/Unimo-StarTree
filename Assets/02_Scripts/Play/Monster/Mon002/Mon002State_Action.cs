using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mon002State_Action : MonsterState_Action
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float audioStartDist = 15f;
    [SerializeField] private float waitTimebeforeDeactive = 1.5f;
    [SerializeField] private float dashSpeed = 20f;
    private bool hasDeactive = false;
    private bool isReadyforDisappear = false;
    private float timeAfterDeactive = 0f;
    public override void TransitionAction(MonsterController controller)
    {
        base.TransitionAction(controller);
        if (controller.gameObject.TryGetComponent<Rigidbody>(out var rigidbody))
        {
            rigidbody.linearVelocity = dashSpeed * controller.transform.forward;
        }
    }
    public override void UpdateAction()
    {
        base.UpdateAction();

        if (waitTimebeforeDeactive >= 0f) { waitTimebeforeDeactive -= Time.deltaTime; }
        else
        {
            float dist = (controller.playerTransform.position - controller.transform.position).magnitude;
            if (dist < audioStartDist && audioSource.enabled == false) { audioSource.enabled = true; }
            float projdist = Vector3.Dot(controller.playerTransform.position - controller.transform.position, controller.transform.forward);
            if (isReadyforDisappear)
            {
                if (projdist < -4f && !hasDeactive)
                {
                    hasDeactive = true;
                    controller.DeactiveEnemy();
                }
                if (hasDeactive)
                {
                    timeAfterDeactive += Time.deltaTime;
                    if (timeAfterDeactive > 2f) { controller.EnemyExplode(); }
                }
            }
            else
            {
                if (projdist < 0f) { isReadyforDisappear = true; }
            }
        }
    }
}
