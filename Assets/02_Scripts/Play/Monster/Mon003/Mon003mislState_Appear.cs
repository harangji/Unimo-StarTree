using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mon003mislState_Appear : MonsterState_Appear
{
    public override void TransitionAction(MonsterController controller)
    {
        this.controller = controller;
        invokePreactState();
    }
}
