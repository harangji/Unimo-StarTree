using UnityEngine;

public class Mon005State_Disappear : MonsterState_Disappear
{
    [SerializeField] private GameObject disappearFX;
    public override void TransitionAction(MonsterController controller)
    {
        base.TransitionAction(controller);
        disappearFX.SetActive(true);
    }
}