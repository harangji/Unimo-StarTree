using System.Collections;
using UnityEngine;

public class Boss1State_Pattern_4 : BossState_Pattern
{
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
        foreach (var position in spawnPositions)
        {
            Instantiate(suicideBomberPrefab, position.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(1);

        controller.EnemyPreaction();
    }
}