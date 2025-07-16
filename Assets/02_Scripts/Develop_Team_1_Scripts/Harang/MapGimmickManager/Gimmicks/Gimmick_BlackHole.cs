using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class Gimmick_BlackHole : Gimmick
{
    [Header("��Ȧ ����")]
    
    [Space]
    [LabelText("������� ��")] [Tooltip("��Ȧ�� ������� ����")] [Required]
    public float gravityStrength = 10f;

    [Space]
    [LabelText("��Ȧ ũ��")] [Tooltip("�߷� ������ ���� �ʴ� ��ǳ�� �� ����")] [Required]
    public float effectiveRange = 5f;
    
    [Space]
    [LabelText("��Ȧ �߽� ���� ����")] [Tooltip("�߷� ������ ���� �ʴ� ��ǳ�� �� ����")] [Required]
    public float effectiveCenterRange = 0.2f;
    
    private Rigidbody PlayerRigidbody {get; set;}
    private float TimeElapsed {get; set;}

    private void Start()
    {
        TimeElapsed = 0f;

        if (GameManager.Instance != null)
        {
            PlayerRigidbody = GameManager.Instance.unimoPrefab.GetComponent<Collider>().attachedRigidbody;
        }
    }

    private void Update()
    {
        TimeElapsed += Time.deltaTime;
        if (TimeElapsed >= gimmickDuration)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void FixedUpdate()
    {
        if (PlayerRigidbody == null) return;

        float distance = Vector3.Distance(transform.position, PlayerRigidbody.position); //������ ���� ���� (�÷��̾ ��Ȧ�� �ٶ󺸴� ����)
        
        if (distance <= effectiveRange && distance > effectiveCenterRange) // ��Ȧ ���� && �߽� �������ٴ� �ٱ��ʿ� ��ġ
        {
            MyDebug.Log("Player In");
            Vector3 direction = (transform.position - PlayerRigidbody.position).normalized;
            PlayerRigidbody.AddForce(direction * gravityStrength, ForceMode.Acceleration);
        }
        else if(distance <= effectiveCenterRange || distance > effectiveRange) //��Ȧ �߽� ������ ��ġ or ��Ȧ ��������
        {
            MyDebug.Log("Player Out");
            PlayerRigidbody.linearVelocity = Vector3.zero;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, effectiveRange);
    }

    protected override void InitializeGimmick() { }

    public override void ExcuteGimmick() { }
}