using System;
using System.Collections;
using UnityEngine;

public class BossState_PreAction : MonsterState_Preaction
{
    [Header("�̵� ����")]
    [SerializeField] private float moveSpeed = 4.2f;
    
    [Header("���� ����")]
    [SerializeField] private float[] skillCoolTime = new float[4];

    private float[] currentTime = new float[4];

    private bool canPattern;
    
    #region ��Ÿ�ӿ� MonoBehaviour

    private void Start()
    {
        for (int i = 0; i < 1; i++)
        {
            currentTime[i] = 0;
        }
    }

    private void Update()
    {
        //�ð� �߰� ����
        for (int i = 0; i < 1; i++)
        {
            currentTime[i] += Time.deltaTime;
        }
    }

    #endregion

    public override void TransitionAction(MonsterController controller)
    {
        base.TransitionAction(controller);
        StartCoroutine(WaitForNextPattern());
    }
    
    public override void UpdateAction()
    {
        RotateMonster();
        MoveMonster();

        if (!canPattern) return;
        
        var bossController = controller as BossController;
        
        for (int i = bossController.bossPatterns.Length - 1; i >= 0; i--)
        {
            if (currentTime[i] >= skillCoolTime[i]) //��ų ������� �� á�°�? and �������� ��ų�� �ִ°�?
            {
                //�ϴ� �� �ʱ�ȭ
                currentTime[i] = 0;

                //�������� ���� ����
                bossController.EnemyPattern(bossController.bossPatterns[i]);
                return;
            }
        }
    }

    #region ���� �Լ�

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

    private IEnumerator WaitForNextPattern()
    {
        yield return new WaitForSeconds(2);

        canPattern = true;
    }
    #endregion
    
}
