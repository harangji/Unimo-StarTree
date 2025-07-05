using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mon002State_PreAction : MonsterState_Preaction
{
    [SerializeField] private float waitTime = 0.1f;
    [SerializeField] private float indicatorTime = 1f;
    [SerializeField] private float indicatorWaitTime = 0.5f;
    private float currentTime = 0f;

    public override void TransitionAction(MonsterController controller)
    {
        base.TransitionAction(controller);
        StartCoroutine(indicatorCoroutine());
        if (controller.gameObject.TryGetComponent<Rigidbody>(out var rigidbody))
        {
            rigidbody.linearVelocity = Vector3.zero;
        }
    }
    public override void UpdateAction()
    {
        currentTime += Time.deltaTime;
        if (currentTime > waitTime) 
        {
            invokeActionState(); 
        }
    }
    private IEnumerator indicatorCoroutine()
    {
        yield return new WaitForSeconds(indicatorWaitTime);
        float lapseTime = 0f;
        while (lapseTime <= indicatorTime)
        {
            lapseTime += Time.deltaTime;
            float ratio = Mathf.Clamp01(lapseTime/indicatorTime);
            controller.indicatorCtrl.ControlIndicator(Mathf.Pow(ratio,1.5f));
            yield return null;
        }
        yield break;
    }
}
