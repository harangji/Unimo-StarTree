using System.Collections;
using UnityEngine;

public class Boss1State_Pattern_1 : BossState_Pattern
{
    [SerializeField] private GameObject pattern1;
    
    public override void TransitionAction(MonsterController controller)
    {
        base.TransitionAction(controller);
        Debug.Log("[Boss Pattern] ∆–≈œ 1");

        StartCoroutine(Pattern());
    }
    public override void UpdateAction()
    {
        base.UpdateAction();
    }

    private IEnumerator Pattern()
    {
        Instantiate(pattern1,  transform.position + transform.forward * 2f, Quaternion.identity);
        
        yield return new WaitForSeconds(2);
        
        controller.EnemyPreaction();
    }
}