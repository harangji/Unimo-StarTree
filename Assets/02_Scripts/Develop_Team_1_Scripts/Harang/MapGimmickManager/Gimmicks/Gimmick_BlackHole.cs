using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

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
                bTimeElapsed = 0f; //블랙홀 시간 초기화
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
        if (bTimeElapsed >= bGimmickDuration) //지속 시간 경과 시 비활성
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

        distance = Vector3.Distance(transform.position, bPlayerRigidbody.position); //끌어당길 방향 설정 (플레이어가 블랙홀을 바라보는 방향)
        
        if(distance <= EffectiveCenterRange) //블랙홀 중심 구역에 위치 
        {
            MyDebug.Log("Player In InnerSuction");
            direction = (transform.position - bPlayerRigidbody.position).normalized;
            bPlayerRigidbody.AddForce(direction * OuterSuctionGravityStrength[(int)eGimmickGrade], ForceMode.Acceleration);
        }
        else if (distance <= EffectiveRange && distance > EffectiveCenterRange) // 블랙홀 내부 && 중심 구역보다는 바깥쪽에 위치
        {
            MyDebug.Log("Player In OuterSuction");
            direction = (transform.position - bPlayerRigidbody.position).normalized;
            bPlayerRigidbody.AddForce(direction * OuterSuctionGravityStrength[(int)eGimmickGrade], ForceMode.Acceleration);
        }
        else if (distance > EffectiveRange) //블랙홀 빠져나감
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
            
            fadeSequence.Join(tween); // 동시에 실행되도록 시퀀스에 추가
        }

        fadeSequence.OnComplete(() =>
        {
            MyDebug.Log($"Fade to {targetAlpha} Complete");
            gameObject.SetActive(fadeIn);
        });
    }
}