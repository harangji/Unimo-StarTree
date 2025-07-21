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
    
    //============= ���� ���� =============//
    // private void DoSkill(int idx)
    // {
    //     switch (idx)
    //     {
    //         case 0:
    //             Debug.Log("[Boss Pattern] ���� 1 ����!");
    //             StartCoroutine(wait(0));
    //             break;
    //         case 1:
    //             Debug.Log("[Boss Pattern] ���� 2 ����!");
    //             StartCoroutine(wait(1));
    //             break;
    //         case 2:
    //             Debug.Log("[Boss Pattern] ���� 3 ����!");
    //             StartCoroutine(wait(2));
    //             break;
    //         case 3:
    //             StartCoroutine(wait());
    //             break;
    //         default:
    //             Debug.Log("[Boss Pattern] ���� �ȵǴ� �Ҹ��ϰ� �ֳ�");
    //             StartCoroutine(wait());
    //             break;
    //     }
    // }

    private IEnumerator wait(int idx)
    {
        Debug.Log($"[Boss Pattern] ���� {idx + 1} ����!");
        
        yield return new WaitForSeconds(2f);
        
        BossSkillCoolTimeManager.isSkillDo[idx] = false;
        controller.EnemyPreaction();
    }
}
