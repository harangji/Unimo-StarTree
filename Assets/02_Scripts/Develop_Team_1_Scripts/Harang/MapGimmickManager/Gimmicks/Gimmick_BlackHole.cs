using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class Gimmick_BlackHole : Gimmick
{
    [Header("��Ȧ ����")]
    
    //SerializeField
    [field: SerializeField, LabelText("������� ��"), Tooltip("��Ȧ�� ������� ����"), Required, Space]
    private float[] OuterSuctionGravityStrength { get; set; } = { 10.0f, 12.0f, 15.0f, 20.0f };

    [field: SerializeField, LabelText("���� ������� ��"), Tooltip("��Ȧ�� ������� ����"), Required, Space]
    private float[] InnerSuctionGravityStrength { get; set; } = { 15.0f, 20.0f, 25.0f, 30.0f };
    
    [field: SerializeField, LabelText("��Ȧ ũ��"), Tooltip("�߷� ������ ���� �ʴ� ��ǳ�� �� ����"), Required, Space]
    private float EffectiveRange { get; set; } = 5.0f;

    [field: SerializeField, LabelText("��Ȧ �߽� ���� ����"), Tooltip("�߷� ������ ���� �ʴ� ��ǳ�� �� ����"), Required, Space]
    private float EffectiveCenterRange { get; set; } = 0.2f;

    //private
    private Rigidbody bPlayerRigidbody { get; set; }
    private float bTimeElapsed { get; set; } = 0f;

    private void Start()
    {
        if (GimmickManager.Instance != null)
        {
            if (GimmickManager.Instance.UnimoPrefab.TryGetComponent(out Collider coll))
            {
                bPlayerRigidbody = coll.attachedRigidbody;
            }
            else
            {
                MyDebug.Log("There is no UnimoPrefab attached to this object.");
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        bTimeElapsed = 0f; //��Ȧ �ð� �ʱ�ȭ
    }

    private void Update()
    {
        bTimeElapsed += Time.deltaTime;
        if (bTimeElapsed >= bGimmickDuration) //���� �ð� ��� �� ��Ȱ��
        {
            DeactivateGimmick();
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
        FadeOutAll(1f);
        // gameObject.SetActive(false);
    }
    
    void FadeOutAll(float duration)
    {
        Renderer[] renderers = gameObject.transform.GetComponentsInChildren<Renderer>();
        
        foreach (var rend in renderers)
        {
            Material mat = rend.material;
            Color color = mat.color;
            DOTween.To(() => color.a, x => {
                color.a = x;
                mat.color = color;
            }, 0f, duration).OnComplete(() =>
            {
                MyDebug.Log("Fade Out Complete");
                gameObject.SetActive(false);
            });
        }
    }
}