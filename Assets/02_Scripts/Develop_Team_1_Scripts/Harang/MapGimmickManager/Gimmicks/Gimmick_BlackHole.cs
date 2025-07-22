using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class Gimmick_BlackHole : Gimmick
{
    [Header("��Ȧ ����")]
    
    //SerializeField
    [field: SerializeField, LabelText("������� ��"), Tooltip("��Ȧ�� ������� ����"), Required, Space]
    private float[] OuterSuctionGravityStrength { get; set; } =
    {
        10f,
        12f,
        15f,
        20f
    };

    [field: SerializeField, LabelText("���� ������� ��"), Tooltip("��Ȧ�� ������� ����"), Required, Space]
    private float[] InnerSuctionGravityStrength { get; set; } =
    {
        15f,
        20f,
        25f,
        30f
    };
    
    [field: SerializeField, LabelText("��Ȧ ũ��"), Tooltip("�߷� ������ ���� �ʴ� ��ǳ�� �� ����"), Required, Space]
    private float EffectiveRange { get; set; } = 5f;

    [field: SerializeField, LabelText("��Ȧ �߽� ���� ����"), Tooltip("�߷� ������ ���� �ʴ� ��ǳ�� �� ����"), Required, Space]
    private float EffectiveCenterRange { get; set; } = 0.2f;

    //private
    private Rigidbody bPlayerRigidbody { get; set; }
    private float bTimeElapsed { get; set; }

    private void OnEnable()
    {
        bTimeElapsed = 0f;

        if (GameManager.Instance != null)
        {
            bPlayerRigidbody = GameManager.Instance.unimoPrefab.GetComponent<Collider>().attachedRigidbody;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        bTimeElapsed += Time.deltaTime;
        if (bTimeElapsed >= bGimmickDuration)
        {
            Destroy(gameObject);
            return;
        }
    }

    //cash
    private float distance;
    private Vector3 direction;
    
    private void FixedUpdate()
    {
        if (bPlayerRigidbody == null) return;

        distance = Vector3.Distance(transform.position, bPlayerRigidbody.position); //������ ���� ���� (�÷��̾ ��Ȧ�� �ٶ󺸴� ����)
        
        if(distance <= EffectiveCenterRange) //��Ȧ �߽� ������ ��ġ 
        {
            MyDebug.Log("Player In InnerSuction");
            direction = (transform.position - bPlayerRigidbody.position).normalized;
            bPlayerRigidbody.AddForce(direction * OuterSuctionGravityStrength[(int)ebGimmickGrade], ForceMode.Acceleration);
        }
        else if (distance <= EffectiveRange && distance > EffectiveCenterRange) // ��Ȧ ���� && �߽� �������ٴ� �ٱ��ʿ� ��ġ
        {
            MyDebug.Log("Player In OuterSuction");
            direction = (transform.position - bPlayerRigidbody.position).normalized;
            bPlayerRigidbody.AddForce(direction * OuterSuctionGravityStrength[(int)ebGimmickGrade], ForceMode.Acceleration);
        }
        else if (distance > EffectiveRange) //��Ȧ ��������
        {
            MyDebug.Log("Player Out");
            bPlayerRigidbody.linearVelocity = Vector3.zero;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, EffectiveRange);
    }

    public override void ActivateGimmick()
    {
        gameObject.SetActive(true);
    }

    public override void DeactivateGimmick()
    {
        gameObject.SetActive(false);
    }
}