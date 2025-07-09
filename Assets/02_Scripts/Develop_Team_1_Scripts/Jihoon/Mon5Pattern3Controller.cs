using System;
using UnityEngine;

public class Mon5Pattern3Controller : MonoBehaviour
{
    [Header("���� 3 ����")] 
    [SerializeField] [Tooltip("�̵� �ӵ�")] private float moveSpeed;
    [SerializeField] [Tooltip("�ʴ� ���� Ƚ��")] private float waveFrequency = 2f;
    [SerializeField] [Tooltip("����(= ���� ũ��)")] private float waveAmplitude = 1f;

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

        // �̵� ���
        Vector3 forwardMove = moveDir.normalized * (moveSpeed * time);
        float offset = Mathf.Sin(time * waveFrequency) * waveAmplitude;
        Vector3 waveOffset = Vector3.Cross(moveDir.normalized, Vector3.up) * offset;

        Vector3 newPos = originPos + forwardMove + waveOffset;

        // ȸ��: ���� �̵� ���� ����
        Vector3 velocity = newPos - prevPos;
        if (velocity != Vector3.zero)
        {
            float angleY = Mathf.Atan2(velocity.x, velocity.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, angleY, 0f);
        }

        // ��ġ ����
        transform.position = newPos;
        prevPos = newPos;
    }
}