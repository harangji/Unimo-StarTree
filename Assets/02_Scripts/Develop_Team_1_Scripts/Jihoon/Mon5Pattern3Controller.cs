using System;
using UnityEngine;

public class Mon5Pattern3Controller : MonoBehaviour
{
    [Header("패턴 3 설정")] 
    [SerializeField] [Tooltip("이동 속도")] private float moveSpeed;
    [SerializeField] [Tooltip("초당 진동 횟수")] private float waveFrequency = 2f;
    [SerializeField] [Tooltip("진폭(= 세로 크기)")] private float waveAmplitude = 1f;

    public Vector3 moveDir = Vector3.forward;

    private Vector3 originPos;
    private Vector3 prevPos;

    private Patterns currentPattern;

    private void Awake()
    {
        currentPattern = GetComponentInChildren<MonsterController>().pattern;
    }

    private void Start()
    {
        originPos = transform.position;
        prevPos = originPos;
    }

    private void Update()
    {
        float time = Time.time;

        // 이동 계산
        Vector3 forwardMove = moveDir.normalized * (moveSpeed * time);
        float offset = Mathf.Sin(time * waveFrequency) * waveAmplitude;
        Vector3 waveOffset = Vector3.Cross(moveDir.normalized, Vector3.up) * offset;

        Vector3 newPos = originPos + forwardMove + waveOffset;

        // 회전: 실제 이동 방향 기준
        Vector3 velocity = newPos - prevPos;
        if (velocity != Vector3.zero)
        {
            float angleY = Mathf.Atan2(velocity.x, velocity.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, angleY, 0f);
        }

        // 위치 갱신
        transform.position = newPos;
        prevPos = newPos;
    }
}