using UnityEngine;

public class BossState_PreAction : MonsterState_Preaction
{
    [SerializeField] private float moveSpeed = 4.2f;
    
    public override void UpdateAction()
    {
        RotateMonster();
        MoveMonster();
    }
    
    //============= 아래는 내부 함수 =============//
    private void RotateMonster()
    {
        var obj = controller.gameObject;
        
        Vector3 targetPos = controller.playerTransform.position;
        targetPos.y = transform.position.y; // 수직 높이 고정
        obj.transform.LookAt(targetPos);
    }

    private void MoveMonster()
    {
        controller.transform.position += moveSpeed * Time.deltaTime * transform.forward;
    }
}
