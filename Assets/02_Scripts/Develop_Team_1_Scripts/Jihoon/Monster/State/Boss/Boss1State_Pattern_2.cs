using System.Collections;
using UnityEngine;

public class Boss1State_Pattern_2 : BossState_Pattern
{
    public override void TransitionAction(MonsterController controller)
    {
        base.TransitionAction(controller);
        Debug.Log("[Boss Pattern] ���� 2");
    }
    public override void UpdateAction()
    {
        base.UpdateAction();
        Debug.Log("[Boss Pattern] ���� 2 ������");

        StartCoroutine(Pattern());
    }

    private IEnumerator Pattern()
    {
        yield return new WaitForSeconds(2);
        
        controller.EnemyPreaction();
    }
}