using System;
using UnityEngine;

public class Gimmick_BlackHole : Gimmick
{
    [Header("중력 세기")]
    public float gravityStrength = 10f;
    
    [Header("검사 대상 레이어")]
    public LayerMask playerLayer;
    
    private Rigidbody target; // 감지한 플레이어 물리
    private float timeElapsed; // 시작 시간
    
    private void Start()
    {
        timeElapsed = 0f;
    }
    
    private void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= gimmickDuration)
        {
            Destroy(gameObject);
        }
        
        if(target == null) return;
        
        Vector3 direction = (transform.position - target.position).normalized;
        target.AddForce(direction * gravityStrength, ForceMode.Acceleration);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"GimmickOnTriggerEnter : {other.gameObject.name}");
        if (other.gameObject.layer != playerLayer) return;
        if (other.attachedRigidbody != null)
        {
            target = other.attachedRigidbody;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != playerLayer) return;
        target = null;
    }

    
    public override void InitializeGimmick()
    {
    }

    public override void ExcuteGimmick()
    {
    }
    
    private void OnDrawGizmosSelected()
    {
        Renderer rend = GetComponent<Renderer>();
        if (rend == null) return;

        Bounds bounds = rend.bounds;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }
}
