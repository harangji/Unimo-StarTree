// using System;
// using UnityEngine;
//
// public class Gimmick_BlackHole : Gimmick
// {
//     [Header("�߷� ����")]
//     public float gravityStrength = 10f;
//     
//     [Header("�˻� ��� ���̾�")]
//     public LayerMask playerLayer;
//     
//     [Header("OBB ���� ����")]
//     public Vector3 boxSize = new Vector3(5f, 0f, 5f);  // ��Ȧ ���μ��� ����
//     
//     private Rigidbody target; // ������ �÷��̾� ����
//     private float timeElapsed; // ���� �ð�
//     
//     private void Start()
//     {
//         timeElapsed = 0f;
//     }
//
//     private void Update()
//     {
//         timeElapsed += Time.deltaTime;
//
//         // OBB �浹 �˻� (�ڽ� �߽�, ũ��/2, ȸ��, ���̾�)
//         Collider[] hits = Physics.OverlapBox(
//             transform.position,                  // �߽�
//             boxSize * 0.5f,                      // �� ũ�� (halfExtents)
//             transform.rotation,                 // ȸ�� ����
//             playerLayer                         // ��� ���̾�
//         );
//
//         target = null;
//         foreach (var hit in hits)
//         {
//             if (hit.attachedRigidbody != null)
//             {
//                 target = hit.attachedRigidbody;
//                 break;
//             }
//         }
//
//         if (target != null)
//         {
//             Vector3 direction = (transform.position - target.position).normalized;
//             target.AddForce(direction * gravityStrength, ForceMode.Acceleration);
//         }
//
//         if (timeElapsed >= GimmickDuration)
//         {
//             Destroy(gameObject);
//         }
//     }
//
//     private void OnDrawGizmosSelected()
//     {
//         Gizmos.color = Color.green;
//         Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
//         Gizmos.DrawWireCube(Vector3.zero, boxSize);
//     }
// }
