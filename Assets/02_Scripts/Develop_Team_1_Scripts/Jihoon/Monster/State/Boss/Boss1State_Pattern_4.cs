using System.Collections;
using UnityEngine;

public class Boss1State_Pattern_4 : BossState_Pattern
{
    public override void TransitionAction(MonsterController controller)
    {
        base.TransitionAction(controller);
        Debug.Log("[Boss Pattern] ∆–≈œ 4");
    }
    public override void UpdateAction()
    {
        base.UpdateAction();

        StartCoroutine(Pattern());
    }

    private IEnumerator Pattern()
    {
        yield return new WaitForSeconds(2);
        
        controller.EnemyPreaction();
    }
}