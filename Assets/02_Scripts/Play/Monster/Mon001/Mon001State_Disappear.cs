using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mon001State_Disappear : MonsterState_Disappear
{
    [SerializeField] private GameObject disapearFX;
    public override void TransitionAction(MonsterController controller)
    {
        base.TransitionAction(controller);
        disapearFX.SetActive(true);
    }
}
