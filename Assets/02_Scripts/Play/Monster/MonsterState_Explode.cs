using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterState_Explode : MonsterState
{
    public override void TransitionAction(MonsterController controller)
    {
        base.TransitionAction(controller);
        //Instantiate(ExplodeFX);
        controller.DestroyEnemy();
    }
}
