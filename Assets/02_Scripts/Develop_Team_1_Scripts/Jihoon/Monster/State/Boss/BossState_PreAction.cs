using UnityEngine;

public class BossState_PreAction : MonsterState_Preaction
{
    [SerializeField] private float moveSpeed = 4.2f;
    
    public override void UpdateAction()
    {
        RotateMonster();
        
        controller.transform.position += moveSpeed * Time.deltaTime * transform.forward;
    }
    
    //============= 아래는 내부 함수 =============//
    private void RotateMonster()
    {
        Debug.Log("회전 중");

        var obj = controller.gameObject;
        
        Vector3 targetPos = controller.playerTransform.position;
        targetPos.y = transform.position.y; // 수직 높이 고정
        obj.transform.LookAt(targetPos);
    }
}
