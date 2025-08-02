using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

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
    private Rigidbody mPlayerRigidbody { get; set; }
    
    private void OnEnable()
    {
        mbDeactivateStart = false;
        
        if (GimmickManager.Instance != null)
        {
            if (GimmickManager.Instance.UnimoPrefab.TryGetComponent(out Collider coll))
            {
                mPlayerRigidbody = coll.attachedRigidbody;
                mbTimeElapsed = 0f; //��Ȧ �ð� �ʱ�ȭ
            }
            else
            {
                MyDebug.Log("There is no UnimoPrefab attached to this object.");
            }
        }
        else
        {
            mPlayerRigidbody = null;
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        base.Update();
    }

    //cash
    private float mDistance;
    private Vector3 mDirection;
    private bool mbFirstInnerSuction = false;
    
    private void FixedUpdate()
    {
        if (mPlayerRigidbody == null || !mbReadyExecute) return;
        
        //������ ���� ���� (�÷��̾ ��Ȧ�� �ٶ󺸴� ����) + �Ÿ� ���
        mDistance = Vector3.Distance(transform.position, mPlayerRigidbody.position); 
        
        if(mDistance <= EffectiveCenterRange) //��Ȧ �߽� ������ ��ġ 
        {
            //���� ���� ���� //�߾ӿ� ���� ù �����ӿ� �޴� �� �ʱ�ȭ
            if (!mbFirstInnerSuction) //���� �����ӿ��� ���� �� ����
            {
                mPlayerRigidbody.linearVelocity = Vector3.zero;
                mbFirstInnerSuction = true;
            }
            
            mDirection = (transform.position - mPlayerRigidbody.position).normalized;
            mPlayerRigidbody.AddForce(mDirection * InnerSuctionGravityStrength[(int)emGimmickGrade], ForceMode.Acceleration);
        }
        else if (mDistance <= EffectiveRange && mDistance > EffectiveCenterRange) // ��Ȧ ���� && �߽� �������ٴ� ��¦ �ٱ��ʿ� ��ġ
        {
            mbFirstInnerSuction = false;
            mDirection = (transform.position - mPlayerRigidbody.position).normalized;
            mPlayerRigidbody.AddForce(mDirection * OuterSuctionGravityStrength[(int)emGimmickGrade], ForceMode.Acceleration);
        }
        else if (mDistance > EffectiveRange) //��Ȧ ��������
        {
            mbFirstInnerSuction = false;
            mPlayerRigidbody.linearVelocity = Vector3.zero;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, EffectiveRange);
    }
    
    public override async void ActivateGimmick()
    {
        Vector2 randomPos = Random.insideUnitCircle * ( PlaySystemRefStorage.mapSetter.MaxRange - 2 );
        gameObject.transform.position = new Vector3(randomPos.x, 0, randomPos.y);
        
        gameObject.SetActive(true);
        await FadeAll(true);
        mbReadyExecute = true;
    }

    public override async void DeactivateGimmick()
    {
        mPlayerRigidbody.linearVelocity = Vector3.zero;
        mPlayerRigidbody = null;
        mbReadyExecute = false;
        
        await FadeAll(false);
        gameObject.SetActive(false);
    }
}