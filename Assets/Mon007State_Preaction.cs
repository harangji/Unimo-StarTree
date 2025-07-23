using UnityEngine;

public class Mon007State_Preaction : MonsterState_Preaction
{
    [SerializeField] private float moveSpeed = 6f;

    public override void TransitionAction(MonsterController controller)
    {
        this.controller = controller;
        Debug.Log("[자폭병] 프리액션 진입");
    }
    public override void UpdateAction()
    {
        RotateMonster();
        MoveMonster();
        
        float distanceToPlayer = Vector3.Distance(controller.transform.position, controller.playerTransform.position);
        if (distanceToPlayer < 3f)
        {
            invokeActionState();
        }
    }
    
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
