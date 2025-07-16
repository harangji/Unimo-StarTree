using System.Collections;
using UnityEngine;

public class BossState_Action : MonsterState_Action
{
    private bool isDoing = false; 
    
    public override void TransitionAction(MonsterController controller)
    {
        base.TransitionAction(controller);
        isDoing = false;
    }
    
    public override void UpdateAction()
    {
        if (isDoing) return;
        
        for (int i = 0; i < 4; i++)
        {
            if (BossSkillCoolTimeManager.isSkillDo[i])
            {
                isDoing = true;
                StartCoroutine(wait(i));
                return;
            }
        }
    }
    
    //============= 내부 로직 =============//
    // private void DoSkill(int idx)
    // {
    //     switch (idx)
    //     {
    //         case 0:
    //             Debug.Log("[Boss Pattern] 패턴 1 동작!");
    //             StartCoroutine(wait(0));
    //             break;
    //         case 1:
    //             Debug.Log("[Boss Pattern] 패턴 2 동작!");
    //             StartCoroutine(wait(1));
    //             break;
    //         case 2:
    //             Debug.Log("[Boss Pattern] 패턴 3 동작!");
    //             StartCoroutine(wait(2));
    //             break;
    //         case 3:
    //             StartCoroutine(wait());
    //             break;
    //         default:
    //             Debug.Log("[Boss Pattern] 말도 안되는 소리하고 있노");
    //             StartCoroutine(wait());
    //             break;
    //     }
    // }

    private IEnumerator wait(int idx)
    {
        Debug.Log($"[Boss Pattern] 패턴 {idx + 1} 동작!");
        
        yield return new WaitForSeconds(2f);
        
        BossSkillCoolTimeManager.isSkillDo[idx] = false;
        controller.EnemyPreaction();
    }
}
