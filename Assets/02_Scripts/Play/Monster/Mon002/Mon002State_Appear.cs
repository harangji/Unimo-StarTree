using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mon002State_Appear : MonsterState_Appear
{
    [SerializeField] private float waitTime = 1f;
    public override void TransitionAction(MonsterController controller)
    {
        this.controller = controller;
        StartCoroutine(CoroutineExtensions.DelayedActionCall(() => { invokePreactState(); }, waitTime));
    }
}