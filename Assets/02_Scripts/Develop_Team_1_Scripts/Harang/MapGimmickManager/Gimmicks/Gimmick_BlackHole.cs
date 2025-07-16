using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class Gimmick_BlackHole : Gimmick
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

        float distance = Vector3.Distance(transform.position, PlayerRigidbody.position); //끌어당길 방향 설정 (플레이어가 블랙홀을 바라보는 방향)
        
        if (distance <= effectiveRange && distance > effectiveCenterRange) // 블랙홀 내부 && 중심 구역보다는 바깥쪽에 위치
        {
            MyDebug.Log("Player In");
            Vector3 direction = (transform.position - PlayerRigidbody.position).normalized;
            PlayerRigidbody.AddForce(direction * gravityStrength, ForceMode.Acceleration);
        }
        else if(distance <= effectiveCenterRange || distance > effectiveRange) //블랙홀 중심 구역에 위치 or 블랙홀 빠져나감
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