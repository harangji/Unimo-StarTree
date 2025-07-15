using System;
using UnityEngine;

public class Gimmick_BlackHole : Gimmick
{
    [Header("�߷� ����")]
    public float gravityStrength = 10f;

    [Header("�߷� ���� �ݰ�")]
    public float effectiveRange = 5f;
    
    [Header("�߽� �ݰ�")]
    public float effectiveCenterRange = 0.2f;
    
    // public GameObject player;
    public Rigidbody playerRigidbody;
    private float timeElapsed;

    private void Start()
    {
        timeElapsed = 0f;

        // // Rigidbody ĳ��
        // if (player != null)
        // {
        //     playerRigidbody = player.GetComponent<Rigidbody>();
        //     if (playerRigidbody == null)
        //         Debug.LogWarning("Player�� Rigidbody�� �����ϴ�.");
        // }
        // else
        // {
        //     Debug.LogWarning("Player ������ �����ϴ�.");
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
        if (distance <= effectiveRange && distance > effectiveCenterRange) // effectiveRange ���� �ȿ� �÷��̾ ���� & �߽������ٴ� �ְ� ����
        {
            Debug.Log("Player In");
            Vector3 direction = (transform.position - playerRigidbody.position).normalized;
            playerRigidbody.AddForce(direction * gravityStrength, ForceMode.Acceleration);
        }
        else if (distance <= effectiveCenterRange) //�߽��� ���� ����
        {
            Debug.Log("Player In Center");
            playerRigidbody.linearVelocity = Vector3.zero;
        }
        else if(distance > effectiveRange) //ȿ�� ���� �ٱ��� ����
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