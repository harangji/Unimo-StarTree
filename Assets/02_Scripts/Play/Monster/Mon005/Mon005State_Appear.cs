using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mon005State_Appear : MonsterState_Appear
{
    public override void TransitionAction(MonsterController controller)
    {
        this.controller = controller;
        int randSeed = UnityEngine.Random.Range(0, 10);
        UnityEngine.Random.InitState(System.DateTime.Now.Millisecond + randSeed);
        controller.enemyAnimator.Play("anim_MON005_move", 0, UnityEngine.Random.Range(0f,1f));
        invokePreactState();
    }
}