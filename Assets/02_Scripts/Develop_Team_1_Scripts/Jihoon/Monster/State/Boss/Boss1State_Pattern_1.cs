using System.Collections;
using UnityEngine;

public class Boss1State_Pattern_1 : BossState_Pattern
{
    private static readonly int PATTERN1 = Animator.StringToHash("Pattern1");
    
    [SerializeField] private GameObject pattern1;

    public override void TransitionAction(MonsterController controller)
    {
        base.TransitionAction(controller);
        Debug.Log("[Boss Pattern] ∆–≈œ 1");

        StartCoroutine(Pattern());
    }

    public override void UpdateAction()
    {
        RotateMonster();
    }

    private IEnumerator Pattern()
    {
        controller.enemyAnimator.SetTrigger(PATTERN1);
        
        for (int i = 0; i < 4; i++)
        {
            Instantiate(pattern1, transform.position + transform.forward * 5f, Quaternion.identity);

            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(1f);

        controller.EnemyPreaction();
    }
}