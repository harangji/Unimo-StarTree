using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class Gimmick_BlackHoleTween : Gimmick
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
    
    private Sequence gravitySequence;

    private void Awake()
    {
        if (GameManager.Instance != null)
        {
            PlayerRigidbody = GimmickManager.Instance.unimoPrefab.GetComponent<Collider>().attachedRigidbody;
        }

        CreateGravitySequence();
    }
    
    private void CreateGravitySequence()
    {
        gravitySequence = DOTween.Sequence().Pause().SetAutoKill(false);
        gravitySequence.SetUpdate(UpdateType.Late); //LateUpdate���� ����
        
        // ��Ȧ ���� �ð� ���� �ֱ������� �߷� ȿ�� ����
        // gravitySequence.AppendInterval(0.05f); // 50ms �������� üũ (FixedUpdate ��ü)
        gravitySequence.AppendCallback(ApplyGravity);
        gravitySequence.SetLoops(-1, LoopType.Restart);

        // ������ ����
        gravitySequence.OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }

    private void ApplyGravity()
    {
        if (PlayerRigidbody == null) return;

        float distance = Vector3.Distance(transform.position, PlayerRigidbody.position); //������ ���� ���� (�÷��̾ ��Ȧ�� �ٶ󺸴� ����)

        if (distance <= effectiveRange && distance > effectiveCenterRange)  // ��Ȧ ���� && �߽� �������ٴ� �ٱ��ʿ� ��ġ
        {
            MyDebug.Log("Player In");
            Vector3 direction = (transform.position - PlayerRigidbody.position).normalized;
            PlayerRigidbody.AddForce(direction * gravityStrength, ForceMode.Acceleration);
        }
        else //��Ȧ �߽� ������ ��ġ or ��Ȧ ��������
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

    public override void ActivateGimmick()
    {
        gameObject.SetActive(true);
        gravitySequence.Restart();
    }

    public override void DeactivateGimmick()
    {
        gravitySequence.Kill();
        gameObject.SetActive(false);
    }
}