using System.Collections;
using UnityEngine;

public class Boss1State_Pattern_4 : BossState_Pattern
{
    private static readonly int PATTERN4 = Animator.StringToHash("Pattern4");
    [SerializeField] private GameObject suicideBomberPrefab;

    [SerializeField] private Transform[] spawnPositions;

    public override void TransitionAction(MonsterController controller)
    {
        base.TransitionAction(controller);
        Debug.Log("[Boss Pattern] ∆–≈œ 4");

        StartCoroutine(Pattern());
    }

    private IEnumerator Pattern()
    {
        controller.enemyAnimator.SetTrigger(PATTERN4);
        
        foreach (var position in spawnPositions)
        {
            var monster = Instantiate(suicideBomberPrefab, position.position, Quaternion.identity);
            var ctrl = monster.GetComponent<MonsterController>();
            ctrl.InitEnemy(controller.playerTransform);
        }

        yield return new WaitForSeconds(1);

        controller.EnemyPreaction();
    }
}