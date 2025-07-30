using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class Gimmick_BlackHole : Gimmick
{
    [Header("��Ȧ ����")] 
    public override eGimmickType eGimmickType => eGimmickType.Dangerous;
    
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

    [SerializeField] private ParticleSystemRenderer[] particleSystemRenderers;

    private void OnEnable()
    {
        if (GimmickManager.Instance != null)
        {
            if (GimmickManager.Instance.UnimoPrefab.TryGetComponent(out Collider coll))
            {
                foreach (var psr in particleSystemRenderers)
                {
                    Material mat = psr.material;
                    Color color = mat.color;
                    color.a = 0f;
                    mat.color = color;
                }
                FadeAll(true, 1f);
                
                bPlayerRigidbody = coll.attachedRigidbody;
                bTimeElapsed = 0f; //��Ȧ �ð� �ʱ�ȭ
            }
            else
            {
                MyDebug.Log("There is no UnimoPrefab attached to this object.");
            }
        }
        else
        {
            bPlayerRigidbody = null;
            gameObject.SetActive(false);
        }
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
            bPlayerRigidbody.AddForce(direction * OuterSuctionGravityStrength[(int)eGimmickGrade], ForceMode.Acceleration);
        }
        else if (distance <= EffectiveRange && distance > EffectiveCenterRange) // ��Ȧ ���� && �߽� �������ٴ� �ٱ��ʿ� ��ġ
        {
            MyDebug.Log("Player In OuterSuction");
            direction = (transform.position - bPlayerRigidbody.position).normalized;
            bPlayerRigidbody.AddForce(direction * OuterSuctionGravityStrength[(int)eGimmickGrade], ForceMode.Acceleration);
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
        MyDebug.Log("Activate Gimmick");
        gameObject.SetActive(true);
    }

    public override void DeactivateGimmick()
    {
        MyDebug.Log("Deactivate Gimmick");
        FadeAll(false);
        // gameObject.SetActive(false);
    }

    void FadeAll(bool fadeIn, float duration = 1f)
    {
        float targetAlpha = fadeIn? 1f : 0f;
        Sequence fadeSequence = DOTween.Sequence();
        
        foreach (ParticleSystemRenderer particleSystemRenderer in particleSystemRenderers)
        {
            Material mat = particleSystemRenderer.material;
            Color color = mat.color;

            float startAlpha = color.a;

            Tween tween = DOTween.To(() => startAlpha, x =>
            {
                startAlpha = x;
                color.a = x;
                mat.color = color;
            }, targetAlpha, duration);
            
            fadeSequence.Join(tween); // ���ÿ� ����ǵ��� �������� �߰�
        }

        fadeSequence.OnComplete(() =>
        {
            MyDebug.Log($"Fade to {targetAlpha} Complete");
            gameObject.SetActive(fadeIn);
        });
    }
}