using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class Gimmick_BlackHoleTween : Gimmick
{
    [Header("블랙홀 설정")]
    
    [Space]
    [LabelText("끌어당기는 힘")] [Tooltip("블랙홀이 끌어당기는 정도")] [Required]
    public float gravityStrength = 10f;

    [Space]
    [LabelText("블랙홀 크기")] [Tooltip("중력 영향을 받지 않는 폭풍의 눈 구역")] [Required]
    public float effectiveRange = 5f;
    
    [Space]
    [LabelText("블랙홀 중심 안전 구역")] [Tooltip("중력 영향을 받지 않는 폭풍의 눈 구역")] [Required]
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
        gravitySequence.SetUpdate(UpdateType.Late); //LateUpdate에서 동작
        
        // 블랙홀 지속 시간 동안 주기적으로 중력 효과 적용
        // gravitySequence.AppendInterval(0.05f); // 50ms 간격으로 체크 (FixedUpdate 대체)
        gravitySequence.AppendCallback(ApplyGravity);
        gravitySequence.SetLoops(-1, LoopType.Restart);

        // 끝나면 삭제
        gravitySequence.OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }

    private void ApplyGravity()
    {
        if (PlayerRigidbody == null) return;

        float distance = Vector3.Distance(transform.position, PlayerRigidbody.position); //끌어당길 방향 설정 (플레이어가 블랙홀을 바라보는 방향)

        if (distance <= effectiveRange && distance > effectiveCenterRange)  // 블랙홀 내부 && 중심 구역보다는 바깥쪽에 위치
        {
            MyDebug.Log("Player In");
            Vector3 direction = (transform.position - PlayerRigidbody.position).normalized;
            PlayerRigidbody.AddForce(direction * gravityStrength, ForceMode.Acceleration);
        }
        else //블랙홀 중심 구역에 위치 or 블랙홀 빠져나감
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