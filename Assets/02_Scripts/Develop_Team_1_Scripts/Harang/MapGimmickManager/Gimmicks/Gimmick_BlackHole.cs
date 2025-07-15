using System;
using UnityEngine;

public class Gimmick_BlackHole : Gimmick
{
    [Header("중력 세기")]
    public float gravityStrength = 10f;

    [Header("중력 적용 반경")]
    public float effectiveRange = 5f;
    
    [Header("중심 반경")]
    public float effectiveCenterRange = 0.2f;
    
    // public GameObject player;
    public Rigidbody playerRigidbody;
    private float timeElapsed;

    private void Start()
    {
        timeElapsed = 0f;

        // // Rigidbody 캐시
        // if (player != null)
        // {
        //     playerRigidbody = player.GetComponent<Rigidbody>();
        //     if (playerRigidbody == null)
        //         Debug.LogWarning("Player에 Rigidbody가 없습니다.");
        // }
        // else
        // {
        //     Debug.LogWarning("Player 참조가 없습니다.");
        // }
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= gimmickDuration)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void FixedUpdate()
    {
        if (playerRigidbody == null) return;

        float distance = Vector3.Distance(transform.position, playerRigidbody.position);
        if (distance <= effectiveRange && distance > effectiveCenterRange) // effectiveRange 범위 안에 플레이어가 있음 & 중심점보다는 멀게 존재
        {
            Debug.Log("Player In");
            Vector3 direction = (transform.position - playerRigidbody.position).normalized;
            playerRigidbody.AddForce(direction * gravityStrength, ForceMode.Acceleration);
        }
        else if (distance <= effectiveCenterRange) //중심점 내에 존재
        {
            Debug.Log("Player In Center");
            playerRigidbody.linearVelocity = Vector3.zero;
        }
        else if(distance > effectiveRange) //효과 범위 바깥에 존재
        {
            Debug.Log("Player Out");
            playerRigidbody.linearVelocity = Vector3.zero;
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