using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mon004State_Explode : MonsterState_Explode
{
    [SerializeField] private GameObject ExplodeFX;
    public override void TransitionAction(MonsterController controller)
    {
        Instantiate(ExplodeFX,controller.transform.position + 1.5f * Vector3.up, Quaternion.identity);
        base.TransitionAction(controller);
    }
}
