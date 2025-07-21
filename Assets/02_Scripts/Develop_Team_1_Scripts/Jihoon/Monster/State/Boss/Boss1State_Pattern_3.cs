using System.Collections;
using UnityEngine;

public class Boss1State_Pattern_3 : BossState_Pattern
{
    public override void TransitionAction(MonsterController controller)
    {
        base.TransitionAction(controller);
        Debug.Log("[Boss Pattern] 패턴 3");
    }
    public override void UpdateAction()
    {
        base.UpdateAction();
        Debug.Log("[Boss Pattern] 패턴 3 실행중");

        StartCoroutine(Pattern());
    }

    private IEnumerator Pattern()
    {
        yield return new WaitForSeconds(2);
        
        controller.EnemyPreaction();
    }
}