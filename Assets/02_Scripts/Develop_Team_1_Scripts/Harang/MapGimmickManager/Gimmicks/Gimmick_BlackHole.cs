using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Gimmick_BlackHole : Gimmick
{
    [Header("블랙홀 설정")] 
    public override eGimmickType eGimmickType => eGimmickType.Dangerous;
    
    //SerializeField
    [field: SerializeField, LabelText("끌어당기는 힘"), Tooltip("블랙홀이 끌어당기는 정도"), Required, Space]
    private float[] OuterSuctionGravityStrength { get; set; } = { 10.0f, 12.0f, 15.0f, 20.0f };

    [field: SerializeField, LabelText("안쪽 끌어당기는 힘"), Tooltip("블랙홀이 끌어당기는 정도"), Required, Space]
    private float[] InnerSuctionGravityStrength { get; set; } = { 15.0f, 20.0f, 25.0f, 30.0f };
    
    [field: SerializeField, LabelText("블랙홀 크기"), Tooltip("중력 영향을 받지 않는 폭풍의 눈 구역"), Required, Space]
    private float EffectiveRange { get; set; } = 5.0f;

    [field: SerializeField, LabelText("블랙홀 중심 안전 구역"), Tooltip("중력 영향을 받지 않는 폭풍의 눈 구역"), Required, Space]
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
                mbTimeElapsed = 0f; //블랙홀 시간 초기화
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
        
        //끌어당길 방향 설정 (플레이어가 블랙홀을 바라보는 방향) + 거리 재기
        mDistance = Vector3.Distance(transform.position, mPlayerRigidbody.position); 
        
        if(mDistance <= EffectiveCenterRange) //블랙홀 중심 구역에 위치 
        {
            //로프 현상 방지 //중앙에 들어온 첫 프레임에 받는 힘 초기화
            if (!mbFirstInnerSuction) //이전 프레임에서 들어온 적 없음
            {
                mPlayerRigidbody.linearVelocity = Vector3.zero;
                mbFirstInnerSuction = true;
            }
            
            mDirection = (transform.position - mPlayerRigidbody.position).normalized;
            mPlayerRigidbody.AddForce(mDirection * InnerSuctionGravityStrength[(int)emGimmickGrade], ForceMode.Acceleration);
        }
        else if (mDistance <= EffectiveRange && mDistance > EffectiveCenterRange) // 블랙홀 내부 && 중심 구역보다는 살짝 바깥쪽에 위치
        {
            mbFirstInnerSuction = false;
            mDirection = (transform.position - mPlayerRigidbody.position).normalized;
            mPlayerRigidbody.AddForce(mDirection * OuterSuctionGravityStrength[(int)emGimmickGrade], ForceMode.Acceleration);
        }
        else if (mDistance > EffectiveRange) //블랙홀 빠져나감
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