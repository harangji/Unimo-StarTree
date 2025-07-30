using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class Gimmick_RedZone : Gimmick
{
    [Header("레드존 설정")]
    
    //SerializeField
    // [field: SerializeField, LabelText("끌어당기는 힘"), Tooltip("블랙홀이 끌어당기는 정도"), Required, Space]
    // private float[] OuterSuctionGravityStrength { get; set; } = { 10.0f, 12.0f, 15.0f, 20.0f };
    //
    // [field: SerializeField, LabelText("안쪽 끌어당기는 힘"), Tooltip("블랙홀이 끌어당기는 정도"), Required, Space]
    // private float[] InnerSuctionGravityStrength { get; set; } = { 15.0f, 20.0f, 25.0f, 30.0f };
    
    public override eGimmickType eGimmickType => eGimmickType.Dangerous;
    
    [field: SerializeField, LabelText("블랙홀 크기"), Tooltip("중력 영향을 받지 않는 폭풍의 눈 구역"), Required, Space]
    private float EffectiveRange { get; set; } = 5.0f;

    [field: SerializeField, LabelText("블랙홀 중심 안전 구역"), Tooltip("중력 영향을 받지 않는 폭풍의 눈 구역"), Required, Space]
    private float EffectiveCenterRange { get; set; } = 0.2f;

    //private
    private Rigidbody bPlayerRigidbody { get; set; }
    private float bTimeElapsed { get; set; } = 0f;


    private void OnEnable()
    {
        if (GimmickManager.Instance != null)
        {
            if (GimmickManager.Instance.UnimoPrefab.TryGetComponent(out Collider coll))
            {
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
        FadeOutAll(1f);
        // gameObject.SetActive(false);
    }
    
    void FadeOutAll(float duration)
    {
        ParticleSystem[] renderers = gameObject.transform.GetComponentsInChildren<ParticleSystem>();
        
        foreach (var rend in renderers)
        {
            Material mat = rend.GetComponent<ParticleSystemRenderer>().material;
            Color color = mat.color;
            DOTween.To(() => color.a, x => {
                color.a = x;
                mat.color = color;
            }, 0f, duration).OnComplete(() =>
            {
                MyDebug.Log("Fade Out Complete");
                bPlayerRigidbody = null;
                gameObject.SetActive(false);
            });
        }
    }
}