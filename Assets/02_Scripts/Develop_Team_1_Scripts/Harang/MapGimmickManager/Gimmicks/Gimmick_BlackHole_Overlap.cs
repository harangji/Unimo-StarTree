using System;
using UnityEngine;

public class Gimmick_BlackHole_Overlap : Gimmick
{
    private Rigidbody target;
    private Rigidbody lastTarget;
    
    private float timeElapsed;

    [Header("중력 세기")]
    public float gravityStrength = 20f;

    [Header("지속 시간")]
    public float duration = 3f;

    [Header("중력 범위")]
    public float radius = 5f;

    [Header("검사 대상 레이어")]
    public LayerMask playerLayer;

    private void Start()
    {
        timeElapsed = 0f;
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;

        // 범위 내 플레이어 탐지
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, playerLayer);

        target = null;
        foreach (var hit in hits)
        {
            if (hit.attachedRigidbody != null)
            {
                target = hit.attachedRigidbody;
                break; // 하나만 선택
            }
        }

        // 플레이어가 있을 때만 중력 작용
        if (target != null)
        {
            Debug.Log("Player Catch");
            lastTarget = target;
            Vector3 direction = (transform.position - target.position).normalized;
            target.AddForce(direction * gravityStrength, ForceMode.Acceleration);
        }
        else
        {
            Debug.Log("Player Out");
            lastTarget.linearVelocity = Vector3.zero;
            lastTarget = null;
        }


        if (timeElapsed >= duration)
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // 에디터에서 범위 시각화
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public override void ExcuteGimmick()
    {
        
    }
}
