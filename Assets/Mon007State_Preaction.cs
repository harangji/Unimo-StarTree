using UnityEngine;

public class Mon007State_Preaction : MonsterState_Preaction
{
    [SerializeField] private float moveSpeed = 6f;

    public override void TransitionAction(MonsterController controller)
    {
        this.controller = controller;
        Debug.Log("[������] �����׼� ����");
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
        targetPos.y = transform.position.y; // ���� ���� ����
        obj.transform.LookAt(targetPos);
    }

    private void MoveMonster()
    {
        controller.transform.position += moveSpeed * Time.deltaTime * transform.forward;
    }
}
