using System;
using System.Collections;
using UnityEngine;

public class Mon5PatternController : MonoBehaviour
{
    [Header("�⺻ ����")] [SerializeField] private float moveSpeed;

    [Header("���� 3 �߰� ����")] [SerializeField] [Tooltip("�ʴ� ���� Ƚ��")]
    private float waveFrequency = 2f;

    [SerializeField] [Tooltip("����(= ���� ũ��)")]
    private float waveAmplitude = 1f;

    private Vector3 moveDirection = Vector3.zero;
    private bool canMove = false;

    private Patterns currentPattern;

    private Vector3 originPos;
    private Vector3 prevPos;

    private void Awake()
    {

        // var names = GetComponentInChildren<MonsterController>().gameObject.name;
        // Debug.Log(names);
    }

    private IEnumerator Start()
    {
        currentPattern = GetComponentInChildren<MonsterController>().pattern;
        
        SetDirection();
        SetRotation();

        yield return new WaitForSeconds(1f);

        canMove = true;
    }

    private void Update()
    {
        if (!canMove) return;

        switch (currentPattern)
        {
            case Patterns.Pattern1:
                break;
            case Patterns.Pattern2:
                MovePattern2();
                break;
            case Patterns.Pattern3:
                MovePattern3();
                break;
        }
    }

    private void SetDirection()
    {
        var mover = FindAnyObjectByType<PlayerMover>();
        if (mover == null)
        {
            Debug.LogWarning("Mon5Controller: No player mover found");
            return;
        }

        Vector3 targetPos = mover.transform.position;
        moveDirection = targetPos - transform.position;

        // XZ ��鿡���� ���� ����
        moveDirection.y = 0f;
    }

    private void SetRotation()
    {
        // Y�� ȸ���� ���� (XZ ��鿡�� �ٶ󺸴� ����)
        if (moveDirection != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
        }
    }

    private void MovePattern2()
    {
        transform.position += moveDirection.normalized * (moveSpeed * Time.deltaTime);
    }

    private void MovePattern3()
    {
        float time = Time.time;

        // �̵� ���
        Vector3 forwardMove = moveDirection.normalized * (moveSpeed * time);
        float offset = Mathf.Sin(time * waveFrequency) * waveAmplitude;
        Vector3 waveOffset = Vector3.Cross(moveDirection.normalized, Vector3.up) * offset;

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