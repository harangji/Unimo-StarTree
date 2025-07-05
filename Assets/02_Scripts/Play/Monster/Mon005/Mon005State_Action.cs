using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mon005State_Action : MonsterState_Action
{
    [SerializeField] private float moveSpeed = 4.2f;
    [SerializeField] private int moveDir_0x1y2z = 1;
    [SerializeField] private int moveFrontBack = -1;
    private Vector3 moveVec;
    public override void TransitionAction(MonsterController controller)
    {
        base.TransitionAction(controller);
        setMoveDirVector();
        //moveSpeed *= Random.Range(0.75f, 1.2f);
    }
    public override void UpdateAction()
    {
        controller.transform.position += moveSpeed * Time.deltaTime * moveVec;
        if (controller.transform.position.y < -2f * controller.transform.localScale.y) { controller.EnemyExplode(); }
    }
    private void setMoveDirVector()
    {
        Vector3 moveDir = moveDir_0x1y2z switch
        {
            0 => Vector3.right,
            1 => Vector3.up,
            2 => Vector3.forward,
            _ => Vector3.up
        };
        moveVec = moveFrontBack * moveDir;
    }
}
