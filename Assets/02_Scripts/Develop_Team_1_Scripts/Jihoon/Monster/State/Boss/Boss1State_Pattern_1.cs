using UnityEngine;

public class Boss1State_Pattern_1 : BossState_Pattern
{
    public override void TransitionAction(MonsterController controller)
    {
        base.TransitionAction(controller);
        Debug.Log("[Boss Pattern] ���� 1");
    }
    public override void UpdateAction()
    {
        base.UpdateAction();
        Debug.Log("[Boss Pattern] ���� 1 ������");
        controller.EnemyPreaction();
    }
}