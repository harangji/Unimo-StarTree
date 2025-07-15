using UnityEngine;

public class BossState_PreAction : MonsterState_Preaction
{
    [SerializeField] private float moveSpeed = 4.2f;
    
    public override void UpdateAction()
    {
        RotateMonster();
        
        controller.transform.position += moveSpeed * Time.deltaTime * transform.forward;
    }
    
    //============= �Ʒ��� ���� �Լ� =============//
    private void RotateMonster()
    {
        Debug.Log("ȸ�� ��");

        var obj = controller.gameObject;
        
        Vector3 targetPos = controller.playerTransform.position;
        targetPos.y = transform.position.y; // ���� ���� ����
        obj.transform.LookAt(targetPos);
    }
}
