using UnityEngine;

public class BossState_Pattern : MonsterState
{
    protected void RotateMonster()
    {
        var obj = controller.gameObject;
        
        Vector3 targetPos = controller.playerTransform.position;
        targetPos.y = transform.position.y; // ���� ���� ����
        obj.transform.LookAt(targetPos);
    }
}