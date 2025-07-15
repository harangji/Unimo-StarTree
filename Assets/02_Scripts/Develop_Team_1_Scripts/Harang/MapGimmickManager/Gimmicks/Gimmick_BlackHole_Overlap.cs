using System;
using UnityEngine;

public class Gimmick_BlackHole_Overlap : Gimmick
{
    private Rigidbody target;
    private Rigidbody lastTarget;
    
    private float timeElapsed;

    [Header("�߷� ����")]
    public float gravityStrength = 20f;

    [Header("���� �ð�")]
    public float duration = 3f;

    [Header("�߷� ����")]
    public float radius = 5f;

    [Header("�˻� ��� ���̾�")]
    public LayerMask playerLayer;

    private void Start()
    {
        timeElapsed = 0f;
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;

        // ���� �� �÷��̾� Ž��
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, playerLayer);

        target = null;
        foreach (var hit in hits)
        {
            if (hit.attachedRigidbody != null)
            {
                target = hit.attachedRigidbody;
                break; // �ϳ��� ����
            }
        }

        // �÷��̾ ���� ���� �߷� �ۿ�
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
        // �����Ϳ��� ���� �ð�ȭ
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public override void ExcuteGimmick()
    {
        
    }
}
