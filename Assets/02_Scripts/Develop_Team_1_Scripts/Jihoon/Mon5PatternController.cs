using System;
using System.Collections;
using UnityEngine;

public class Mon5PatternController : MonoBehaviour
{
    [Header("�⺻ ����")] 
    [SerializeField] private float moveSpeed;

    [Header("���� 3 �߰� ����")] 
    [SerializeField] [Tooltip("�ʴ� ���� Ƚ��")]
    private float waveFrequency = 2f;

    [SerializeField] [Tooltip("����(= ���� ũ��)")]
    private float waveAmplitude = 1f;

    [SerializeField] private float rotateSpeed = 10f; // �� �ε巯�� ȸ�� �ӵ�

    private Vector3 moveDirection = Vector3.zero;
    private bool canMove = false;

    private Patterns currentPattern;

    private Vector3 originPos;
    private Vector3 prevPos;

    private float patternTime = 0f;
    private float existTime = 6f;

    private void Awake()
    {
        // Optional debugging
    }

    private IEnumerator Start()
    {
        currentPattern = GetComponentInChildren<MonsterController>().pattern;

        originPos = transform.position;
        prevPos = originPos;

        SetDirection();
        SetRotation();

        yield return new WaitForSeconds(1f);
        
        StartCoroutine(DelayDestroy());
        patternTime = 0f;
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
        patternTime += Time.deltaTime;

        Vector3 forwardMove = moveDirection.normalized * (moveSpeed * patternTime);
        float offset = Mathf.Sin(patternTime * waveFrequency) * waveAmplitude;
        Vector3 waveOffset = Vector3.Cross(moveDirection.normalized, Vector3.up) * offset;

        Vector3 newPos = originPos + forwardMove + waveOffset;

        // �ε巯�� ȸ�� ó��
        Vector3 velocity = newPos - prevPos;
        if (velocity != Vector3.zero)
        {
            float angleY = Mathf.Atan2(velocity.x, velocity.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, angleY, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
        }

        transform.position = newPos;
        prevPos = newPos;
    }

    private IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(existTime);
        
        Destroy(gameObject);
    }
}
