using UnityEngine;

public class BossState_Pattern : MonsterState
{
    protected void RotateMonster()
    {
        var obj = controller.gameObject;
        
        Vector3 targetPos = controller.playerTransform.position;
        targetPos.y = transform.position.y; // 수직 높이 고정
        obj.transform.LookAt(targetPos);
    }
}